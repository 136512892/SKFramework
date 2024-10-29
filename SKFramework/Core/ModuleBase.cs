/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public class ModuleBase : MonoBehaviour, IModule
    {
        public virtual void OnInitialization() { }

        public virtual void OnTermination() { }
    }
}