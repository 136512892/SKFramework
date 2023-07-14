using UnityEngine;
using UnityEngine.Events;
using SK.Framework.Actions;

namespace SK.Framework.UI
{
    /// <summary>
    /// UI视图基类
    /// </summary>
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UIView : MonoBehaviour, IUIView
    {
        private CanvasGroup canvasGroup;

        private IActionChain animationChain;
        [HideInInspector, SerializeField] private UIViewAnimation openAnim;
        [HideInInspector, SerializeField] private UIViewAnimation closeAnim;
        [HideInInspector, SerializeField] private UnityEvent onOpenEvent;
        [HideInInspector, SerializeField] private UnityEvent onCloseEvent;

        public string Name { get; set; }

        public virtual bool IsOpened
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        public virtual void OnInit(object data) 
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void OnOpen(bool instant, object data) 
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            canvasGroup.interactable = false;
            animationChain?.Stop();
            onOpenEvent?.Invoke();
            animationChain = openAnim.Play(this, instant, () =>
            {
                canvasGroup.interactable = true;
                animationChain = null;
            });
        }

        public virtual void OnClose(bool instant) 
        {
            canvasGroup.interactable = false;
            animationChain?.Stop();
            animationChain = closeAnim.Play(this, instant, () =>
            {
                animationChain = null;
                onCloseEvent?.Invoke();
                gameObject.SetActive(false);
            });
        }

        public virtual void OnUnload() { }
    }
}