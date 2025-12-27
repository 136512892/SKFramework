/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.Config
{
    public class CSVConfigLoader : ConfigLoader
    {
        private static readonly Regex m_CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public override Dictionary<int, T> Load<T>(string filePath) where T : class
        {
            var dic = new Dictionary<int, T>();
            TextAsset csv = Resources.Load<TextAsset>(filePath);
            if (csv == null)
            {
                ILogger logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
                logger.Error("Csv file not found: {0}", filePath);
                return dic;
            }
            ParseCSVText(csv.text, dic);
            return dic;
        }

        public override void LoadAsync<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted = null) where T : class
        {
            SKFramework.Module<Resource.Resource>().LoadAssetAsync<TextAsset>(filePath, (success, textAsset) =>
            {
                if (success)
                {
                    var dic = new Dictionary<int, T>();
                    ParseCSVText(textAsset.text, dic);
                    onCompleted?.Invoke(true, dic);
                }
                else
                {
                    onCompleted?.Invoke(false, null);
                }
            });
        }

        public override void LoadAsyncFromStreamingAssets<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted = null) where T : class
        {
            var path = Path.Combine(IOUtility.streamingAssetsPath, filePath);
            SKFramework.Module<Config>().StartCoroutine(LoadCoroutine(path, ParseCSVText, onCompleted));
        }

        private void ParseCSVText<T>(string text, Dictionary<int, T> dic) where T : class
        {
            string[] lines = text.Split('\n');
            if (lines.Length < 2)
                return;

            for (int i = 1; i < lines.Length; i++) //skip header(line 0)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;
                List<string> cells = ParseCSVLine(lines[i]);
                if (cells.Count == 0)
                    continue;
                T config = Activator.CreateInstance(typeof(T), cells) as T;
                int id = int.Parse(cells[0]);
                dic[id] = config;
            }
        }

        private List<string> ParseCSVLine(string line)
        {
            List<string> values = new List<string>();
            string[] cells = m_CSVParser.Split(line);
            for (int i = 0; i < cells.Length; i++)
            {
                string v = cells[i].Trim()
                    .TrimStart('"').TrimEnd('"')
                    .Replace("\\\"", "\"");
                values.Add(v);
            }
            return values;
        }
    }

