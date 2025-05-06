/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public abstract class ModuleBase : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected internal int m_Priority;

        protected internal virtual void OnInitialization()
        {

        }

        protected internal virtual void OnTermination()
        {

        }
    }
}