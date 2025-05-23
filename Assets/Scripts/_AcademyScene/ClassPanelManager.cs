using System.Collections;
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
    public GameObject classAnimationPanel;
    public GameObject classRewardPanel;

    [Header("정보 패널 내부 UI")]
    public TMP_Text classNameText;
    public TMP_Text stat1NameText;
    public TMP_Text stat1ValueText;
    public TMP_Text stat2NameText;
    public TMP_Text stat2ValueText;

    [Header("애니메이션 관련")]
    public Image cardFrontImage;
    public Image cardBackImage;
    public Sprite successSprite;
    public Sprite greatSuccessSprite;
    public Sprite failureSprite;

    [Header("보상 카드 선택")]
    public Button[] rewardButtons;
    public Text[] rewardTexts;

    public Button attendButton;

    private ClassData selectedClass;
    private List<GameObject> spawnedSlots = new();
    private List<CardData> rewardPool = new();

    private void OnEnable()
    {
        GenerateClassSlots();
    }

    private void GenerateClassSlots()
    {
        List<ClassData> classList = ClassDatabase.Instance.GetAvailableClasses();

        foreach (var obj in spawnedSlots)
            Destroy(obj);
        spawnedSlots.Clear();

        foreach (var data in classList)
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
        attendButton.onClick.AddListener(() => StartCoroutine(StartClassRoutine()));
    }

    private void UpdateClassInfoPanel()
    {
        classNameText.text = selectedClass.className;

        stat1NameText.text = "";
        stat1ValueText.text = "";
        stat2NameText.text = "";
        stat2ValueText.text = "";

        if (selectedClass.statModifiers.Count > 0)
        {
            stat1NameText.text = selectedClass.statModifiers[0].statType.ToString();
            stat1ValueText.text = new string('+', selectedClass.statModifiers[0].amount);
        }
        if (selectedClass.statModifiers.Count > 1)
        {
            stat2NameText.text = selectedClass.statModifiers[1].statType.ToString();
            stat2ValueText.text = new string('+', selectedClass.statModifiers[1].amount);
        }
    }

    private IEnumerator StartClassRoutine()
    {
        classInformationPanel.SetActive(false);
        classAnimationPanel.SetActive(true);

        cardFrontImage.gameObject.SetActive(false);
        cardBackImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.2f);

        ClassResult result = RollClassResult();
        Sprite resultSprite = GetResultSprite(result);
        cardBackImage.sprite = resultSprite;

        yield return new WaitForSeconds(1.2f);

        float multiplier = GetMultiplier(result);
        List<StatModifier> finalMods = ApplyStatMultiplier(selectedClass.statModifiers, multiplier);
        GameContext.Instance.academyPlayer.ApplyModifiers(finalMods);

        ShowRewardCardOptions(selectedClass.rewardPool);
    }

    private ClassResult RollClassResult()
    {
        int roll = Random.Range(0, 100);
        if (roll < 20) return ClassResult.Failure;
        else if (roll < 80) return ClassResult.Success;
        else return ClassResult.GreatSuccess;
    }

    private Sprite GetResultSprite(ClassResult result)
    {
        return result switch
        {
            ClassResult.Success => successSprite,
            ClassResult.GreatSuccess => greatSuccessSprite,
            ClassResult.Failure => failureSprite,
            _ => failureSprite
        };
    }

    private float GetMultiplier(ClassResult result)
    {
        return result switch
        {
            ClassResult.Success => 1.0f,
            ClassResult.GreatSuccess => 1.5f,
            ClassResult.Failure => 0.5f,
            _ => 1.0f
        };
    }

    private List<StatModifier> ApplyStatMultiplier(List<StatModifier> baseMods, float multiplier)
    {
        List<StatModifier> result = new();
        foreach (var mod in baseMods)
        {
            result.Add(new StatModifier
            {
                statType = mod.statType,
                amount = Mathf.CeilToInt(mod.amount * multiplier)
            });
        }
        return result;
    }

    private void ShowRewardCardOptions(List<CardData> rewardPool)
    {
        classAnimationPanel.SetActive(false);
        classRewardPanel.SetActive(true);
        // 추후 구현 예정
    }

    private enum ClassResult { Success, GreatSuccess, Failure }
}