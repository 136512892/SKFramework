using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 过滤条件预设文件
    /// </summary>
    public class GameObjectFilterConditionPreset : ScriptableObject
    {
        [HideInInspector] public GameObjectFilterCondition[] conditions;
    }
}