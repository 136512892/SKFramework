using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    public class ImageContext
    {
        /// <summary>
        /// Image转RawImage
        /// </summary>
        [MenuItem("CONTEXT/Image/Convert 2 RawImage")]
        public static void Image2RawImage()
        {
            Image image = Selection.activeGameObject.GetComponent<Image>();
            //Image中Sprite不为空则获取其texture
            Texture2D texture2D = image.sprite ? image.sprite.texture : null;
            var raycastTarget = image.raycastTarget;
            //销毁Image组件
            Object.DestroyImmediate(image);
            //添加RawImage组件
            RawImage rawImage = Selection.activeGameObject.AddComponent<RawImage>();
            rawImage.texture = texture2D;
            rawImage.raycastTarget = raycastTarget;
            //SetDirty以保存
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
    }
}