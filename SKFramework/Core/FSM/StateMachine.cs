/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework.FSM
{
    public class StateMachine : IStateMachine
    {
        //状态列表 存储状态机内所有状态
        private readonly List<IState> m_States = new List<IState>(8);
        //状态切换条件列表
        private readonly List<StateSwitchCondition> m_Conditions = new List<StateSwitchCondition>(0);

        public string name { get; set; }

        public IState currentState { get; protected set; }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>0：添加成功； -1：状态已存在,无需重复添加； -2：存在同名状态，添加失败</returns>
        public int Add(IState state)
        {
            //判断是否已经存在
            if (!m_States.Contains(state))
            {
                //判断是否存在同名状态
                if (m_States.Find(m => m.name == state.name) == null)
                {
                    //存储到列表
                    m_States.Add(state);
                    //执行状态初始化事件
                    state.OnInitialization();
                    //设置状态所属的状态机
                    state.machine = this;
                    return 0;
                }
                return -2;
            }
            return -1;
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态命名</param>
        /// <returns>0：添加成功； -1：状态已存在,无需重复添加； -2：存在同名状态，添加失败</returns>
        public int Add<T>(string stateName = null) where T : IState, new()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            t.name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            return Add(t);
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <returns>true：移除成功； false：状态不存在，移除失败</returns>
        public bool Remove(string stateName)
        {
            //根据状态名称查找目标状态
            var target = m_States.Find(m => m.name == stateName);
            if (target != null)
            {
                //如果要移除的状态为当前状态 首先执行当前状态退出事件
                if (currentState == target)
                {
                    currentState.OnExit();
                    currentState = null;
                }
                //执行状态终止事件
                target.OnTermination();
                //从列表中移除
                m_States.Remove(target);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>true：移除成功； false：状态不存在，移除失败</returns>
        public bool Remove(IState state)
        {
            return Remove(state.name);
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>true：移除成功； false：状态不存在，移除失败</returns>
        public bool Remove<T>() where T : IState
        {
            return Remove(typeof(T).Name);
        }

        /// <summary>
        /// 状态是否存在
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <returns>true：存在  false：不存在</returns>
        public bool IsExists(string stateName)
        {
            return m_States.FindIndex(m => m.name == stateName) != -1;
        }

        /// <summary>
        /// 状态是否存在
        /// </summary>
        /// <returns>true：存在  false：不存在</returns>
        public bool IsExists<T>()
        {
            return IsExists(typeof(T).Name);
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态名称</param>
        /// <returns>状态</returns>
        public T Get<T>(string stateName) where T : IState, new()
        {
            var target = m_States.Find(m => m.name == stateName);
            return target != null ? (T)target : default;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>状态</returns>
        public T Get<T>() where T : IState, new()
        {
            return Get<T>(typeof(T).Name);
        }

        /// <summary>
        /// 尝试获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态名称</param>
        /// <param name="state">状态</param>
        /// <returns>true：获取成功  false：获取失败</returns>
        public bool TryGet<T>(string stateName, out T state) where T : IState, new()
        {
            int index = m_States.FindIndex(m => m.name == stateName);
            state = index != -1 ? (T)m_States[index] : default;
            return index != -1;
        }

        /// <summary>
        /// 尝试获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="state">状态</param>
        /// <returns>true：获取成功  false：获取失败</returns>
        public bool TryGet<T>(out T state) where T : IState, new()
        {
            return TryGet(typeof(T).Name, out state);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <param name="data">数据</param>
        /// <returns>0：切换成功； -1：状态不存在； -2：当前状态已经是切换的目标状态，并且该状态不可切换至自身</returns>
        public int Switch(string stateName, object data = null)
        {
            //根据状态名称在列表中查询
            var target = m_States.Find(m => m.name == stateName);
            if (target == null) return -1;
            //如果当前状态已经是切换的目标状态 并且该状态不可切换至自身 无需切换 返回false
            if (currentState == target && !target.canSwitch2Self) return -2;
            //当前状态不为空则执行状态退出事件
            currentState?.OnExit();
            //更新当前状态
            currentState = target;
            //更新后 执行状态进入事件
            currentState.OnEnter(data);
            return 0;
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <typeparam name="data">数据</typeparam>
        /// <returns>0：切换成功； -1：状态不存在； -2：当前状态已经是切换的目标状态，并且该状态不可切换至自身</returns>
        public int Switch<T>(object data = null) where T : IState
        {
            return Switch(typeof(T).Name, data);
        }

        /// <summary>
        /// 切换至下一状态
        /// </summary>
        /// <returns>true：切换成功； false：状态机中不存在任何状态，切换失败</returns>
        public bool Switch2Next()
        {
            if (m_States.Count != 0)
            {
                //如果当前状态不为空 则根据当前状态找到下一个状态
                if (currentState != null)
                {
                    int index = m_States.IndexOf(currentState);
                    //当前状态的索引值+1后若小于列表中的数量 则下一状态的索引为index+1
                    //否则表示当前状态已经是列表中的最后一个 下一状态则回到列表中的第一个状态 索引为0
                    index = index + 1 < m_States.Count ? index + 1 : 0;
                    IState targetState = m_States[index];
                    //首先执行当前状态的退出事件 再更新到目标状态
                    currentState.OnExit();
                    currentState = targetState;
                }
                //当前状态为空 则直接进入列表中的第一个状态
                else
                {
                    currentState = m_States[0];
                }
                //执行状态进入事件
                currentState.OnEnter();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 切换至上一状态
        /// </summary>
        /// <returns>true：切换成功； false：状态机中不存在任何状态，切换失败</returns>
        public bool Switch2Last()
        {
            if (m_States.Count != 0)
            {
                //如果当前状态不为空 则根据当前状态找到上一个状态
                if (currentState != null)
                {
                    int index = m_States.IndexOf(currentState);
                    //当前状态的索引值-1后若大等于0 则下一状态的索引为index-1
                    //否则表示当前状态是列表中的第一个 上一状态则回到列表中的最后一个状态
                    index = index - 1 >= 0 ? index - 1 : m_States.Count - 1;
                    IState targetState = m_States[index];
                    //首先执行当前状态的退出事件 再更新到目标状态
                    currentState.OnExit();
                    currentState = targetState;
                }
                //当前状态为空 则直接进入列表中的最后一个状态
                else
                {
                    currentState = m_States[m_States.Count - 1];
                }
                //执行状态进入事件
                currentState.OnEnter();
                return true;
            }
            return false;
        }

        public void Exit()
        {
            if (currentState != null)
            {
                currentState.OnExit();
                currentState = null;
            }
        }

        public virtual void OnInitialization() { }

        public virtual void OnUpdate()
        {
            //若当前状态不为空 执行状态停留事件
            currentState?.OnStay();
            //检测所有状态切换条件
            for (int i = 0; i < m_Conditions.Count; i++)
            {
                var condition = m_Conditions[i];
                //条件满足
                if (condition.predicate.Invoke())
                {
                    //源状态名称为空 表示从任意状态切换至目标状态
                    if (string.IsNullOrEmpty(condition.sourceStateName))
                        Switch(condition.targetStateName);
                    //源状态名称不为空 表示从指定状态切换至目标状态
                    else
                    {
                        //首先判断当前的状态是否为指定的状态
                        if (currentState.name == condition.sourceStateName)
                            Switch(condition.targetStateName);
                    }
                }
            }
        }

        public virtual void OnTermination()
        {
            for (int i = 0; i < m_States.Count; i++)
                m_States[i].OnTermination();
        }

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns>状态机</returns>
        public IStateMachine SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            m_Conditions.Add(new StateSwitchCondition(predicate, null, targetStateName));
            return this;
        }

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="sourceStateName">源状态名称</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns></returns>
        public IStateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName)
        {
            m_Conditions.Add(new StateSwitchCondition(predicate, sourceStateName, targetStateName));
            return this;
        }


        /// <summary>
        /// 构建状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态名称</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> Build<T>(string stateName = null) where T : State, new()
        {
            Type type = typeof(T);
            string name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            if (m_States.Find(m => m.name == name) == null)
            {
                T state = (T)Activator.CreateInstance(type);
                state.name = name;
                state.machine = this;
                m_States.Add(state);
                return new StateBuilder<T>(state);
            }
            return null;
        }
    }
}