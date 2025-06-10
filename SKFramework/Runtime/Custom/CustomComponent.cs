/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework.Custom
{
    public class CustomComponent : MonoBehaviour, ICustomComponent
    {
        void ICustomComponent.OnInitialization() => OnInitialization();

        void ICustomComponent.OnTermination() => OnTermination();

        protected virtual void OnInitialization()
        {

        }

        protected virtual void OnTermination()
        {

        }
    }
}