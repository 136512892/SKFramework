namespace SK.Framework.Refer
{
    public interface IReferComponent
    {
        /// <summary>
        /// 创建引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用命名</param>
        /// <returns>引用</returns>
        T Create<T>(string referName) where T : IRefer, new();

        /// <summary>
        /// 销毁引用
        /// </summary>
        /// <param name="referName">引用名称</param>
        /// <returns>true：销毁成功 false：销毁失败</returns>
        bool Destroy(string referName);

        /// <summary>
        /// 是否存在指定引用
        /// </summary>
        /// <param name="referName">引用名称</param>
        /// <returns>true：存在 false：不存在</returns>
        bool IsExists(string referName);

        /// <summary>
        /// 获取引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称</param>
        /// <returns>引用</returns>
        T Get<T>(string referName) where T : IRefer;

        /// <summary>
        /// 尝试获取引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称</param>
        /// <param name="refer">引用</param>
        /// <returns>true：获取成功 false：获取失败</returns>
        bool TryGet<T>(string referName, out T refer) where T : IRefer;

        /// <summary>
        /// 获取或创建引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称或引用命名</param>
        /// <returns>引用</returns>
        T GetOrCreate<T>(string referName) where T : IRefer, new();
    }
}