#if UNITY_EDITOR
    public class CSVConfigClassGenerator
    {
        [MenuItem("SKFramework/Config/CSV Config Class Generate")]
        public static void Execute()
        {
            string folder = EditorUtility.OpenFolderPanel("Select the folder where the csv file is located",
                Application.dataPath, string.Empty);
            if (string.IsNullOrEmpty(folder))
                return;
            if (!folder.StartsWith(Application.dataPath))
            {
                Debug.Log("Please select a folder within the Assets directory.");
                return;
            }
            
            string[] csvFiles = Directory.GetFiles(folder, "*.csv", SearchOption.TopDirectoryOnly);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(m => m.GetTypes()).ToArray();
            for (int i = 0; i < csvFiles.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(csvFiles[i]);
                if (Array.FindIndex(types, m => m.Name == fileName) == -1)
                    ProcessCsvFile(csvFiles[i], folder);
                else
                {
                    if (EditorUtility.DisplayDialog(
                        "Warning", $"It has been detected that there is already a class named {fileName}. Should we proceed?",
                        "Continue", "Cancle"))
                    {
                        ProcessCsvFile(csvFiles[i], folder);
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        private static void ProcessCsvFile(string csvFile, string folder)
        {
            try
            {
                string content = File.ReadAllText(csvFile).Replace("\r", "");
                string[] lines = content.Split('\n')
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToArray();
                if (lines.Length < 2)
                {
                    Debug.LogWarning(string.Format("Invalid csv file:{0}", csvFile));
                    return;
                }

                List<string[]> rows = lines.Select(m => SplitCsvLine(m.Trim())).ToList();
                string[] headers = rows[0];
                List<List<string>> cols = InitializeColumns(headers.Length);
                PopulateColumns(rows, cols);
                string[] columnTypes = DetermineColumnTypes(cols);
                GenerateClassFile(csvFile, folder, headers, columnTypes);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error processing {csvFile} : {e.Message}");
            }
        }

        public static string[] SplitCsvLine(string line)
        {
            List<string> fields = new List<string>();
            StringBuilder sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i < line.Length - 1 && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            fields.Add(sb.ToString());
            return fields.ToArray();
        }

        private static List<List<string>> InitializeColumns(int count)
        {
            var columns = new List<List<string>>();
            for (int i = 0; i < count; i++)
            {
                columns.Add(new List<string>());
            }
            return columns;
        }

        private static void PopulateColumns(List<string[]> rows, List<List<string>> columns)
        {
            for (int i = 1; i < rows.Count; i++)
            {
                string[] row = rows[i];
                for (int j = 0; j < columns.Count; j++)
                {
                    string v = j < row.Length ? row[j] : string.Empty;
                    columns[j].Add(v);
                }
            }
        }

        private static string[] DetermineColumnTypes(List<List<string>> columns)
        {
            return columns.Select(col =>
            {
                if (col.All(IsInt)) return "int";
                if (col.All(IsFloat)) return "float";
                if (col.All(IsBool)) return "bool";
                return "string";
            }).ToArray();
        }
        
        private static bool IsInt(string v) => int.TryParse(v, out _);
        private static bool IsFloat(string v) => float.TryParse(v, out _);
        private static bool IsBool(string v) => Regex.IsMatch(v, @"^(true|false)$", RegexOptions.IgnoreCase);

        private static void GenerateClassFile(string csvFile, string folder, string[] headers, string[] columnTypes)
        {
            StringBuilder sb = new StringBuilder();
            AddScriptHeader(sb);
            string className = Path.GetFileNameWithoutExtension(csvFile);
            AddClassDefinition(sb, className, headers, columnTypes);
            SaveGeneratedFile(folder, className, sb);
        }

        private static void AddScriptHeader(StringBuilder sb)
        {
            sb.AppendLine("/***********************************************************");
            sb.AppendLine("* Auto-generated CSV class.");
            sb.AppendLine($"* Generated on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            sb.AppendLine("************************************************************/");
            sb.AppendLine();
        }

        private static void AddClassDefinition(StringBuilder sb, string className, string[] headers, string[] columnTypes)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;\r\n");

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public class {className}");
            sb.AppendLine("{");
            for (int i = 0; i < headers.Length; i++)
            {
                string fieldName = headers[i];
                sb.AppendLine($"    public {columnTypes[i]} {fieldName};");
            }

            sb.AppendLine($"    public {className}(List<string> cells)");
            sb.AppendLine("    {");
            for (int i = 0; i < headers.Length; i++)
            {
                string fieldName = headers[i];
                var typeName = columnTypes[i];
                switch (typeName)
                {
                    case "int":
                        sb.AppendLine($"        {fieldName} = int.Parse(cells[{i}]);");
                        break;
                    case "float":
                        sb.AppendLine($"        {fieldName} = float.Parse(cells[{i}]);");
                        break;
                    case "bool":
                        sb.AppendLine($"        {fieldName} = bool.Parse(cells[{i}]);");
                        break;
                    default:
                        sb.AppendLine($"        {fieldName} = cells[{i}];");
                        break;
                }
            }
            sb.AppendLine("    }");

            sb.AppendLine("}");
        }

        private static void SaveGeneratedFile(string folder, string className, StringBuilder sb)
        {
            string path = $"{folder}/{className}.cs";
            File.WriteAllText(path, sb.ToString());
            Debug.Log($"Generated config class: {className} at {path}");
        }
    }

    public class CSVConfigEditor : EditorWindow
    {
        [MenuItem("SKFramework/Config/CSV Editor")]
        public static void Open()
        {
            GetWindow<CSVConfigEditor>("CSV Editor").Show();
        }

        private class FileData
        {
            public string path;
            public List<string[]> data;

            public FileData(string path, List<string[]> data)
            {
                this.path = path;
                this.data = data;
            }
        }

        private string m_DirectoryPath;
        private bool m_IsInit;
        private Vector2 m_LScrollPosition, m_RScrollPosition;
        private float m_SplitterPosition;
        private bool m_IsSplitterDragging;
        private readonly Dictionary<string, FileData> m_FileDic = new Dictionary<string, FileData>();
        private string m_SelectedFile;
        private List<string[]> m_SelectedTable;
        private string m_SelectedFilePath;
        private int m_SelectedRowIndex = -1;
        private string m_SearchContent;

        private GUIStyle m_StyleHeader;
        private GUIStyle m_StyleRowLabel;
        private GUIContent m_GUIContentBrowse;
        private GUIContent m_GUIContentSave;

        private class Column
        {
            public string title;
            public float width;
            public bool isSplitterDragging;

            public Column(string title)
            {
                this.title = title;
                width = 100f;
            }
        }
        private readonly Dictionary<string, Column> m_ColumnsDic = new Dictionary<string, Column>();

        private void OnEnable()
        {
            string key = $"{Application.productName}-{nameof(CSVConfigEditor)}";
            var directory = EditorPrefs.HasKey(key) ? EditorPrefs.GetString(key) : Application.dataPath;
            OnDirectoryChanged(directory);
            if (m_SelectedFile != null && m_FileDic.ContainsKey(m_SelectedFile))
                OnSelectedFileChanged(m_SelectedFile);
            m_IsInit = false;
        }

        private void OnGUI()
        {
            if (!m_IsInit)
            {
                m_IsInit = true;
                m_SplitterPosition = position.width * .35f;
                m_StyleHeader = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    fixedHeight = 22f,
                };
                m_StyleRowLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                m_GUIContentBrowse = EditorGUIUtility.TrIconContent("FolderOpened On Icon", "Browse");
                m_GUIContentSave = EditorGUIUtility.TrIconContent("GUISkin Icon", "Save");
            }

            OnTopGUI();
            DrawSplitLine(2f, true);
            GUILayout.BeginHorizontal();
            OnLeftGUI();
            DrawSplitLine(2f, false, ref m_SplitterPosition, ref m_IsSplitterDragging);
            OnRightGUI();
            GUILayout.EndHorizontal();
        }

        private void OnDisable()
        {
            m_FileDic.Clear();
            m_ColumnsDic.Clear();
            EditorPrefs.SetString($"{Application.productName}-{nameof(CSVConfigEditor)}", m_DirectoryPath);
        }

        private void OnTopGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(30f));

            GUI.enabled = false;
            GUILayout.TextField(m_DirectoryPath, EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
            GUI.enabled = true;
            if (GUILayout.Button(m_GUIContentBrowse, EditorStyles.toolbarButton, GUILayout.Width(30f)))
            {
                var path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, string.Empty);
                if (Directory.Exists(path))
                    OnDirectoryChanged(path);
            }
            if (m_SelectedTable != null && GUILayout.Button(m_GUIContentSave, EditorStyles.toolbarButton, GUILayout.Width(30f)))
            {
                SaveCurrentFile();
            }
            GUILayout.EndHorizontal();
        }

        private void OnLeftGUI()
        {
            m_LScrollPosition = GUILayout.BeginScrollView(m_LScrollPosition,
                GUILayout.Width(m_SplitterPosition),
                GUILayout.MaxWidth(m_SplitterPosition),
                GUILayout.MinWidth(150f));

            m_SearchContent = GUILayout.TextField(m_SearchContent, EditorStyles.toolbarSearchField, GUILayout.Height(22f));

            if (m_FileDic.Count != 0)
            {
                var filteredFiles = string.IsNullOrEmpty(m_SearchContent)
                    ? m_FileDic.Keys.ToList()
                    : m_FileDic.Keys.Where(k => k.IndexOf(m_SearchContent, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                foreach (var file in filteredFiles)
                {
                    var style = m_SelectedFile == file ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop";
                    GUILayout.BeginHorizontal(style, GUILayout.Height(22f));
                    GUILayout.Label(file, GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();

                    var rect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.MouseDown
                        && Event.current.button == 0
                        && rect.Contains(Event.current.mousePosition)
                        && m_SelectedFile != file)
                    {
                        OnSelectedFileChanged(file);
                        Event.current.Use();
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No CSV files found in selected folder.", MessageType.Info);
            }

            GUILayout.EndScrollView();
        }

        private void OnRightGUI()
        {
            m_RScrollPosition = GUILayout.BeginScrollView(m_RScrollPosition, GUILayout.ExpandWidth(true));

            if (m_SelectedTable != null && m_SelectedTable.Count > 0)
            {
                //Header
                GUILayout.BeginHorizontal(GUILayout.Height(24f));
                GUILayout.Space(30f);
                for (int i = 0; i < m_SelectedTable[0].Length; i++)
                {
                    var header = m_SelectedTable[0][i];
                    var column = m_ColumnsDic[header];

                    GUILayout.Label(header, m_StyleHeader, GUILayout.Width(column.width + 5f));
                }
                GUILayout.EndHorizontal();

                //Body
                for (int i = 1; i < m_SelectedTable.Count; i++)
                {
                    var color = GUI.backgroundColor;
                    GUI.backgroundColor = i == m_SelectedRowIndex ? Color.cyan : color;
                    GUILayout.BeginHorizontal(GUILayout.Height(22f));
                    string[] line = m_SelectedTable[i];
                    GUILayout.Label(i.ToString(), m_StyleRowLabel, GUILayout.Width(30f));
                    for (int j = 0; j < line.Length; j++)
                    {
                        float width = m_ColumnsDic[m_SelectedTable[0][j]].width;
                        string newValue = EditorGUILayout.TextField(line[j],
                            EditorStyles.textField,
                            GUILayout.Width(width),
                            GUILayout.Height(20f));

                        if (newValue != line[j])
                            m_SelectedTable[i][j] = newValue;
                        DrawColumnSplitter(m_ColumnsDic[m_SelectedTable[0][j]]);
                    }
                    GUILayout.EndHorizontal();
                    GUI.backgroundColor = color;

                    if (Event.current.type == EventType.MouseDown 
                        && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        if (Event.current.button == 0)
                        {
                            m_SelectedRowIndex = i;
                            Repaint();
                        }
                        else if (Event.current.button == 1)
                        {
                            int index = i;
                            GenericMenu gm = new GenericMenu();
                            gm.AddItem(new GUIContent("Delete"), false, DeleteSelectedRow);
                            gm.AddItem(new GUIContent("Insert a row above"), false, () => AddNewRow(index, false));
                            gm.AddItem(new GUIContent("Insert a row below"), false, () => AddNewRow(index, true));
                            gm.ShowAsContext();
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select a CSV file from the left panel to edit.", MessageType.Info);
            }
            GUILayout.EndScrollView();
        }

        private void DrawSplitLine(float thickness, bool horizontal)
        {
            if (horizontal)
                GUILayout.Box(string.Empty, "EyeDropperHorizontalLine",
                    GUILayout.Height(thickness),
                    GUILayout.ExpandWidth(true));
            else
                GUILayout.Box(string.Empty, "EyeDropperVerticalLine",
                    GUILayout.Width(thickness),
                    GUILayout.ExpandHeight(true));
        }
        private void DrawSplitLine(float thickness, bool horizontal, ref float splitterPos,
            ref bool isDragging, float minPercent = .2f, float maxPercent = .8f, bool dirInvert = false)
        {
            DrawSplitLine(thickness, horizontal);
            Rect rect = GUILayoutUtility.GetLastRect();

            if (Event.current != null)
            {
                EditorGUIUtility.AddCursorRect(rect, horizontal ? MouseCursor.ResizeVertical : MouseCursor.ResizeHorizontal);
                switch (Event.current.rawType)
                {
                    case EventType.MouseDown:
                        isDragging = rect.Contains(Event.current.mousePosition);
                        break;
                    case EventType.MouseDrag:
                        if (isDragging)
                        {
                            splitterPos += (horizontal ? Event.current.delta.y : Event.current.delta.x)
                                * (dirInvert ? -1f : 1f);
                            splitterPos = Mathf.Clamp(
                                splitterPos,
                                (horizontal ? position.height : position.width) * minPercent,
                                (horizontal ? position.height : position.width) * maxPercent);
                            Repaint();
                        }
                        break;
                    case EventType.MouseUp:
                        isDragging = false;
                        break;
                }
            }
        }
        private void DrawColumnSplitter(Column column)
        {
            DrawSplitLine(2f, false);
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        column.isSplitterDragging = true;
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (column.isSplitterDragging)
                    {
                        column.width += Event.current.delta.x;
                        column.width = Mathf.Clamp(column.width, 20f, 200f);
                        Event.current.Use();
                        Repaint();
                    }
                    break;
                case EventType.MouseUp:
                    column.isSplitterDragging = false;
                    break;
            }
        }

        private void OnDirectoryChanged(string directoryPath)
        {
            m_DirectoryPath = directoryPath;
            m_FileDic.Clear();
            m_SelectedFile = null;
            m_SelectedTable = null;

            try
            {
                string[] csvFiles = Directory.GetFiles(m_DirectoryPath, "*.csv", SearchOption.AllDirectories);
                foreach (var filePath in csvFiles)
                {
                    string content = File.ReadAllText(filePath).Replace("\r", "");
                    string[] lines = content.Split('\n')
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .ToArray();
                    List<string[]> rows = lines.Select(m => CSVConfigClassGenerator.SplitCsvLine(m.Trim())).ToList();
                    if (rows.Count == 0)
                    {
                        rows.Add(new[] { "ID", "Field1", "Field2" });
                    }
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    m_FileDic[fileName] = new FileData(filePath, rows);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading CSV files: {e.Message}");
            }
        }
        private void OnSelectedFileChanged(string file)
        {
            m_SelectedFile = file;
            m_ColumnsDic.Clear();
            m_SelectedRowIndex = -1;

            if (m_FileDic.TryGetValue(file, out var fileData))
            {
                m_SelectedTable = fileData.data;
                m_SelectedFilePath = fileData.path;
                var headers = m_SelectedTable[0];
                foreach (var header in headers)
                {
                    m_ColumnsDic[header] = new Column(header);
                }
            }
        }

        private void AddNewRow(int index, bool below = true)
        {
            if (m_SelectedTable == null || m_SelectedTable.Count == 0) 
                return;

            int columnCount = m_SelectedTable[0].Length;
            string[] newRow = new string[columnCount];
            index += below ? 1 : 0;
            m_SelectedTable.Insert(index, newRow);
            m_SelectedRowIndex = index;
        }
        private void DeleteSelectedRow()
        {
            if (m_SelectedTable == null || m_SelectedRowIndex < 1 || m_SelectedRowIndex >= m_SelectedTable.Count)
                return;

            if (EditorUtility.DisplayDialog("Confirm Delete",
                $"Are you sure you want to delete row {m_SelectedRowIndex}?",
                "Delete", "Cancel"))
            {
                m_SelectedTable.RemoveAt(m_SelectedRowIndex);
                m_SelectedRowIndex = -1;
            }
        }
        private void SaveCurrentFile()
        {
            if (string.IsNullOrEmpty(m_SelectedFilePath) || m_SelectedTable == null) 
                return;
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var row in m_SelectedTable)
                {
                    string[] processedCells = row.Select(cell =>
                    {
                        if (cell.Contains(",") || cell.Contains("\""))
                        {
                            return $"\"{cell.Replace("\"", "\"\"")}\"";
                        }
                        return cell;
                    }).ToArray();
                    sb.AppendLine(string.Join(",", processedCells));
                }
                File.WriteAllText(m_SelectedFilePath, sb.ToString());
                Debug.Log($"Successfully saved: {m_SelectedFilePath}");
                EditorUtility.DisplayDialog("Success", "File saved successfully!", "OK");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save file: {e.Message}");
                EditorUtility.DisplayDialog("Error", $"Save failed: {e.Message}", "OK");
            }
        }
    }
#endif
}