using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ClassSlotUI : MonoBehaviour
{
    public TextMeshProUGUI classNameText;
    public Image iconSprite;
    private ClassData data;
    private System.Action onClick;

    public void Initialize(ClassData classData, System.Action onClickCallback)
    {
        
        data = classData;
        iconSprite.sprite = classData.classIcon; 
        classNameText.text = data.className;
        onClick = onClickCallback;
    }

    public void OnClick()
    {
        onClick?.Invoke();
    }
}
