using System;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 状态机
    /// </summary>
    public class StateMachine
    {
        //状态列表 存储状态机内所有状态
        protected readonly List<IState> states = new List<IState>();
        //状态切换条件列表
        protected List<StateSwitchCondition> conditions = new List<StateSwitchCondition>();
        
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
        /// <returns>添加成功返回true 否则返回false</returns>
        public bool Add(IState state)
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
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态命名</param>
        /// <returns>添加成功返回true 否则返回false</returns>
        public bool Add<T>(string stateName = null) where T : IState, new()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            t.Name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            return Add(t);
        }
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>移除成功返回true 否则返回false</returns>
        public bool Remove(IState state)
        {
            //判断是否存在
            if (states.Contains(state))
            {
                //如果要移除的状态为当前状态 首先执行当前状态退出事件
                if (CurrentState == state)
                {
                    CurrentState.OnExit();
                    CurrentState = null;
                }
                //执行状态终止事件
                state.OnTermination();
                return states.Remove(state);
            }
            return false;
        }
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <returns>移除成功返回true 否则返回false</returns>
        public bool Remove(string stateName)
        {
            var targetIndex = states.FindIndex(m => m.Name == stateName);
            if (targetIndex != -1)
            {
                var targetState = states[targetIndex];
                if (CurrentState == targetState)
                {
                    CurrentState.OnExit();
                    CurrentState = null;
                }
                targetState.OnTermination();
                return states.Remove(targetState);
            }
            return false;
        }
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>移除成返回true 否则返回false</returns>
        public bool Remove<T>() where T : IState
        {
            return Remove(typeof(T).Name);
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>切换成功返回true 否则返回false</returns>
        public bool Switch(IState state)
        {
            //如果当前状态已经是切换的目标状态 无需切换 返回false
            if (CurrentState == state) return false;
            //当前状态不为空则执行状态退出事件
            CurrentState?.OnExit();
            //判断切换的目标状态是否存在于列表中
            if (!states.Contains(state)) return false;
            //更新当前状态
            CurrentState = state;
            //更新后 当前状态不为空则执行状态进入事件
            CurrentState?.OnEnter();
            return true;
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <returns>切换成功返回true 否则返回false</returns>
        public bool Switch(string stateName)
        {
            //根据状态名称在列表中查询
            var targetState = states.Find(m => m.Name == stateName);
            return Switch(targetState);
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>切换成返回true 否则返回false</returns>
        public bool Switch<T>() where T : IState
        {
            return Switch(typeof(T).Name);
        }
        /// <summary>
        /// 切换至下一状态
        /// </summary>
        public void Switch2Next()
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
                    //首先执行当前状态的退出事件 再更新到下一状态
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
            }
        }
        /// <summary>
        /// 切换至上一状态
        /// </summary>
        public void Switch2Last()
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
                    //首先执行当前状态的退出事件 再更新到上一状态
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
            }
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
        /// 获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态名称</param>
        /// <returns>状态</returns>
        public T GetState<T>(string stateName) where T : IState
        {
            return (T)states.Find(m => m.Name == stateName);
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns>状态</returns>
        public T GetState<T>() where T : IState
        {
            return (T)states.Find(m => m.Name == typeof(T).Name);
        }
        /// <summary>
        /// 销毁状态机
        /// </summary>
        public void Destroy()
        {
            FSM.Instance.Destroy(this);
        }
        /// <summary>
        /// 状态机刷新事件
        /// </summary>
        public void OnUpdate()
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
        /// 状态机销毁事件
        /// </summary>
        public void OnDestroy()
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
        public StateMachine SwitchWhen(Func<bool> predicate, string targetStateName)
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
        public StateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName)
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
            T t = (T)Activator.CreateInstance(type);
            t.Name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            if (states.Find(m => m.Name == t.Name) == null)
            {
                states.Add(t);
            }
            return new StateBuilder<T>(t, this);
        }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public static StateMachine Create(string stateMachineName)
        {
            return FSM.Instance.Create<StateMachine>(stateMachineName);
        }
        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public static T Create<T>(string stateMachineName = null) where T : StateMachine, new()
        {
            return FSM.Instance.Create<T>(stateMachineName);
        }
        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>销毁成功返回true 否则返回false</returns>
        public static bool Destroy(string stateMachineName)
        {
            return FSM.Instance.Destroy(stateMachineName);
        }
        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <returns>销毁成功返回true 否则返回false</returns>
        public static bool Destroy<T>() where T : StateMachine
        {
            return FSM.Instance.Destroy(typeof(T).Name);
        }
        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public static StateMachine Get(string stateMachineName)
        {
            return FSM.Instance.GetMachine<StateMachine>(stateMachineName);
        }
        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public static T Get<T>(string stateMachineName) where T : StateMachine
        {
            return FSM.Instance.GetMachine<T>(stateMachineName);
        }
        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <returns>状态机</returns>
        public static T Get<T>() where T : StateMachine
        {
            return FSM.Instance.GetMachine<T>(typeof(T).Name);
        }
    }
}