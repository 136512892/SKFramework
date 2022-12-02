using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

namespace SK.Framework.Debugger
{
    public class HierarchyWindow : IDebuggerWIndow
    {
        private List<HierarchyWindowItem> list;
        private Vector2 scroll;

        public void OnInitilization()
        {
            list = new List<HierarchyWindowItem>();
            CollectRoots();
        }

        public void OnTermination()
        {
            list.Clear();
            list = null;
        }

        public void OnWindowGUI()
        {
            if (GUILayout.Button("Repaint"))
            {
                CollectRoots();
                return;
            }

            scroll = GUILayout.BeginScrollView(scroll);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Draw();
            }
            GUILayout.EndScrollView();
        }

        private void CollectRoots()
        {
            list.Clear();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;

                var roots = scene.GetRootGameObjects();
                for (int j = 0; j < roots.Length; j++)
                {
                    CollectChildrens(roots[j].transform, list);
                }
            }
            var gos = Main.Debugger.gameObject.scene.GetRootGameObjects();
            for (int i = 0; i < gos.Length; i++)
            {
                CollectChildrens(gos[i].transform, list);
            }
        }
        private void CollectChildrens(Transform transform, List<HierarchyWindowItem> list) 
        {
            var item = new HierarchyWindowItem(transform);
            list.Add(item);
            for (int i = 0; i < transform.childCount; i++)
            {
                CollectChildrens(transform.GetChild(i), item.childrens);
            }
        }
    }
}