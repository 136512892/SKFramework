/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public static class ColorUtility
    {
        public static Color Invert(Color color)
        {
            color.r = 1 - color.r;
            color.g = 1 - color.g;
            color.b = 1 - color.b;
            color.a = 1 - color.a;
            return color;
        }

        public static Color Alpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Color From255(float r, float g, float b, float a = 255)
        {
            Color color = new Color
            {
                r = r / 255f,
                g = g / 255f,
                b = b / 255f,
                a = a / 255f
            };
            return color;
        }

        public static Color FromHex(string hexValue, float alpha = 1)
        {
            if (string.IsNullOrEmpty(hexValue)) return Color.clear;
            if (hexValue[0] == '#') hexValue = hexValue.TrimStart('#');
            if (hexValue.Length > 6) hexValue = hexValue.Remove(6, hexValue.Length - 6);
            int value = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            int r = value >> 16 & 255;
            int g = value >> 8 & 255;
            int b = value & 255;
            float a = 255 * alpha;
            return From255(r, g, b, a);
        }
    }
}