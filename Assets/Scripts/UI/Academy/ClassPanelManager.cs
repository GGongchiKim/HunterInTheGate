using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassPanelManager : MonoBehaviour
{
    [Header("수업 슬롯 관리")]
    public Transform classSlotParent;
    public GameObject classSlotPrefab;

    [Header("UI 패널")]
    public GameObject classInformationPanel;
    public GameObject classSlotPanel;

    [Header("정보 패널 내부 UI")]
    public Image classIconImage;
    public TMP_Text classNameText;
    public TMP_Text stat1NameText;
    public TMP_Text stat1ValueText;
    public TMP_Text stat2NameText;
    public TMP_Text stat2ValueText;

    [Header("보상 카드 프리뷰 슬롯")]
    public CardPreviewSlot[] cardPreviewSlots;

    [System.Serializable]
    public class CardPreviewSlot
    {
        public TMP_Text cardNameText;
        public Image cardSpriteImage;
        public TMP_Text cardDescriptionText;
    }

    public Button attendButton;
    public ClassAnimationController animationController;

    private ClassData selectedClass;
    private List<GameObject> spawnedSlots = new();

    private void OnEnable()
    {
        GenerateClassSlots();
    }

    private void GenerateClassSlots()
    {
        foreach (var obj in spawnedSlots)
            Destroy(obj);
        spawnedSlots.Clear();

        foreach (var data in ClassDatabase.Instance.GetAvailableClasses())
        {
            GameObject slot = Instantiate(classSlotPrefab, classSlotParent);
            ClassSlotUI ui = slot.GetComponent<ClassSlotUI>();
            ui.Initialize(data, () => OnSelectClass(data));
            spawnedSlots.Add(slot);
        }
    }

    public void OnSelectClass(ClassData classData)
    {
        selectedClass = classData;
        UpdateClassInfoPanel();
        classInformationPanel.SetActive(true);
        attendButton.interactable = true;
        attendButton.onClick.RemoveAllListeners();
        attendButton.onClick.AddListener(() =>
        {
            SetClassUIPanelsActive(false);
            animationController.StartClassRoutine(classData);
        });
    }

    public void SetClassUIPanelsActive(bool isActive)
    {
        classInformationPanel.SetActive(isActive);
        classSlotPanel.SetActive(isActive);
    }

    public void OnClickReturn() 
    {
        this.gameObject.SetActive(false);
    
    
    }

    private void UpdateClassInfoPanel()
    {
        classNameText.text = selectedClass.className;
        classIconImage.sprite = selectedClass.classIcon;
        stat1NameText.text = stat1ValueText.text = stat2NameText.text = stat2ValueText.text = "";

        if (selectedClass.statModifiers.Count > 0)
        {
            stat1NameText.text = selectedClass.statModifiers[0].statType.ToString();
            stat1ValueText.text = GetStatSymbol(selectedClass.statModifiers[0].amount);
        }
        if (selectedClass.statModifiers.Count > 1)
        {
            stat2NameText.text = selectedClass.statModifiers[1].statType.ToString();
            stat2ValueText.text = GetStatSymbol(selectedClass.statModifiers[1].amount);
        }

        for (int i = 0; i < cardPreviewSlots.Length; i++)
        {
            if (i < selectedClass.previewCards.Count)
            {
                var card = selectedClass.previewCards[i];
                cardPreviewSlots[i].cardNameText.text = card.cardName;
                cardPreviewSlots[i].cardSpriteImage.sprite = card.cardSprite;
                cardPreviewSlots[i].cardDescriptionText.text = card.cardDescription;
            }
            else
            {
                cardPreviewSlots[i].cardNameText.text = "";
                cardPreviewSlots[i].cardSpriteImage.sprite = null;
                cardPreviewSlots[i].cardDescriptionText.text = "";
            }
        }
    }

    private string GetStatSymbol(int amount)
    {
        if (amount >= 8) return "++";
        if (amount >= 3) return "+";
        if (amount <= -4) return "--";
        if (amount <= -1) return "-";
        return "";
    }

    

}
