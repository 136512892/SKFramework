using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 瞄准系统
    /// </summary>
    public class AimSystem : MonoBehaviour
    {
        #region NonPublic Variables
        private static AimSystem instance;
        //开关
        [SerializeField] private bool toggle = true;
        //瞄准相机
        [SerializeField] private Camera mainCamera;
        //瞄准检测层级
        [SerializeField] private LayerMask aimLayer;
        //瞄准最大检测距离
        [SerializeField] private float aimMaxDistance = 10f;
        //瞄准方式
        [SerializeField] private AimMode aimMode = AimMode.Mouse;
        #endregion

        #region Public Properties
        public static AimSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AimSystem>() ?? new GameObject("[SKFramework.Aim]").AddComponent<AimSystem>();
                }
                return instance;
            }
        }
        /// <summary>
        /// 开关
        /// </summary>
        public bool Toggle
        {
            get
            {
                return toggle;
            }
            set
            {
                if (toggle != value)
                {
                    toggle = value;
                    if (CurrentAimableObject != null)
                    {
                        CurrentAimableObject.Exit();
                        CurrentAimableObject = null;
                    }
                }
            }
        }
        /// <summary>
        /// 当前瞄准的物体
        /// </summary>
        public IAimableObject CurrentAimableObject { get; private set; }
        #endregion

        #region NonPublic Methods
        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main ?? FindObjectOfType<Camera>();
            }
            //如果使用鼠标方式瞄准 则不使用最大瞄准距离进行限制
            aimMaxDistance = aimMode == AimMode.Mouse ? float.MaxValue : aimMaxDistance;
        }
        private void Update()
        {
            if (!toggle) return;
            Ray ray = aimMode == AimMode.Mouse
                ? mainCamera.ScreenPointToRay(Input.mousePosition)
                : mainCamera.ViewportPointToRay(Vector2.one * .5f);
            if (Physics.Raycast(ray, out RaycastHit hit, aimMaxDistance, aimLayer))
            {
                IAimableObject obj = hit.collider.GetComponent<IAimableObject>();
                if (obj != CurrentAimableObject)
                {
                    CurrentAimableObject?.Exit();
                    CurrentAimableObject = obj;
                    CurrentAimableObject?.Enter();
                }
            }
            else
            {
                if (CurrentAimableObject != null)
                {
                    CurrentAimableObject.Exit();
                    CurrentAimableObject = null;
                }
            }
            CurrentAimableObject?.Stay();
        }
        private void OnDestroy()
        {
            if (CurrentAimableObject != null)
            {
                CurrentAimableObject.Exit();
                CurrentAimableObject = null;
            }
            instance = null;
        }
        #endregion
    }
}