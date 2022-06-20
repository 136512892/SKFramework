using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SK.Framework
{
    /// <summary>
    /// 移动门
    /// </summary>
    public class MoveDoor : SwitchableDoor
    {
        [SerializeField] private Vector3 direction = Vector3.left; //移动方向
        [SerializeField] private float magnitude = 1f; //移动的长度
        private Coroutine switchCoroutine;

        private void Start()
        {
            switch (state)
            {
                case SwitchState.Open:
                    openValue = transform.position;
                    closeValue = transform.position + direction.normalized * magnitude;
                    break;
                case SwitchState.Close:
                    openValue = transform.position + direction.normalized * magnitude;
                    closeValue = transform.position;
                    break;
            }
        }
        public override void Open()
        {
            if (state == SwitchState.Open) return;
            state = SwitchState.Open;
            if (switchCoroutine != null) StopCoroutine(switchCoroutine);
            switchCoroutine = StartCoroutine(OpenCoroutine());
        }
        public override void Close()
        {
            if (state == SwitchState.Close) return;
            state = SwitchState.Close;
            if (switchCoroutine != null) StopCoroutine(switchCoroutine);
            switchCoroutine = StartCoroutine(CloseCoroutine());
        }

        private IEnumerator OpenCoroutine()
        {
            float beginTime = Time.time;
            Vector3 beginPos = transform.position;
            for (;(Time.time - beginTime) < duration;)
            {
                float t = (Time.time - beginTime) / duration;
                transform.position = Vector3.Lerp(beginPos, openValue, t);
                yield return null;
            }
            transform.position = openValue;
            switchCoroutine = null;
        }
        private IEnumerator CloseCoroutine()
        {
            float beginTime = Time.time;
            Vector3 beginPos = transform.position;
            for (; (Time.time - beginTime) < duration;)
            {
                float t = (Time.time - beginTime) / duration;
                transform.position = Vector3.Lerp(beginPos, closeValue, t);
                yield return null;
            }
            transform.position = closeValue;
            switchCoroutine = null;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                switch (state)
                {
                    case SwitchState.Open:
                        openValue = transform.position;
                        closeValue = transform.position + direction.normalized * magnitude;
                        break;
                    case SwitchState.Close:
                        openValue = transform.position + direction.normalized * magnitude;
                        closeValue = transform.position;
                        break;
                }
            }
            Handles.color = Color.cyan;
            Handles.DrawWireCube(openValue, Vector3.one * .1f);
            Handles.DrawWireCube(closeValue, Vector3.one * .1f);
            Handles.DrawLine(openValue, closeValue);
            Handles.Label(openValue, "Open");
            Handles.Label(closeValue, "Close");
        }
#endif
    }
}