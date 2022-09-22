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
            //播放动画开始音效
            onVisible.onBeganSound.Play();
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onVisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
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
            //播放动画开始音效
            onInvisible.onBeganSound.Play();
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onInvisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
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
            //播放动画开始音效
            onVisible.onBeganSound.Play();
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onVisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
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
            UI.Instance.Remove(Name);
            OnUnload();

            //执行动画开始事件
            onInvisible.onBeganEvent?.Invoke();
            //播放动画开始音效
            onInvisible.onBeganSound.Play();
            //可交互性置为false
            CanvasGroup.interactable = false;
            //播放动画
            if (animationChain != null) animationChain.Stop();
            animationChain = onInvisible.animation.Play(this, instant, () =>
            {
                //执行动画结束事件
                onVisible.onEndEvent?.Invoke();
                //销毁视图物体
                Destroy(gameObject);
            });
        }

        protected virtual void OnInit(IViewData data) { }
        protected virtual void OnShow(IViewData data) { }
        protected virtual void OnHide() { }
        protected virtual void OnUnload() { }

        #region Static Methods
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="viewResourcePath">视图资源路径</param>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public static T Load<T>(string viewName, string viewResourcePath, ViewLevel level = ViewLevel.COMMON, IViewData data = null, bool instant = false) where T : UIView
        {
            if (UI.Instance.LoadView(viewName, viewResourcePath, level, out IUIView uiview, data, instant))
            {
                return uiview as T;
            }
            Debug.LogError($"加载UI视图 [{viewName}] 失败.");
            return null;
        }
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public static T Load<T>(string viewName, ViewLevel level = ViewLevel.COMMON, IViewData data = null, bool instant = false) where T : UIView
        {
            if (UI.Instance.LoadView(viewName, viewName, level, out IUIView uiview, data, instant))
            {
                return uiview as T;
            }
            Debug.LogError($"加载UI视图 [{viewName}] 失败.");
            return null;
        }
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public static T Load<T>(ViewLevel level, IViewData data = null, bool instant = false) where T : UIView
        {
            if (UI.Instance.LoadView(typeof(T).Name, typeof(T).Name, level, out IUIView uiview, data, instant))
            {
                return uiview as T;
            }
            Debug.LogError($"加载UI视图 [{typeof(T).Name}] 失败.");
            return null;
        }
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public static T Load<T>(IViewData data = null, bool instant = false) where T : UIView
        {
            if (UI.Instance.LoadView(typeof(T).Name, typeof(T).Name, ViewLevel.COMMON, out IUIView uiview, data, instant))
            {
                return uiview as T;
            }
            Debug.LogError($"加载UI视图 [{typeof(T).Name}] 失败.");
            return null;
        }
        /// <summary>
        /// 显示视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public static T Show<T>(IViewData data = null, bool instant = false) where T : UIView
        {
            return UI.Instance.ShowView(typeof(T).Name, data, instant) as T;
        }
        /// <summary>
        /// 显示视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public static T Show<T>(string viewName, IViewData data = null, bool instant = false) where T : UIView
        {
            return UI.Instance.ShowView(viewName, data, instant) as T;
        }
        /// <summary>
        /// 隐藏视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="instant">是否立即隐藏</param>
        /// <returns>视图</returns>
        public static T Hide<T>(bool instant = false) where T : UIView
        {
            return UI.Instance.HideView(typeof(T).Name, instant) as T;
        }
        /// <summary>
        /// 隐藏视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即隐藏</param>
        /// <returns>视图</returns>
        public static T Hide<T>(string viewName, bool instant = false) where T : UIView
        {
            return UI.Instance.HideView(viewName, instant) as T;
        }
        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <returns>视图</returns>
        public static T Get<T>() where T : UIView
        {
            return UI.Instance.GetView(typeof(T).Name) as T;
        }
        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <returns>视图</returns>
        public static T Get<T>(string viewName) where T : UIView
        {
            return UI.Instance.GetView(viewName) as T;
        }
        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="instant">是否立即卸载</param>
        /// <returns>成功卸载返回true 否则返回false</returns>
        public static bool Unload<T>(bool instant = false) where T : UIView
        {
            return UI.Instance.UnloadView(typeof(T).Name, instant);
        }
        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即卸载</param>
        /// <returns>成功卸载返回true 否则返回false</returns>
        public static bool Unload<T>(string viewName, bool instant = false) where T : UIView
        {
            return UI.Instance.UnloadView(viewName, instant);
        }
        /// <summary>
        /// 卸载所有视图
        /// </summary>
        public static void UnloadAll()
        {
            UI.Instance.UnloadAll();
        }
        #endregion
    }
}