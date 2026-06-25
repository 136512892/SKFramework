/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    public class ViewScriptGenerator : EditorWindow
    {
        [MenuItem("SKFramework/UI/View Script Generator")]
        public static void Open()
        {
            var window = GetWindow<ViewScriptGenerator>();
            window.titleContent = new GUIContent("View Script Generator");
            window.Show();
        }

        private string m_Namespace;
        private GameObject m_ObjectView;
        private int m_IndentLevel = 0;
        private const int m_IndentSpaceCount = 4;
        private string m_Content;
        private Vector2 m_ScrollPosition;
        private static readonly Regex m_TypeBracketRegex = new Regex(@"^.*\[(?<type>[^\[\]]+?)\](?<varname>[^\[\]]*)$");
        private static readonly Regex m_ValidVarNameRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");

        private void OnEnable()
        {
            var key = $"{Application.productName}_ViewGenerator_Namespace";
            m_Namespace = EditorPrefs.HasKey(key) ? EditorPrefs.GetString(key) : null;
            m_ObjectView = GetValidSelectedViewObject();
            m_Content = string.Empty;
        }

        private void OnDisable()
        {
            var key = $"{Application.productName}_ViewGenerator_Namespace";
            if (string.IsNullOrWhiteSpace(m_Namespace))
                EditorPrefs.DeleteKey(key);
            else
                EditorPrefs.SetString(key, m_Namespace.Trim());
        }

        private void OnSelectionChange()
        {
            m_ObjectView = GetValidSelectedViewObject();
            m_Content = string.Empty;
            Repaint();
        }

        private void OnGUI()
        {
            if (m_ObjectView == null)
            {
                EditorGUILayout.HelpBox("Please select the UIView root objects whose names start with \"View\" in the Hierarchy.", MessageType.Warning);
                return;
            }

            var newNamespace = EditorGUILayout.TextField("namespace", m_Namespace);
            if (newNamespace != m_Namespace)
                m_Namespace = newNamespace.Trim().Replace(" ", "");
            if (GUILayout.Button("Preview"))
            {
                CodePreview();
            }
            if (!string.IsNullOrEmpty(m_Content))
            {
                m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
                EditorGUILayout.TextArea(m_Content);
                EditorGUILayout.EndScrollView();
                if (GUILayout.Button("Generate"))
                {
                    CodeGenerate();
                }
            }
        }

        private GameObject GetValidSelectedViewObject()
        {
            return Selection.activeGameObject != null
                && Selection.activeGameObject.name.StartsWith("View")
                    ? Selection.activeGameObject : null;
        }

        private void CodePreview()
        {
            if (string.IsNullOrEmpty(m_Namespace))
            {
                ShowNotification(new GUIContent("Please provide a valid namespace."));
                return;
            }
            m_IndentLevel = 0;
            var behaviours = m_ObjectView.GetComponentsInChildren<Behaviour>(true)
                .Where(m => m_TypeBracketRegex.IsMatch(m.name))
                .ToArray();
            var namespaces = new List<string>() { "UnityEngine", "SK.Framework.UI" };

            var sbField = new StringBuilder();
            var sbEvent = new StringBuilder();
            PushIndent();
            PushIndent();
            for (int i = 0; i < behaviours.Length; i++)
            {
                var behaviour = behaviours[i];
                var match = m_TypeBracketRegex.Match(behaviour.name);
                if (!match.Success)
                    continue;
                var type = match.Groups["type"].Value;
                var varRaw = match.Groups["varname"].Value.TrimStart('_');
                var varName = varRaw.StartsWith('_') ? varRaw[1..] : varRaw;

                if (!m_ValidVarNameRegex.IsMatch(varName)) 
                {
                    Debug.LogWarning($"The variable name {behaviour.name} of the control is invalid, skipping the generation.");
                    continue;
                }
                if (!behaviour.GetType().Name.Contains(type)) 
                    continue;

                var ns = behaviour.GetType().Namespace; 
                if (!string.IsNullOrEmpty(ns) && !namespaces.Contains(ns))
                    namespaces.Add(ns);

                var fieldName = $"m_{type}{varName}";
                AppendLineWithIndent(sbField, $"[SerializeField] private {behaviour.GetType().Name} {fieldName};");
                
                AppendEventMethod(behaviour, type, varName, sbEvent);
            }

            PopIndent();
            PopIndent();
            var sb = new StringBuilder();
            foreach (var ns in namespaces.OrderBy(m => m.Length))
                AppendLineWithIndent(sb, $"using {ns};");

            sb.AppendLine();
            AppendLineWithIndent(sb, $"namespace {m_Namespace}");
            AppendLineWithIndent(sb, "{");
            PushIndent();
            AppendLineWithIndent(sb, $"public class {m_ObjectView.name} : UIView");
            AppendLineWithIndent(sb, "{");
            sb.Append(sbField.ToString());
            if (sbEvent.Length > 0)
                sb.Append(sbEvent.ToString());
            AppendLineWithIndent(sb, "}");
            PopIndent();
            //AppendLineWithIndent(sb, "}");
            sb.Append("}");

            m_Content = sb.ToString();
        }

        private void PushIndent() => m_IndentLevel++;
        private void PopIndent() => m_IndentLevel--;

        private string GetCurrentIndent()
        {
            return new string(' ', m_IndentLevel * m_IndentSpaceCount);
        }

        private void AppendLineWithIndent(StringBuilder sb, string content)
        {
            sb.AppendLine($"{GetCurrentIndent()}{content}");
        }

        private void AppendEventMethod(Behaviour behaviour, string type, string name, StringBuilder sbEvent)
        {
            if (behaviour is Button button)
            {
                if (button.onClick.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}Clicked()");
                }
            }
            else if (behaviour is Toggle toggle)
            {
                if (toggle.onValueChanged.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}ValueChanged(bool isOn)");
                }
            }
            else if (behaviour is Slider slider)
            {
                if (slider.onValueChanged.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}ValueChanged(float value)");
                }
            }
            else if (behaviour is InputField input)
            {
                if (input.onEndEdit.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}EndEdit(string value)");

                }
                if (input.onValueChanged.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}ValueChanged(string value)");
                }
            }
            else if (behaviour is TMP_InputField tmpInput)
            {
                if (tmpInput.onEndEdit.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}EndEdit(string value)");
                }
                if (tmpInput.onValueChanged.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}ValueChanged(string value)");
                }
            }
            else if (behaviour is Dropdown dropdown)
            {
                if (dropdown.onValueChanged.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}ValueChanged(int value)");
                }
            }
            else if (behaviour is TMP_Dropdown tmpDropdown)
            {
                if (tmpDropdown.onValueChanged.GetPersistentEventCount() != 0)
                {
                    AppendEventMethod(sbEvent, $"public void On{type}{name}ValueChanged(int value)");
                }
            }
        }
        private void AppendEventMethod(StringBuilder sb, string content)
        {
            sb.AppendLine();
            AppendLineWithIndent(sb, content);
            AppendLineWithIndent(sb, "{");
            PushIndent();
            AppendLineWithIndent(sb, "");
            PopIndent();
            AppendLineWithIndent(sb, "}");
        }

        private void CodeGenerate()
        {
            var className = m_ObjectView.name;
            if (AppDomain.CurrentDomain.GetAssemblies().Any(m => m.GetType($"{m_Namespace}.{className}") != null))
            {
                ShowNotification(new GUIContent($"The script file named {className} already exists."));
                return;
            }
            var folder = EditorUtility.SaveFolderPanel("Save", Application.dataPath, string.Empty);
            if (string.IsNullOrEmpty(folder))
                return;
            var filePath = Path.Combine(folder, $"{className}.cs");
            File.WriteAllText(filePath, m_Content, Encoding.UTF8);
            AssetDatabase.Refresh();
            var assetPath = "Assets" + filePath[Application.dataPath.Length..];
            var target = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            EditorGUIUtility.PingObject(target);
        }
    }
}