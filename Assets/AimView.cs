using UnityEngine;
using UnityEngine.UI;

public class AimView : MonoBehaviour
{
    //用于显示描述信息的文本
    [SerializeField] Text descriptionText;

    public void OnEnter(string description, Vector3 worldPos)
    {
        //描述信息
        descriptionText.text = description;
        //世界坐标转屏幕坐标 更新文本位置
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        descriptionText.rectTransform.anchoredPosition3D = screenPos;
    }
    public void OnExit()
    {
        descriptionText.text = null;
    }
}   