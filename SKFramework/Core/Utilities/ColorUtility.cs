using UnityEngine;

namespace SK.Framework.Utility
{
    public class ColorUtility 
    {
        public static bool IsApproximatelyBlack(Color color)
        {
            return color.r + color.g + color.b <= Mathf.Epsilon;
        }

        public static bool IsApproximatelyWhite(Color color)
        {
            return color.r + color.g + color.b >= 1 - Mathf.Epsilon;
        }

        public static Color Invert(Color color)
        {
            color.r = 1 - color.r;
            color.g = 1 - color.g;
            color.b = 1 - color.b;
            color.a = 1 - color.a;
            return color;
        }

        public static Color WithAlpha(Color color, float alpha)
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