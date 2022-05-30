using System;

namespace SK.Framework
{
    /// <summary>
    /// 蓝湖界面UI元素
    /// </summary>
    [Serializable]
    public class LanHuViewElement 
    {
        /// <summary>
        /// 图层名称
        /// </summary>
        public string name;
        /// <summary>
        /// 位置x
        /// </summary>
        public string x;
        /// <summary>
        /// 位置y
        /// </summary>
        public string y;
        /// <summary>
        /// 宽度
        /// </summary>
        public string width;
        /// <summary>
        /// 高度
        /// </summary>
        public string height;
        /// <summary>
        /// 不透明度
        /// </summary>
        public string opacity;
        /// <summary>
        /// 像素倍数
        /// </summary>
        public string pixel = "x1";

        /// <summary>
        /// 构造函数
        /// </summary>
        public LanHuViewElement(string name, string x, string y, string width, string height, string opacity, string pixel)
        {
            this.name = name;  
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.opacity = opacity;
            this.pixel = pixel;
        }
    }
}