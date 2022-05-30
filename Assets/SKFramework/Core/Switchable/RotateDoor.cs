using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SK.Framework
{
    /// <summary>
    /// 旋转门
    /// </summary>
    public class RotateDoor : SwitchableDoor
    {
        [SerializeField] private float angle = 90f; //旋转角度
        private Coroutine switchCoroutine;

        private void Start()
        {
            switch (state)
            {
                case SwitchState.Open:
                    openValue = transform.forward + transform.position;
                    closeValue = Quaternion.AngleAxis(angle, transform.up) * transform.forward + transform.position;
                    break;
                case SwitchState.Close:
                    openValue = Quaternion.AngleAxis(angle, transform.up) * transform.forward + transform.position;
                    closeValue = transform.forward + transform.position;
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
            Quaternion beginRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(openValue - transform.position, transform.up);
            for (; (Time.time - beginTime) < duration;)
            {
                float t = (Time.time - beginTime) / duration;
                transform.rotation = Quaternion.Lerp(beginRot, targetRot, t);
                yield return null;
            }
            transform.rotation = targetRot;
            switchCoroutine = null;
        }
        private IEnumerator CloseCoroutine()
        {
            float beginTime = Time.time;
            Quaternion beginRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(closeValue - transform.position, transform.up);
            for (; (Time.time - beginTime) < duration;)
            {
                float t = (Time.time - beginTime) / duration;
                transform.rotation = Quaternion.Lerp(beginRot, targetRot, t);
                yield return null;
            }
            transform.rotation = targetRot;
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
                        openValue = transform.forward + transform.position;
                        closeValue = Quaternion.AngleAxis(angle, transform.up) * transform.forward + transform.position;
                        break;
                    case SwitchState.Close:
                        openValue = Quaternion.AngleAxis(angle, transform.up) * transform.forward + transform.position;
                        closeValue = transform.forward + transform.position;
                        break;
                }
            }
            Handles.color = Color.cyan;
            Handles.DrawWireCube(openValue, Vector3.one * .1f);
            Handles.DrawWireCube(closeValue, Vector3.one * .1f);
            Handles.DrawLine(transform.position, openValue);
            Handles.DrawLine(transform.position, closeValue);
            Handles.Label(openValue, "Open");
            Handles.Label(closeValue, "Close");
        }
#endif
    }
}