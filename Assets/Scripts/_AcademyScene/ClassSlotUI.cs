using UnityEngine;
using UnityEngine.UI;
public class ClassSlotUI : MonoBehaviour
{
    public Text classNameText;
    private ClassData data;
    private System.Action onClick;

    public void Initialize(ClassData classData, System.Action onClickCallback)
    {
        data = classData;
        classNameText.text = data.className;
        onClick = onClickCallback;
    }

    public void OnClick()
    {
        onClick?.Invoke();
    }
}
