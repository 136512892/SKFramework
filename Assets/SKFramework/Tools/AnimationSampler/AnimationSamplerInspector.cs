#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    [CustomEditor(typeof(AnimationSampler))]
    public class AnimationSamplerInspector : Editor
    {
        private AnimationSampler sampler;

        private void OnEnable()
        {
            sampler = target as AnimationSampler;
        }

        public override void OnInspectorGUI()
        {
            //选中的物体不包含Animator组件 return
            var animator = sampler.GetComponent<Animator>();
            if (animator == null)
            {
                EditorGUILayout.HelpBox("Not found Animator component.", MessageType.Warning);
                return;
            }
            //动画未初始化 return
            if (!animator.isInitialized)
            {
                EditorGUILayout.HelpBox("Animator is not initialized.", MessageType.Warning);
                if (GUILayout.Button("Rebind"))
                {
                    animator.Rebind();
                }
                return;
            }
            //获取所有动画片段
            var clips = animator.runtimeAnimatorController.animationClips;
            if (clips.Length == 0)
            {
                EditorGUILayout.HelpBox("Animation clips count: 0", MessageType.Info);
                return;
            }
            //获取所有动画片段名称
            var names = new string[clips.Length];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = clips[i].name;
            }
            //通过名称选择动画片段
            sampler.currentClipIndex = EditorGUILayout.Popup(sampler.currentClipIndex, names);
            //水平布局
            GUILayout.BeginHorizontal();
            {
                //采样的进度
                sampler.normalizedTime = EditorGUILayout.Slider(sampler.normalizedTime, 0f, 1f);
                //当前动画片段总时长
                float length = clips[sampler.currentClipIndex].length;
                //当前采样的时间点
                float currentTime = length * sampler.normalizedTime;
                //文本显示时长信息 00:00/00:00
                GUILayout.Label($"{ToMSTimeFormat(currentTime)}/{ToMSTimeFormat(length)}");
                //动画采样
                clips[sampler.currentClipIndex].SampleAnimation(animator.gameObject, currentTime);
            }
            GUILayout.EndHorizontal();
        }
        //将秒数转换为00:00格式字符串
        private string ToMSTimeFormat(float length)
        {
            int v = (int)length;
            int minute = v / 60;
            int second = v % 60;
            return string.Format("{0:D2}:{1:D2}", minute, second);
        }
    }
}
#endif