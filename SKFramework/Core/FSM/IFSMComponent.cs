namespace SK.Framework.FSM
{
    public interface IFSMComponent
    {
        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机命名</param>
        /// <returns>状态机</returns>
        T Create<T>(string stateMachineName) where T : IStateMachine, new();

        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>true：销毁成功  false：销毁失败</returns>
        bool Destroy(string stateMachineName);

        /// <summary>
        /// 状态机是否存在
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>true：存在  false：不存在</returns>
        bool IsExists(string stateMachineName);

        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        T Get<T>(string stateMachineName) where T: IStateMachine, new();

        /// <summary>
        /// 尝试获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <param name="stateMachine">状态机</param>
        /// <returns>true：获取成功  false：获取失败</returns>
        bool TryGet<T>(string stateMachineName, out T stateMachine) where T : IStateMachine, new();
    }
}