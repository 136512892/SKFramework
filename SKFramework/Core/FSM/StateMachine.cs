using System;
using System.Collections.Generic;

namespace SK.Framework.FSM
{
    /// <summary>
    /// 状态机
    /// </summary>
    public class StateMachine : IStateMachine
    {
        //状态列表 存储状态机内所有状态
        private readonly List<IState> states = new List<IState>();
        //状态切换条件列表
        private readonly List<StateSwitchCondition> conditions = new List<StateSwitchCondition>();

        /// <summary>
        /// 状态机名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public IState CurrentState { get; protected set; }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>0：添加成功； -1：状态已存在,无需重复添加； -2：存在同名状态，添加失败</returns>
        public int Add(IState state)
        {
            //判断是否已经存在
            if (!states.Contains(state))
            {
                //判断是否存在同名状态
                if (states.Find(m => m.Name == state.Name) == null)
                {
                    //存储到列表
                    states.Add(state);
                    //执行状态初始化事件
                    state.OnInitialization();
                    //设置状态所属的状态机
                    state.Machine = this;
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
            t.Name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
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
            var target = states.Find(m => m.Name == stateName);
            if (target != null)
            {
                //如果要移除的状态为当前状态 首先执行当前状态退出事件
                if (CurrentState == target)
                {
                    CurrentState.OnExit();
                    CurrentState = null;
                }
                //执行状态终止事件
                target.OnTermination();
                //从列表中移除
                states.Remove(target);
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
            return Remove(state.Name);
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
            return states.FindIndex(m => m.Name == stateName) != -1;
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
            var target = states.Find(m => m.Name == stateName);
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
            int index = states.FindIndex(m => m.Name == stateName);
            state = index != -1 ? (T)states[index] : default;
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
            var target = states.Find(m => m.Name == stateName);
            if (target == null) return -1;
            //如果当前状态已经是切换的目标状态 并且该状态不可切换至自身 无需切换 返回false
            if (CurrentState == target && !target.CanSwitch2Self) return -2;
            //当前状态不为空则执行状态退出事件
            CurrentState?.OnExit();
            //更新当前状态
            CurrentState = target;
            //更新后 执行状态进入事件
            CurrentState.OnEnter(data);
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
            if (states.Count != 0)
            {
                //如果当前状态不为空 则根据当前状态找到下一个状态
                if (CurrentState != null)
                {
                    int index = states.IndexOf(CurrentState);
                    //当前状态的索引值+1后若小于列表中的数量 则下一状态的索引为index+1
                    //否则表示当前状态已经是列表中的最后一个 下一状态则回到列表中的第一个状态 索引为0
                    index = index + 1 < states.Count ? index + 1 : 0;
                    IState targetState = states[index];
                    //首先执行当前状态的退出事件 再更新到目标状态
                    CurrentState.OnExit();
                    CurrentState = targetState;
                }
                //当前状态为空 则直接进入列表中的第一个状态
                else
                {
                    CurrentState = states[0];
                }
                //执行状态进入事件
                CurrentState.OnEnter();
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
            if (states.Count != 0)
            {
                //如果当前状态不为空 则根据当前状态找到上一个状态
                if (CurrentState != null)
                {
                    int index = states.IndexOf(CurrentState);
                    //当前状态的索引值-1后若大等于0 则下一状态的索引为index-1
                    //否则表示当前状态是列表中的第一个 上一状态则回到列表中的最后一个状态
                    index = index - 1 >= 0 ? index - 1 : states.Count - 1;
                    IState targetState = states[index];
                    //首先执行当前状态的退出事件 再更新到目标状态
                    CurrentState.OnExit();
                    CurrentState = targetState;
                }
                //当前状态为空 则直接进入列表中的最后一个状态
                else
                {
                    CurrentState = states[states.Count - 1];
                }
                //执行状态进入事件
                CurrentState.OnEnter();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 切换至空状态（退出当前状态）
        /// </summary>
        public void Switch2Null()
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
                CurrentState = null;
            }
        }

        /// <summary>
        /// 状态机初始化事件
        /// </summary>
        public virtual void OnInitialization() { }

        /// <summary>
        /// 状态机刷新事件
        /// </summary>
        public virtual void OnUpdate()
        {
            //若当前状态不为空 执行状态停留事件
            CurrentState?.OnStay();
            //检测所有状态切换条件
            for (int i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                //条件满足
                if (condition.predicate.Invoke())
                {
                    //源状态名称为空 表示从任意状态切换至目标状态
                    if (string.IsNullOrEmpty(condition.sourceStateName))
                    {
                        Switch(condition.targetStateName);
                    }
                    //源状态名称不为空 表示从指定状态切换至目标状态
                    else
                    {
                        //首先判断当前的状态是否为指定的状态
                        if (CurrentState.Name == condition.sourceStateName)
                        {
                            Switch(condition.targetStateName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 状态机终止事件
        /// </summary>
        public virtual void OnTermination()
        {
            //执行状态机内所有状态的状态终止事件
            for (int i = 0; i < states.Count; i++)
            {
                states[i].OnTermination();
            }
        }

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns>状态机</returns>
        public IStateMachine SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            conditions.Add(new StateSwitchCondition(predicate, null, targetStateName));
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
            conditions.Add(new StateSwitchCondition(predicate, sourceStateName, targetStateName));
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
            if (states.Find(m => m.Name == name) == null)
            {
                T state = (T)Activator.CreateInstance(type);
                state.Name = name;
                state.Machine = this;
                states.Add(state);
                return new StateBuilder<T>(state, this);
            }
            return null;
        }
    }
}