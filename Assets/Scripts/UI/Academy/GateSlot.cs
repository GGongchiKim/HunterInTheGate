using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GateSlot : MonoBehaviour
{
    [SerializeField] private Image gateImage;
    [SerializeField] private TextMeshProUGUI gateNameText;

    private GateData gateData;
    private Action<GateData> onClicked;

    public void SetData(GateData data, Action<GateData> clickCallback)
    {
        gateData = data;
        gateImage.sprite = data.gateIcon;
        gateNameText.text = data.gateName;
        onClicked = clickCallback;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onClicked?.Invoke(gateData);
    }
}