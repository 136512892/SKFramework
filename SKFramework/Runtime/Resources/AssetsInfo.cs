/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using SK.Framework.UI;

namespace SK.Framework.Resource
{
    [Serializable]
    public class AssetsInfo
    {
        public string version;

        public List<AssetInfo> assets = new List<AssetInfo>(0);
        public List<AssetBundleInfo> assetBundles = new List<AssetBundleInfo>(0);
        public List<UIView.AssetInfo> viewAssets = new List<UIView.AssetInfo>(0);

        public AssetsInfo(string version, List<AssetInfo> assets, List<AssetBundleInfo> assetBundles, List<UIView.AssetInfo> viewAssets)
        {
            this.version = version;
            this.assets = assets;
            this.assetBundles = assetBundles;
            this.viewAssets = viewAssets;
        }
    }
}