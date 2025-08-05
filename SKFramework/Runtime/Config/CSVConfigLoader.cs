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
            var path = Path.Combine(Application.streamingAssetsPath, filePath);
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
                        "Warn", $"It has been detected that there is already a class named {fileName}. Should we proceed?",
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
                    Debug.Log(string.Format("Invalid csv file:{0}", csvFile));
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

        private static string[] SplitCsvLine(string line)
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
#endif
}