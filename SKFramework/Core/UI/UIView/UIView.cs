using UnityEngine;
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
        private RectTransform rectTransform;

        //加载、显示事件
        [HideInInspector, SerializeField] private ViewAnimationEvent onVisible;
        //隐藏、卸载事件
        [HideInInspector, SerializeField] private ViewAnimationEvent onInvisible;

        protected IActionChain animationChain;

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null)
                {
                    canvasGroup = GetComponent<CanvasGroup>();
                }
                return canvasGroup;
            }
        }
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }
        /// <summary>
        /// 视图名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示视图
        /// </summary>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        public void Show(IViewData data = null, bool instant = false)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            OnShow(data);

            //执行动画开始事件
            onVisible.onBeganEvent?.Invoke();
            //播放音效
            if (onVisible.onBeganSound.Clip != null)
                Main.Audio.SFX.Play(onVisible.onBeganSound.Clip);
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onVisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
                //播放音效
                Main.Audio.SFX.Play(onVisible.onEndSound.Clip);
                //可交互性置为true
                CanvasGroup.interactable = true;
                animationChain = null;
            });
        }
        /// <summary>
        /// 隐藏视图
        /// </summary>
        /// <param name="instant">是否立即隐藏</param>
        public void Hide(bool instant = false)
        {
            OnHide();

            //执行动画开始事件
            onInvisible.onBeganEvent?.Invoke();
            //播放音效
            if (onInvisible.onBeganSound.Clip != null)
                Main.Audio.SFX.Play(onInvisible.onBeganSound.Clip);
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onInvisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
                //播放音效
                if (onInvisible.onEndSound.Clip != null)
                    Main.Audio.SFX.Play(onInvisible.onEndSound.Clip);
                animationChain = null;
                gameObject.SetActive(false);
            });
        }
        /// <summary>
        /// 视图初始化
        /// </summary>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        public void Init(IViewData data = null, bool instant = false)
        {
            OnInit(data);

            //执行动画开始事件
            onVisible.onBeganEvent?.Invoke();
            //播放音效
            if (onVisible.onBeganSound.Clip != null)
                Main.Audio.SFX.Play(onVisible.onBeganSound.Clip);
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onVisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
                //播放音效
                if (onInvisible.onEndSound.Clip != null)
                    Main.Audio.SFX.Play(onVisible.onEndSound.Clip);
                //可交互性置为true
                CanvasGroup.interactable = true;
                animationChain = null;
            });
        }
        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <param name="instant">是否立即卸载</param>
        public void Unload(bool instant = false)
        {
            Main.UI.Remove(Name);
            OnUnload();

            //执行动画开始事件
            onInvisible.onBeganEvent?.Invoke();
            //播放音效
            if (onInvisible.onBeganSound.Clip != null)
                Main.Audio.SFX.Play(onInvisible.onBeganSound.Clip);
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onInvisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
                //播放音效
                if (onInvisible.onEndSound.Clip != null)
                    Main.Audio.SFX.Play(onInvisible.onEndSound.Clip);
                //销毁视图物体
                Destroy(gameObject);
            });
        }

        protected virtual void OnInit(IViewData data) { }
        protected virtual void OnShow(IViewData data) { }
        protected virtual void OnHide() { }
        protected virtual void OnUnload() { }
    }
}