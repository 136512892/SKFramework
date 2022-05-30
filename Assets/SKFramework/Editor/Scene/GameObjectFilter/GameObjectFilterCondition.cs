using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    [Serializable]
    public class GameObjectFilterCondition
    {
        public GameObjectFilterMode filterMode;
        public MissingMode missingMode;
        public string stringValue;
        public int intValue;
        public bool boolValue;
        public Type typeValue;

        public GameObjectFilterCondition(GameObjectFilterMode filterMode, string stringValue)
        {
            this.filterMode = filterMode;
            this.stringValue = stringValue;
        }
        public GameObjectFilterCondition(GameObjectFilterMode filterMode, int intValue)
        {
            this.filterMode = filterMode;
            this.intValue = intValue;
        }
        public GameObjectFilterCondition(GameObjectFilterMode filterMode, bool boolValue)
        {
            this.filterMode = filterMode;
            this.boolValue = boolValue;
        }
        public GameObjectFilterCondition(GameObjectFilterMode filterMode, Type typeValue)
        {
            this.filterMode = filterMode;
            this.typeValue = typeValue;
        }
        public GameObjectFilterCondition(GameObjectFilterMode filterMode, MissingMode missingMode)
        {
            this.filterMode = filterMode;
            this.missingMode = missingMode;
        }

        /// <summary>
        /// 判断物体是否符合条件
        /// </summary>
        /// <param name="target">物体</param>
        /// <returns>符合条件返回true,否则返回false</returns>
        public bool IsMatch(GameObject target)
        {
            switch (filterMode)
            {
                case GameObjectFilterMode.Name: return target.name.ToLower().Contains(stringValue.ToLower());
                case GameObjectFilterMode.Component: return target.GetComponent(typeValue) != null;
                case GameObjectFilterMode.Layer: return target.layer == intValue;
                case GameObjectFilterMode.Tag: return target.CompareTag(stringValue);
                case GameObjectFilterMode.Active: return target.activeSelf == boolValue;
                case GameObjectFilterMode.Missing:
                    switch (missingMode)
                    {
                        case MissingMode.Material:
                            var mr = target.GetComponent<MeshRenderer>();
                            if (mr == null) return false;
                            Material[] materials = mr.sharedMaterials;
                            bool flag = false;
                            for (int i = 0; i < materials.Length; i++)
                            {
                                if (materials[i] == null)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            return flag;
                        case MissingMode.Mesh:
                            var mf = target.GetComponent<MeshFilter>();
                            if (mf == null) return false;
                            return mf.sharedMesh == null;
                        case MissingMode.Script:
                            Component[] components = target.GetComponents<Component>();
                            bool retV = false;
                            for (int i = 0; i < components.Length; i++)
                            {
                                if (components[i] == null)
                                {
                                    retV = true;
                                    break;
                                }
                            }
                            return retV;
                        default:
                            return false;
                    }
                default: return false;
            }
        }
    }
}