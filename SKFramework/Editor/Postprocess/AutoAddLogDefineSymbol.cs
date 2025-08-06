/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace SK.Framework.Logger
{
    public class AutoAddLogDefineSymbol : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, 
            string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var flag = importedAssets.Any(path => path.EndsWith(
                "SKFramework/Runtime/Log/ILogger.cs", System.StringComparison.OrdinalIgnoreCase));
            if (flag)
            {
                var types = typeof(ILogger).Assembly.GetTypes()
                    .Where(m => !m.IsAbstract && typeof(ILogger).IsAssignableFrom(m));
                foreach (var type in types)
                {
                    var fields = type.GetFields(BindingFlags.Static | BindingFlags.NonPublic)
                        .Where(m => m.GetCustomAttribute<SymbolDefineAttribute>() != null);
                    foreach (var field in fields)
                    {
                        var symbol = field.GetValue(null) as string;
                        if (string.IsNullOrEmpty(symbol))
                            continue;
                        AddDefineSymbol(symbol);
                    }
                }
            }
        }

        private static void AddDefineSymbol(string symbol)
        {
            BuildTargetGroup[] targetGroups = new[]
            {
                BuildTargetGroup.Standalone,  
                BuildTargetGroup.Android,    
                BuildTargetGroup.iOS,       
                BuildTargetGroup.WebGL
            };

            foreach (var group in targetGroups)
            {
                if (!BuildPipeline.IsBuildTargetSupported(group, BuildTargetGroupToBuildTarget(group)))
                    continue;

                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                var defineList = defines.Split(';').ToList();
                if (!defineList.Contains(symbol))
                {
                    defineList.Add(symbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(
                        group,
                        string.Join(";", defineList)
                    );
                    Debug.Log($"Auto add define symbol [{symbol}] for [{group}].");
                }
            }
        }

        private static BuildTarget BuildTargetGroupToBuildTarget(BuildTargetGroup group)
        {
            switch (group)
            {
                case BuildTargetGroup.Standalone:
                    return EditorUserBuildSettings.activeBuildTarget;
                case BuildTargetGroup.Android:
                    return BuildTarget.Android;
                case BuildTargetGroup.iOS:
                    return BuildTarget.iOS;
                case BuildTargetGroup.WebGL:
                    return BuildTarget.WebGL;
                default:
                    return BuildTarget.NoTarget;
            }
        }
    }
}