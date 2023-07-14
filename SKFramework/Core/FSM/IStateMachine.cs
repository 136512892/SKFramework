using System;

namespace SK.Framework.FSM
{
    public interface IStateMachine
    {
        /// <summary>
        /// 状态机名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        IState CurrentState { get; }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>添加结果</returns>
        int Add(IState state);

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <returns>true：移除成功  false：移除失败</returns>
        bool Remove(string stateName);

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>true：移除成功  false：移除失败</returns>
        bool Remove(IState state);

        /// <summary>
        /// 状态是否存在
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <returns>true：存在  false：不存在</returns>
        bool IsExists(string stateName);

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态名称</param>
        /// <returns>状态</returns>
        T Get<T>(string stateName) where T : IState, new();

        /// <summary>
        /// 尝试获取状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="stateName">状态名称</param>
        /// <param name="state">状态</param>
        /// <returns>true：获取成功  false：获取失败</returns>
        bool TryGet<T>(string stateName, out T state) where T : IState, new();

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <param name="data">数据</param>
        /// <returns>切换结果</returns>
        int Switch(string stateName, object data = null);

        /// <summary>
        /// 切换至下一状态
        /// </summary>
        /// <returns>true：切换成功  false：状态机中不存在任何状态，切换失败</returns>
        bool Switch2Next();

        /// <summary>
        /// 切换至上一状态
        /// </summary>
        /// <returns>true：切换成功  false：状态机中不存在任何状态，切换失败</returns>
        bool Switch2Last();

        /// <summary>
        /// 切换至空状态（退出当前状态）
        /// </summary>
        void Switch2Null();

        /// <summary>
        /// 初始化事件
        /// </summary>
        void OnInitialization();

        /// <summary>
        /// 刷新事件
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 终止事件
        /// </summary>
        void OnTermination();

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns>状态机</returns>
        IStateMachine SwitchWhen(Func<bool> predicate, string targetStateName);

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="sourceStateName">源状态名称</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns></returns>
        IStateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName);
    }
}