using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEditor;
using UnityEngine;
using System.Text;

namespace SK.Framework.Resource
{
    public class AssetBundleBuilder : EditorWindow
    {
        [MenuItem("SKFramework/Resource/AssetBundle Builder")]
        public static void Open()
        {
            GetWindow<AssetBundleBuilder>("AssetBundle Builder").Show();
        }

        [SerializeField] private BuildTabData data;
        private Vector2 scroll;

        private void OnEnable()
        {
            //数据文件路径
            string dataPath = Path.GetFullPath(".").Replace("\\", "/") + "/Library/AssetBundleBuild.dat";
            //判断数据文件是否存在
            if (File.Exists(dataPath))
            {
                //打开文件
                using (FileStream fs = File.Open(dataPath, FileMode.Open))
                {
                    //反序列化
                    BinaryFormatter bf = new BinaryFormatter();
                    var deserialize = bf.Deserialize(fs);
                    if (deserialize != null)
                        data = deserialize as BuildTabData;
                    if (data == null)
                    {
                        File.Delete(dataPath);
                        data = new BuildTabData()
                        {
                            buildTarget = BuildTarget.StandaloneWindows,
                            outputPath = Application.streamingAssetsPath,
                            compressionType = BuildTabData.CompressionType.LZ4
                        };
                    }
                }
            }
            else
            {
                data = new BuildTabData()
                {
                    buildTarget = BuildTarget.StandaloneWindows,
                    outputPath = Application.streamingAssetsPath,
                    compressionType = BuildTabData.CompressionType.LZ4
                };
            }
        }

        private void OnDisable()
        {
            //数据文件路径
            string dataPath = Path.GetFullPath(".").Replace("\\", "/") + "/Library/AssetBundleBuild.dat";
            //写入数据文件进行保存
            using (FileStream fs = File.Create(dataPath))
            {
                //序列化
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            scroll = EditorGUILayout.BeginScrollView(scroll);

            //目标平台
            data.buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", data.buildTarget);
            GUILayout.BeginHorizontal();
            //输出路径
            data.outputPath = EditorGUILayout.TextField("Output Path", data.outputPath);
            //浏览按钮
            if (GUILayout.Button("Browse", GUILayout.Width(60f)))
            {
                //选择输出路径
                string selectedPath = EditorUtility.OpenFolderPanel("AssetsBundle构建路径", Application.dataPath, string.Empty);
                //判断没有Cancle(路径不为空)，更新输出路径
                if (!string.IsNullOrEmpty(selectedPath))
                    data.outputPath = selectedPath;
            }
            GUILayout.EndHorizontal();
            //压缩方式
            data.compressionType = (BuildTabData.CompressionType)EditorGUILayout.EnumPopup("Compression", data.compressionType);

            GUILayout.Space(20f);

            //是否拷贝至Assets/StreamingAssets
            data.copy2StreamingAssets = GUILayout.Toggle(data.copy2StreamingAssets, GUIContents.copy2StreamingAssets);

            EditorGUILayout.Space();

            //Options
            GUILayout.Label("Build Options", "BoldLabel");
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            GUILayout.BeginVertical();
            data.disableWriteTypeTree = GUILayout.Toggle(data.disableWriteTypeTree, GUIContents.disableWriteTypeTree);
            data.forceRebuildAssetBundle = GUILayout.Toggle(data.forceRebuildAssetBundle, GUIContents.forceRebuildAssetBundle);
            data.ignoreTypeTreeChanges = GUILayout.Toggle(data.ignoreTypeTreeChanges, GUIContents.ignoreTypeTreeChanges);
            data.appendHashToAssetBundleName = GUILayout.Toggle(data.appendHashToAssetBundleName, GUIContents.appendHash);
            data.strictMode = GUILayout.Toggle(data.strictMode, GUIContents.strictMode);
            data.dryRunBuild = GUILayout.Toggle(data.dryRunBuild, GUIContents.dryRunBuild);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            //构建按钮
            if (GUILayout.Button("Build"))
            {
                //提醒
                if (EditorUtility.DisplayDialog("提醒", "构建AssetsBundle将花费一定时间，是否确定开始？", "确定", "取消"))
                {
                    BuildAssetBundle();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        //将一个文件夹中的内容拷贝到另一个文件夹
        internal void CopyDirectory(string sourceDir, string destDir)
        {
            //如果目标文件夹不存在 创建文件夹
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            //获取源文件夹中的所有文件夹
            string[] directories = Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories);
            //遍历源文件夹中的所有文件夹
            for (int i = 0; i < directories.Length; i++)
            {
                //如果目标文件夹中不存在与源文件夹中对应的文件夹则创建
                string targetDir = directories[i].Replace(sourceDir, destDir);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
            }
            //获取源文件夹中的所有文件
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            //遍历源文件夹中的所有文件
            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string fileDirName = Path.GetDirectoryName(filePath).Replace("\\", "/");
                string fileName = Path.GetFileName(filePath);
                //路径拼接
                string newFilePath = Path.Combine(fileDirName.Replace(sourceDir, destDir), fileName);
                //拷贝文件
                File.Copy(filePath, newFilePath, true);
            }
        }

        private void BuildAssetBundle()
        {
            //检查路径是否存在
            if (!Directory.Exists(data.outputPath))
            {
                Debug.Log(string.Format("路径不存在，进行创建 {0}", data.outputPath));
                Directory.CreateDirectory(data.outputPath);
            }
            //Options
            BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
            switch (data.compressionType)
            {
                case BuildTabData.CompressionType.Uncompressed:
                    options = BuildAssetBundleOptions.UncompressedAssetBundle; break;
                case BuildTabData.CompressionType.LZMA:
                    options = BuildAssetBundleOptions.None; break;
                case BuildTabData.CompressionType.LZ4:
                    options = BuildAssetBundleOptions.ChunkBasedCompression; break;
            }
            if (data.disableWriteTypeTree) options |= BuildAssetBundleOptions.DisableWriteTypeTree;
            if (data.forceRebuildAssetBundle) options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
            if (data.ignoreTypeTreeChanges) options |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
            if (data.appendHashToAssetBundleName) options |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
            if (data.strictMode) options |= BuildAssetBundleOptions.StrictMode;
            if (data.dryRunBuild) options |= BuildAssetBundleOptions.DryRunBuild;

            //开始构建
            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(data.outputPath, options, data.buildTarget);
            //Map
            List<AssetInfo> map = new List<AssetInfo>();
            string[] assetBundleNames = manifest.GetAllAssetBundles();
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNames[i]);
                for (int j = 0; j < paths.Length; j++)
                {
                    map.Add(new AssetInfo(paths[j], assetBundleNames[i]));
                }
            }
            string json = JsonUtility.ToJson(new AssetsInfo(map));
            byte[] buffer = Encoding.Default.GetBytes(json);
            string mapPath = Path.Combine(data.outputPath, "map.dat");
            using (FileStream fs = File.Create(mapPath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, buffer);
            }

            //拷贝至Assets/StreamingAssets
            if (data.copy2StreamingAssets)
            {
                //如果输出路径本身已经是StreamingAssets路径 不处理
                if (!data.outputPath.StartsWith(Application.streamingAssetsPath))
                {
                    CopyDirectory(data.outputPath, Application.streamingAssetsPath);
                }
            }
            AssetDatabase.Refresh();
        }

