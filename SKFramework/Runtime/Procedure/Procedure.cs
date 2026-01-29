/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.Procedure
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Procedure")]
    public class Procedure : ModuleBase
    {
        [SerializeField] private List<string> m_ProcedureNames = new List<string>(0);
        [SerializeField] private string m_ProcedureEntry;
        private readonly List<ProcedureBase> m_Procedures = new List<ProcedureBase>();
        private ILogger m_Logger;

        public ProcedureBase current { get; private set; }

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
            for (var i = 0; i < m_ProcedureNames.Count; i++)
            {
                var procedureName = m_ProcedureNames[i];
                var type = Type.GetType(procedureName);
                if (type == null)
                {
                    m_Logger.Error("[Procedure] Can not get the procedure type {0}", procedureName);
                    continue;
                }
                var instance = Activator.CreateInstance(type) as ProcedureBase;
                var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (Array.FindIndex(ctors, m => m.GetParameters().Length == 0) != -1
                    && Activator.CreateInstance(type) is ProcedureBase procedure)
                {
                    m_Procedures.Add(instance);
                    m_Logger.Info("[Procedure] Create {0}", procedureName);
                    continue;
                }
                m_Logger.Warning("[Procedure] A constructor with 0 arguments does not exist in the type {0}.", procedureName);
            }
            var entry = m_Procedures.FindIndex(m => m.GetType().FullName == m_ProcedureEntry);
            if (entry != -1)
                Switch(m_ProcedureEntry);
        }

        protected internal override void OnUpdate()
        {
            base.OnUpdate();
            current?.OnUpdate();
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            m_Procedures.Clear();
            m_Logger = null;
        }

        private void Switch(string procedureName, object data = null)
        {
            var target = m_Procedures.Find(m => m.GetType().FullName == procedureName);
            if (target == null)
            {
                m_Logger.Error("[Procedure] Procedure with type {0} does not exists.", procedureName);
                return;
            }
            if (current != null)
            {
                current.OnExit();
                m_Logger.Debug("[Procedure] Exit {0}", current.GetType().FullName);
            }
            current = target;
            m_Logger.Debug("[Procedure] Enter {0}", procedureName);
            current.OnEnter(data);
        }

        public void Switch<T>(object data = null) where T : ProcedureBase
        {
            Switch(typeof(T).FullName, data);
        }

        public bool Has<T>() where T : ProcedureBase
        {
            return m_Procedures.Find(m => m.GetType() == typeof(T)) != null;
        }

        public T Get<T>() where T : ProcedureBase
        {
            return  m_Procedures.Find(m => m.GetType() == typeof(T)) as T;
        }

        public bool TryGet<T>(out T procedure) where T : ProcedureBase
        {
            var index = m_Procedures.FindIndex(m => m.GetType() == typeof(T));
            procedure = index != -1 ? m_Procedures[index] as T : null;
            return index != -1;
        }

        public bool CurrentIs<T>() where T : ProcedureBase
        {
            return current is T;
        }
    }
}