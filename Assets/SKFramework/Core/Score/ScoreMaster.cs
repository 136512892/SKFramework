using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public class ScoreMaster : MonoBehaviour
    {
        #region NonPublic Variables
        private static ScoreMaster instance;
        [SerializeField] private ScoreProfile profile;
        private readonly Dictionary<string, ScoreGroup> groups = new Dictionary<string, ScoreGroup>();
        private const string ungrouped = "未分组";
        #endregion

        #region Public Properties
        public static ScoreMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ScoreMaster>();
                }
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.Score]").AddComponent<ScoreMaster>();
                    instance.profile = Resources.Load<ScoreProfile>("Score Profile");
                    if (instance.profile == null)
                    {
                        Log.Error(Module.Score, "加载配置文件失败");
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Public Methods
        public string Create(int id)
        {
            var info = Array.Find(profile.scores, m => m.id == id);
            if (info != null)
            {
                var flag = Guid.NewGuid().ToString();
                var score = new ScoreItem(flag, info.description, info.value);
                Log.Info(Module.Score, string.Format("创建分数ID为[{0}]的分数项 [{1}]", id, info.description));
                if (!groups.ContainsKey(ungrouped))
                {
                    groups.Add(ungrouped, new ScoreGroup(ungrouped, ValueMode.Additive, score));
                }
                else
                {
                    groups[ungrouped].Scores.Add(score);
                }
                return flag;
            }
            else
            {
                Log.Error(Module.Score, string.Format("配置表中不存在ID为[{0}]的分数信息", id));
                return null;
            }
        }
        public string[] CreateGroup(string groupDescription, ValueMode valueMode, params int[] idArray)
        {
            ScoreItem[] scores = new ScoreItem[idArray.Length];
            string[] flags = new string[idArray.Length];
            for (int i = 0; i < idArray.Length; i++)
            {
                var info = Array.Find(profile.scores, m => m.id == idArray[i]);
                if (info != null)
                {
                    var flag = Guid.NewGuid().ToString();
                    flags[i] = flag;
                    scores[i] = new ScoreItem(flag, info.description, info.value);
                    Log.Info(Module.Score, string.Format("创建分数ID为[{0}]的分数项 [{1}]", idArray[i], info.description));
                }
                else
                {
                    Log.Error(Module.Score, string.Format("配置表中不存在ID为[{0}]的分数信息", idArray[i]));
                }
            }
            ScoreGroup group = new ScoreGroup(groupDescription, valueMode, scores);
            groups.Add(groupDescription, group);
            Log.Info(Module.Score, string.Format("创建分数组合[{0}] 计分模式[{1}]", groupDescription, valueMode));
            return flags;
        }

        public bool Delete(string flag)
        {
            if (groups.ContainsKey(ungrouped))
            {
                return groups[ungrouped].Delete(flag);
            }
            return false;
        }
        public bool DeleteGroup(string groupDescription)
        {
            if (groups.ContainsKey(groupDescription))
            {
                groups.Remove(groupDescription);
                Log.Info(Module.Score, string.Format("删除分数组合[{0}]", groupDescription));
                return true;
            }
            Log.Info(Module.Score, string.Format("不存在分数组合[{0}]", groupDescription));
            return false;
        }
        public bool DeleteGroupItem(string groupDescription, string flag)
        {
            if (groups.TryGetValue(groupDescription, out ScoreGroup target))
            {
                return target.Delete(flag);
            }
            Log.Info(Module.Score, string.Format("不存在分数组合[{0}]", groupDescription));
            return false;
        }

        public bool Obtain(string flag)
        {
            if (groups.ContainsKey(ungrouped))
            {
                return groups[ungrouped].Obtain(flag);
            }
            return false;
        }
        public bool Obtain(string groupDescription, string flag)
        {
            if (groups.TryGetValue(groupDescription, out ScoreGroup target))
            {
                return target.Obtain(flag);
            }
            Log.Info(Module.Score, string.Format("不存在分数组合[{0}]", groupDescription));
            return false;
        }

        public bool Cancle(string flag)
        {
            if (groups.ContainsKey(ungrouped))
            {
                return groups[ungrouped].Cancle(flag);
            }
            return false;
        }
        public bool Cancle(string groupDescription, string flag)
        {
            if (groups.TryGetValue(groupDescription, out ScoreGroup target))
            {
                return target.Cancle(flag);
            }
            Log.Info(Module.Score, string.Format("不存在分数组合[{0}]", groupDescription));
            return false;
        }

        public float GetGroupSum(string groupDescription)
        {
            if (groups.TryGetValue(groupDescription, out ScoreGroup target))
            {
                return target.GetSum();
            }
            Log.Info(Module.Score, string.Format("不存在分数组合[{0}]", groupDescription));
            return 0f;
        }
        public float GetSum()
        {
            float retV = 0f;
            foreach (var kv in groups)
            {
                retV += kv.Value.GetSum();
            }
            return retV;
        }
        #endregion
    }
}