        //数据类
        [Serializable]
        internal class BuildTabData
        {
            internal enum CompressionType { Uncompressed, LZMA, LZ4 }

            //输出路径
            internal string outputPath;
            //压缩方式
            internal CompressionType compressionType;
            //目标平台
            internal BuildTarget buildTarget;
            //是否拷贝至Assets/StreamingAssets
            internal bool copy2StreamingAssets;
            //Options
            internal bool disableWriteTypeTree;
            internal bool forceRebuildAssetBundle;
            internal bool ignoreTypeTreeChanges;
            internal bool appendHashToAssetBundleName;
            internal bool strictMode;
            internal bool dryRunBuild;
        }
        /// <summary>
        /// Asset资产信息
        /// </summary>
        [Serializable]
        public class AssetInfo
        {
            /// <summary>
            /// 资源路径
            /// </summary>
            public string path;

            /// <summary>
            /// AssetBundle包名称
            /// </summary>
            public string abName;

            public AssetInfo(string path, string abName)
            {
                this.path = path;
                this.abName = abName;
            }
        }
        [Serializable]
        public class AssetsInfo
        {
            public List<AssetInfo> list = new List<AssetInfo>();

            public AssetsInfo(List<AssetInfo> list)
            {
                this.list = list;
            }
        }

        private class GUIContents
        {
            public static GUIContent copy2StreamingAssets = new GUIContent("Copy To StreamingAssets", "Copy asset bundle to Assets/StreamingAssets after build completed for use in simulation mode.");
            public static GUIContent disableWriteTypeTree = new GUIContent("Disable Write Type Tree", "Do not include type information within the AssetBundle.");
            public static GUIContent forceRebuildAssetBundle = new GUIContent("Force Rebuild", "Force rebuild the assetBundles.");
            public static GUIContent ignoreTypeTreeChanges = new GUIContent("Ignore Type Tree Changes", "Ignore the type tree changes when doing the incremental build check.");
            public static GUIContent appendHash = new GUIContent("Append Hash", "Append the hash to to asset bundle name.");
            public static GUIContent strictMode = new GUIContent("Strict Mode", " Do not allow the build to succeed if any errors are reporting during it.");
            public static GUIContent dryRunBuild = new GUIContent("Dry Run Build", "Do a dry run build.");
        }
    }
}