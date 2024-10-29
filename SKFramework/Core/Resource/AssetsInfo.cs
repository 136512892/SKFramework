/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework.Resource
{
    [Serializable]
    public class AssetsInfo
    {
        public string version;

        public List<AssetInfo> list = new List<AssetInfo>(0);

        public AssetsInfo(string version, List<AssetInfo> list)
        {
            this.version = version;
            this.list = list;
        }
    }
}