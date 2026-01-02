/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace SK.Framework.Resource
{
    public class AssetBundleBuilder
    {
        [SerializeField] private BuildTabData m_Data;
        private string m_CacheFilePath;
        private Vector2 m_ScrollPosition;
        private string[] m_EncryptStrategies;
        private int m_CurrentStrategyIndex;
        private AnimBool m_EncryptAnimBool;

        public void OnEnable(EditorWindow window)
        {
            m_CacheFilePath = Path.GetFullPath(".").Replace("\\", "/") + "/Library/Asset Bundle Build Tab Data.dat";
            if (!CacheLoad(m_CacheFilePath, out m_Data))
                m_Data = new BuildTabData();
            m_EncryptStrategies = typeof(AssetBundleEncryptStrategy).Assembly.GetTypes().Where(
                m => m.IsSubclassOf(typeof(AssetBundleEncryptStrategy)))
                    .ToArray()
                    .Select(m => m.FullName)
                    .ToArray();
            m_CurrentStrategyIndex = string.IsNullOrEmpty(m_Data.encrypt.strategy)
                ? -1 : Array.FindIndex(m_EncryptStrategies, m => m == m_Data.encrypt.strategy);
            m_EncryptAnimBool = new AnimBool(m_Data.encrypt.enable, window.Repaint);
        }
        public void OnDisable()
        {
            CacheSave(m_CacheFilePath);
        }

        private bool CacheLoad(string filePath, out BuildTabData data)
        {
            data = null;
            if (File.Exists(filePath))
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open))
                {
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        var deserialize = bf.Deserialize(fs);
                        data = deserialize as BuildTabData;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        File.Delete(filePath);
                        return false;
                    }
                }
            }
            return false;
        }
        private void CacheSave(string filePath)
        {
            using (FileStream fs = File.Create(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, m_Data);
            }
        }

        public void OnGUI()
        {
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);

            OnBuildSettingsGUI();
            EditorGUILayout.Space();
            OnOptionsGUI();
            EditorGUILayout.Space();
            OnEncryptGUI();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Build"))
            {
                if (EditorUtility.DisplayDialog("提醒",
                        "构建AssetsBundle将花费一定时间，是否确定开始？", "确定", "取消"))
                {
                    BuildAssetBundle();
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndScrollView();
        }
        private void OnBuildSettingsGUI()
        {
            m_Data.version = EditorGUILayout.IntField("Version", m_Data.version);
            m_Data.buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", m_Data.buildTarget);
            GUILayout.BeginHorizontal();
            m_Data.outputPath = EditorGUILayout.TextField("Output Path", m_Data.outputPath);
            if (GUILayout.Button("Browse", GUILayout.Width(60f)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("AssetsBundle构建路径", Application.dataPath, string.Empty);
                if (!string.IsNullOrEmpty(selectedPath))
                    m_Data.outputPath = selectedPath;
            }
            GUILayout.EndHorizontal();
            m_Data.compressionType = (BuildTabData.CompressionType)EditorGUILayout.EnumPopup("Compression", m_Data.compressionType);
            m_Data.copy2StreamingAssets = EditorGUILayout.Toggle(GUIContents.copy2StreamingAssets, m_Data.copy2StreamingAssets);
        }
        private void OnOptionsGUI()
        {
            GUILayout.Label("Build Options", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            GUILayout.BeginVertical();
            m_Data.buildOptions.disableWriteTypeTree = GUILayout.Toggle(
                m_Data.buildOptions.disableWriteTypeTree, GUIContents.disableWriteTypeTree);
            m_Data.buildOptions.forceRebuildAssetBundle = GUILayout.Toggle(
                m_Data.buildOptions.forceRebuildAssetBundle, GUIContents.forceRebuildAssetBundle);
            m_Data.buildOptions.ignoreTypeTreeChanges = GUILayout.Toggle(
                m_Data.buildOptions.ignoreTypeTreeChanges, GUIContents.ignoreTypeTreeChanges);
            m_Data.buildOptions.appendHashToAssetBundleName = GUILayout.Toggle(
                m_Data.buildOptions.appendHashToAssetBundleName, GUIContents.appendHash);
            m_Data.buildOptions.strictMode = GUILayout.Toggle(
                m_Data.buildOptions.strictMode, GUIContents.strictMode);
            m_Data.buildOptions.dryRunBuild = GUILayout.Toggle(
                m_Data.buildOptions.dryRunBuild, GUIContents.dryRunBuild);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void OnEncryptGUI()
        {
            GUILayout.Label("Encrypt", EditorStyles.boldLabel);
            bool enable = EditorGUILayout.Toggle("Enable", m_Data.encrypt.enable);
            if (enable != m_Data.encrypt.enable)
            {
                m_Data.encrypt.enable = enable;
                m_EncryptAnimBool.target = enable;
            }
            if (EditorGUILayout.BeginFadeGroup(m_EncryptAnimBool.faded))
            {
                int strategyIndex = EditorGUILayout.Popup("Strategy", m_CurrentStrategyIndex, m_EncryptStrategies);
                if (m_CurrentStrategyIndex != strategyIndex)
                {
                    m_CurrentStrategyIndex = strategyIndex;
                    m_Data.encrypt.strategy = m_EncryptStrategies[m_CurrentStrategyIndex];
                }
                m_Data.encrypt.secretKey = EditorGUILayout.TextField(
                    "Secret Key", m_Data.encrypt.secretKey);
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void BuildAssetBundle()
        {
            try
            {
                DateTime beginTime = DateTime.Now;
                string outputPath = string.Format("{0}/AssetBundles", m_Data.outputPath);
                if (!Directory.Exists(outputPath))
                {
                    Debug.Log(string.Format("路径不存在，进行创建 {0}", outputPath));
                    Directory.CreateDirectory(outputPath);
                }

                AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
                    outputPath, m_Data.GetBuildOptions(), m_Data.buildTarget);
                string[] assetBundles = manifest.GetAllAssetBundles();

                if (m_Data.encrypt.enable && !string.IsNullOrEmpty(m_Data.encrypt.strategy))
                {
                    Type type = Type.GetType(m_EncryptStrategies[m_CurrentStrategyIndex]);
                    var strategy = Activator.CreateInstance(type) as AssetBundleEncryptStrategy;
                    EncryptAssetBundle(strategy, assetBundles, outputPath);
                }

                BuildAssetInfoMap(assetBundles, outputPath);

                if (m_Data.copy2StreamingAssets)
                {
                    if (!outputPath.StartsWith(Application.streamingAssetsPath))
                        CopyDirectory(outputPath, Application.streamingAssetsPath);
                    AssetDatabase.Refresh();
                }
                double duration = (DateTime.Now - beginTime).TotalSeconds;
                Debug.Log(string.Format("构建完成，耗时{0}s", duration));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void EncryptAssetBundle(AssetBundleEncryptStrategy strategy, string[] assetBundles, string outputPath)
        {
            for (int i = 0; i < assetBundles.Length; i++)
            {
                string filePath = outputPath + "/" + assetBundles[i];
                byte[] bytes = File.ReadAllBytes(filePath);
                if (strategy.Encrypt(ref bytes, m_Data.encrypt.secretKey))
                {
                    File.Delete(filePath);
                    File.WriteAllBytes(filePath, bytes);
                    if (m_Data.copy2StreamingAssets)
                        AssetDatabase.Refresh();
                }
            }
        }
        private void BuildAssetInfoMap(string[] assetBundles, string outputPath)
        {
            List<AssetInfo> assets = new List<AssetInfo>();
            List<AssetBundleInfo> abs = new List<AssetBundleInfo>();
            for (int i = 0; i < assetBundles.Length; i++)
            {
                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundles[i]);
                for (int j = 0; j < paths.Length; j++)
                {
                    assets.Add(new AssetInfo(null, paths[j], assetBundles[i]));
                }
                string filePath = outputPath + "/" + assetBundles[i];
                string md5 = MD5Utility.CalculateFileMD5(filePath);
                abs.Add(new AssetBundleInfo(assetBundles[i], md5, new FileInfo(filePath).Length));
            }
            string json = JsonUtility.ToJson(new AssetsInfo(m_Data.version.ToString(), assets, abs));
            string mapPath = Path.Combine(outputPath, "map.json");
            using (FileStream fs = File.Create(mapPath))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        private void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            string[] directories = Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories);
            for (int i = 0; i < directories.Length; i++)
            {
                string targetDir = directories[i].Replace(sourceDir, destDir);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
            }
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string fileDirName = Path.GetDirectoryName(filePath).Replace("\\", "/");
                string fileName = Path.GetFileName(filePath);
                string newFilePath = Path.Combine(fileDirName.Replace(sourceDir, destDir), fileName);
                File.Copy(filePath, newFilePath, true);
            }
        }

        [Serializable]
        internal class BuildTabData
        {
            public int version;
            public string outputPath;
            public enum CompressionType { Uncompressed, LZMA, LZ4 }
            public CompressionType compressionType;
            public BuildTarget buildTarget;
            public bool copy2StreamingAssets;
            public BuildOptions buildOptions;
            public EncryptSettings encrypt;

            public BuildTabData()
            {
                outputPath = Application.streamingAssetsPath;
                compressionType = CompressionType.LZ4;
                buildTarget = BuildTarget.StandaloneWindows;
                buildOptions = new BuildOptions();
                encrypt = new EncryptSettings() { secretKey = "10101100" };
            }

            public BuildAssetBundleOptions GetBuildOptions()
            {
                BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
                switch (compressionType)
                {
                    case CompressionType.Uncompressed:
                        options = BuildAssetBundleOptions.UncompressedAssetBundle;
                        break;
                    case CompressionType.LZMA:
                        options = BuildAssetBundleOptions.None;
                        break;
                    case CompressionType.LZ4:
                        options = BuildAssetBundleOptions.ChunkBasedCompression;
                        break;
                }
                if (buildOptions.disableWriteTypeTree)
                    options |= BuildAssetBundleOptions.DisableWriteTypeTree;
                if (buildOptions.forceRebuildAssetBundle)
                    options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
                if (buildOptions.ignoreTypeTreeChanges)
                    options |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
                if (buildOptions.appendHashToAssetBundleName)
                    options |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
                if (buildOptions.strictMode)
                    options |= BuildAssetBundleOptions.StrictMode;
                if (buildOptions.dryRunBuild)
                    options |= BuildAssetBundleOptions.DryRunBuild;
                return options;
            }
        }

        [Serializable]
        internal struct BuildOptions
        {
            public bool disableWriteTypeTree;
            public bool forceRebuildAssetBundle;
            public bool ignoreTypeTreeChanges;
            public bool appendHashToAssetBundleName;
            public bool strictMode;
            public bool dryRunBuild;
        }

        [Serializable]
        internal struct EncryptSettings
        {
            public bool enable;
            public string strategy;
            public string secretKey;
        }

        private class GUIContents
        {
            public static GUIContent copy2StreamingAssets = new GUIContent(
                "Copy 2 StreamingAssets", "Copy asset bundle to Assets/StreamingAssets after build completed for use in simulation mode.");
            public static GUIContent disableWriteTypeTree = new GUIContent(
                "Disable Write Type Tree", "Do not include type information within the AssetBundle.");
            public static GUIContent forceRebuildAssetBundle = new GUIContent(
                "Force Rebuild", "Force rebuild the assetBundles.");
            public static GUIContent ignoreTypeTreeChanges = new GUIContent(
                "Ignore Type Tree Changes", "Ignore the type tree changes when doing the incremental build check.");
            public static GUIContent appendHash = new GUIContent(
                "Append Hash", "Append the hash to to asset bundle name.");
            public static GUIContent strictMode = new GUIContent(
                "Strict Mode", " Do not allow the build to succeed if any errors are reporting during it.");
            public static GUIContent dryRunBuild = new GUIContent(
                "Dry Run Build", "Do a dry run build.");
        }
    }
}