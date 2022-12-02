using UnityEngine;

namespace SK.Framework.Debugger
{
    internal interface IComponentInspector
    {
        void Draw(Component component);
    }

    public abstract class ComponentInspector : IComponentInspector
    {
        protected string valueStr;
        protected string newValueStr;
        protected float floatValue;
        protected bool boolValue;

        public void Draw(Component component)
        {
            OnDraw(component);
        }

        protected abstract void OnDraw(Component component);

        protected void DrawText(string label, string text, float labelWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label(text);
            GUILayout.EndHorizontal();
        }
        protected void DrawText(float spaceWidth, string label, string text, float labelWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label(text);
            GUILayout.EndHorizontal();
        }
        protected void DrawText(float spaceWidth, string label, string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label);
            GUILayout.FlexibleSpace();
            GUILayout.Label(text);
            GUILayout.EndHorizontal();
        }

        protected bool DrawToggle(string label, bool value, float labelWith)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWith));
            bool retValue = GUILayout.Toggle(value, GUIContent.none);
            GUILayout.EndHorizontal();
            return retValue;
        }
        protected bool DrawToggle(float spaceWidth, string label, bool value, float labelWith)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label, GUILayout.Width(labelWith));
            bool retValue = GUILayout.Toggle(value, GUIContent.none);
            GUILayout.EndHorizontal();
            return retValue;
        }

        protected int DrawInt(string label, int value, float labelWith, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWith));
            valueStr = value.ToString();
            newValueStr = GUILayout.TextField(valueStr, options);
            GUILayout.EndHorizontal();
            if (newValueStr != valueStr)
            {
                int.TryParse(newValueStr, out value);
            }
            return value;
        }
        protected int DrawInt(float spaceWidth, string label, int value, float labelWith, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label, GUILayout.Width(labelWith));
            valueStr = value.ToString();
            newValueStr = GUILayout.TextField(valueStr, options);
            GUILayout.EndHorizontal();
            if (newValueStr != valueStr)
            {
                int.TryParse(newValueStr, out value);
            }
            return value;
        }

        protected float DrawFloat(string label, float value, float labelWith, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWith));
            valueStr = value.ToString();
            newValueStr = GUILayout.TextField(valueStr, options);
            GUILayout.EndHorizontal();
            if (newValueStr != valueStr)
            {
                float.TryParse(newValueStr, out value);
            }
            return value;
        }
        protected float DrawFloat(float spaceWidth, string label, float value, float labelWith, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label, GUILayout.Width(labelWith));
            valueStr = value.ToString();
            newValueStr = GUILayout.TextField(valueStr, options);
            GUILayout.EndHorizontal();
            if (newValueStr != valueStr)
            {
                float.TryParse(newValueStr, out value);
            }
            return value;
        }

        protected Color DrawColor(string label, Color color, float labelWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label("R", GUILayout.Width(15f));
            valueStr = (color.r * 255f).ToString();
            newValueStr = GUILayout.TextField(valueStr);
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    color.r = floatValue / 255f;
                }
            }
            GUILayout.Label("G", GUILayout.Width(15f));
            valueStr = (color.g * 255f).ToString();
            newValueStr = GUILayout.TextField(valueStr);
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    color.g = floatValue / 255f;
                }
            }
            GUILayout.Label("B", GUILayout.Width(15f));
            valueStr = (color.b * 255f).ToString();
            newValueStr = GUILayout.TextField(valueStr);
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    color.b = floatValue / 255f;
                }
            }
            GUILayout.Label("A", GUILayout.Width(15f));
            valueStr = (color.a * 255f).ToString();
            newValueStr = GUILayout.TextField(valueStr);
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    color.a = floatValue / 255f;
                }
            }
            GUILayout.EndHorizontal();
            return color;
        }

        protected Vector2 DrawVector2(string label, Vector2 vector2, float labelWidth, float perFieldWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label("X", GUILayout.Width(15f));
            valueStr = vector2.x.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector2.x = floatValue;
                }
            }
            GUILayout.Label("Y", GUILayout.Width(15f));
            valueStr = vector2.y.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector2.y = floatValue;
                }
            }
            GUILayout.EndHorizontal();
            return vector2;
        }
        protected Vector2 DrawVector2(string label, Vector2 vector2, float labelWidth, string x, string y, float xyWidth, float perFieldWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label(x, GUILayout.Width(xyWidth));
            valueStr = vector2.x.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector2.x = floatValue;
                }
            }
            GUILayout.Label(y, GUILayout.Width(xyWidth));
            valueStr = vector2.y.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector2.y = floatValue;
                }
            }
            GUILayout.EndHorizontal();
            return vector2;
        }
        protected Vector2 DrawVector2(float spaceWidth, string label, Vector2 vector2, float labelWidth, float perFieldWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label("X", GUILayout.Width(15f));
            valueStr = vector2.x.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector2.x = floatValue;
                }
            }
            GUILayout.Label("Y", GUILayout.Width(15f));
            valueStr = vector2.y.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector2.y = floatValue;
                }
            }
            GUILayout.EndHorizontal();
            return vector2;
        }
        protected Vector3 DrawVector3(string label, Vector3 vector3, float labelWidth, float perFieldWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label("X", GUILayout.Width(15f));
            valueStr = vector3.x.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector3.x = floatValue;
                }
            }
            GUILayout.Label("Y", GUILayout.Width(15f));
            valueStr = vector3.y.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector3.y = floatValue;
                }
            }
            GUILayout.Label("Z", GUILayout.Width(15f));
            valueStr = vector3.z.ToString();
            newValueStr = GUILayout.TextField(valueStr, GUILayout.Width(perFieldWidth));
            if (newValueStr != valueStr)
            {
                if (float.TryParse(newValueStr, out floatValue))
                {
                    vector3.z = floatValue;
                }
            }
            GUILayout.EndHorizontal();
            return vector3;
        }

        protected float DrawHorizontalSlider(string label, float value, float leftValue, float rightValue, float labelWidth, float valueLabelWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label(value.ToString("F2"), GUILayout.Width(valueLabelWidth));
            value = GUILayout.HorizontalSlider(value, leftValue, rightValue);
            GUILayout.EndHorizontal();
            return value;
        }
        protected float DrawHorizontalSlider(float spaceWidth, string label, float value, float leftValue, float rightValue, float labelWidth, float valueLabelWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            GUILayout.Label(value.ToString("F2"), GUILayout.Width(valueLabelWidth));
            value = GUILayout.HorizontalSlider(value, leftValue, rightValue);
            GUILayout.EndHorizontal();
            return value;
        }
    }
}