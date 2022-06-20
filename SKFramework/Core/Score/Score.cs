namespace SK.Framework
{
    public class Score
    {
        #region Public Methods
        /// <summary>
        /// 创建分数项
        /// </summary>
        /// <param name="id">分数信息ID</param>
        /// <returns>返回分数项标识符</returns>
        public static string Create(int id)
        {
            return ScoreMaster.Instance.Create(id);
        }
        /// <summary>
        /// 创建分数组合
        /// </summary>
        /// <param name="groupDescription">分数组合描述</param>
        /// <param name="valueMode">分数组计分方式</param>
        /// <param name="idArray">分数信息ID组合</param>
        /// <returns>返回分数项标识符组合</returns>
        public static string[] CreateGroup(string groupDescription, ValueMode valueMode, params int[] idArray)
        {
            return ScoreMaster.Instance.CreateGroup(groupDescription, valueMode, idArray);
        }
        /// <summary>
        /// 删除分数项
        /// </summary>
        /// <param name="flag">分数项标识符</param>
        /// <returns></returns>
        public static bool Delete(string flag)
        {
            return ScoreMaster.Instance.Delete(flag);
        }
        /// <summary>
        /// 删除分数组合
        /// </summary>
        /// <param name="groupDescription">分数组合描述</param>
        /// <returns>成功删除返回true 否则返回false</returns>
        public static bool DeleteGroup(string groupDescription)
        {
            return ScoreMaster.Instance.DeleteGroup(groupDescription);
        }
        /// <summary>
        /// 删除分数组合中的分数项
        /// </summary>
        /// <param name="groupDescription">分数组合</param>
        /// <param name="flag">分数项标识符</param>
        /// <returns>成功删除返回true 否则返回false</returns>
        public static bool DeleteGroupItem(string groupDescription, string flag)
        {
            return ScoreMaster.Instance.DeleteGroupItem(groupDescription, flag);
        }

        /// <summary>
        /// 获取分数项分值
        /// </summary>
        /// <param name="flag">分数项标识符</param>
        /// <returns>获取成功返回true 否则返回false</returns>
        public static bool Obtain(string flag)
        {
            return ScoreMaster.Instance.Obtain(flag);
        }
        /// <summary>
        /// 获取分数组合中指定标识分数项的分值
        /// </summary>
        /// <param name="groupDescription">分数组合</param>
        /// <param name="flag">分数项标识</param>
        /// <returns>获取成功返回true 否则返回false</returns>
        public static bool Obtain(string groupDescription, string flag)
        {
            return ScoreMaster.Instance.Obtain(groupDescription, flag);
        }

        /// <summary>
        /// 取消分数项分值
        /// </summary>
        /// <param name="flag">分数项标识符</param>
        /// <returns>取消成功返回true 否则返回false</returns>
        public static bool Cancle(string flag)
        {
            return ScoreMaster.Instance.Cancle(flag);
        }
        /// <summary>
        /// 取消分数组合中指定标识分数项的分值
        /// </summary>
        /// <param name="groupDescription">分数组合</param>
        /// <param name="flag">分数项标识</param>
        /// <returns>取消成功返回true 否则返回false</returns>
        public static bool Cancle(string groupDescription, string flag)
        {
            return ScoreMaster.Instance.Cancle(groupDescription, flag);
        }
        /// <summary>
        /// 获取分数组合已经获得的总分值
        /// </summary>
        /// <param name="groupDescription">分数组合</param>
        /// <returns>分数组合已经获得的总分值</returns>
        public static float GetGroupSum(string groupDescription)
        {
            return ScoreMaster.Instance.GetGroupSum(groupDescription);
        }
        /// <summary>
        /// 获取总分值
        /// </summary>
        /// <returns>总分值</returns>
        public static float GetSum()
        {
            return ScoreMaster.Instance.GetSum();
        }
        #endregion
    }
}