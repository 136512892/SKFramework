using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Debugger
{
    public class HierarchyWindowItem
    {
        private bool expand;

        private int level;

        private readonly Transform transform;

        public readonly List<HierarchyWindowItem> childrens;

        public HierarchyWindowItem(Transform transform)
        {
            this.transform = transform;
            childrens = new List<HierarchyWindowItem>();
            GetParent(transform);
        }

        public void Draw()
        {
            if (transform == null) return;
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f * level);
            if (transform.childCount > 0)
            {
                if (GUILayout.Button(expand ? "▾" : "▸", Main.Debugger.MiniButtonStyle, GUILayout.Width(17.5f), GUILayout.Height(15f)))
                {
                    expand = !expand;
                }
            }
            else
            {
                GUILayout.Label(GUIContent.none, GUILayout.Width(17.5f));
            }
            if (GUILayout.Toggle(Main.Debugger.currentSelected == transform.gameObject, transform.name))
            {
                Main.Debugger.currentSelected = transform.gameObject;
            }
            GUILayout.EndHorizontal();
            if (expand)
            {
                for (int i = 0; i < childrens.Count; i++)
                {
                    childrens[i].Draw();
                }
            }
        }

        private void GetParent(Transform transform)
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                level++;
                GetParent(parent);
            }
        }
    }
}