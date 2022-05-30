using UnityEngine;
using UnityEngine.UI;

public class AimView : MonoBehaviour
{
    //������ʾ������Ϣ���ı�
    [SerializeField] Text descriptionText;

    public void OnEnter(string description, Vector3 worldPos)
    {
        //������Ϣ
        descriptionText.text = description;
        //��������ת��Ļ���� �����ı�λ��
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        descriptionText.rectTransform.anchoredPosition3D = screenPos;
    }
    public void OnExit()
    {
        descriptionText.text = null;
    }
}   