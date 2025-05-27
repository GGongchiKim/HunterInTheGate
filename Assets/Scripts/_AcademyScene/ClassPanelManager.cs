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

    [Header("보상 카드 프리뷰 슬롯 (정적 4개)")]
    public CardPreviewSlot[] cardPreviewSlots;

    [System.Serializable]
    public class CardPreviewSlot
    {
        public TMP_Text cardNameText;
        public Image cardSpriteImage;
        public TMP_Text cardDescriptionText;
    }

    [Header("애니메이션 관련")]
    public Image cardFrontImage;
    public Image cardBackImage;

    [Header("보상 카드 선택")]
    public Button[] rewardButtons;
    public Text[] rewardTexts;

    [Header("능력치 결과 애니메이션 바")]
    public TMP_Text resultStatLabel1;
    public Slider resultStatBar1;
    public TMP_Text resultStatText1;
    public TMP_Text resultStatLabel2;
    public Slider resultStatBar2;
    public TMP_Text resultStatText2;

    public Button attendButton;

    private ClassData selectedClass;
    private List<GameObject> spawnedSlots = new();
    private List<CardData> rewardPool = new();

    private void OnEnable()
    {
        classAnimationPanel.SetActive(false);
        classRewardPanel.SetActive(false);
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
            stat1ValueText.text = GetStatSymbol(selectedClass.statModifiers[0].amount);
        }
        if (selectedClass.statModifiers.Count > 1)
        {
            stat2NameText.text = selectedClass.statModifiers[1].statType.ToString();
            stat2ValueText.text = GetStatSymbol(selectedClass.statModifiers[1].amount);
        }

        for (int i = 0; i < cardPreviewSlots.Length; i++)
        {
            if (i < selectedClass.previewCards.Count && selectedClass.previewCards[i] != null)
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
        ShowResultStatAnimation(finalMods);

        ShowRewardCardOptions(selectedClass.rewardPool);
    }

    private void ShowResultStatAnimation(List<StatModifier> modifiers)
    {
        var player = GameContext.Instance.academyPlayer;

        resultStatLabel1.text = "";
        resultStatText1.text = "";
        resultStatBar1.value = 0;

        resultStatLabel2.text = "";
        resultStatText2.text = "";
        resultStatBar2.value = 0;

        for (int i = 0; i < modifiers.Count && i < 2; i++)
        {
            var mod = modifiers[i];
            int original = GetStatValue(player, mod.statType) - mod.amount;
            int updated = original + mod.amount;

            if (i == 0)
            {
                resultStatLabel1.text = mod.statType.ToString();
                resultStatText1.text = original.ToString();
                resultStatBar1.maxValue = 999f;
                StartCoroutine(AnimateSlider(resultStatBar1, original, updated, resultStatText1, mod.statType));
            }
            else if (i == 1)
            {
                resultStatLabel2.text = mod.statType.ToString();
                resultStatText2.text = original.ToString();
                resultStatBar2.maxValue = 999f;
                StartCoroutine(AnimateSlider(resultStatBar2, original, updated, resultStatText2, mod.statType));
            }
        }
    }

    private IEnumerator AnimateSlider(Slider slider, int from, int to, TMP_Text text, StatType statType)
    {
        float duration = 0.5f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / duration);
            slider.value = val;
            text.text = Mathf.RoundToInt(val).ToString();
            yield return null;
        }
        slider.value = to;
        text.text = to.ToString();
    }

    private int GetStatValue(AcademyPlayer player, StatType type)
    {
        return type switch
        {
            StatType.Strength => player.combat.strength,
            StatType.Agility => player.combat.agility,
            StatType.Magic => player.combat.magic,
            StatType.Insight => player.combat.insight,
            StatType.WillPower => player.combat.willPower,
            StatType.Wit => player.combat.wit,
            StatType.Charisma => player.relation.charisma,
            StatType.Luck => player.relation.luck,
            StatType.Fame => player.relation.fame,
            StatType.Mood => player.condition.mood,
            StatType.Stress => player.condition.stress,
            _ => 0
        };
    }

    private ClassResult RollClassResult()
    {
        int stress = GameContext.Instance.academyPlayer.condition.stress;
        int roll = Random.Range(0, 100);

        if (stress <= 30) // 무드1
        {
            if (roll < 20) return ClassResult.GreatSuccess;
            if (roll < 90) return ClassResult.Success;
            return ClassResult.Failure;
        }
        else if (stress <= 70) // 무드2
        {
            if (roll < 10) return ClassResult.GreatSuccess;
            if (roll < 70) return ClassResult.Success;
            return ClassResult.Failure;
        }
        else // 무드3
        {
            if (roll < 5) return ClassResult.GreatSuccess;
            if (roll < 50) return ClassResult.Success;
            return ClassResult.Failure;
        }
    }

    private Sprite GetResultSprite(ClassResult result)
    {
        return result switch
        {
            ClassResult.Success => selectedClass.resultSuccess,
            ClassResult.GreatSuccess => selectedClass.resultGreatSuccess,
            ClassResult.Failure => selectedClass.resultFailure,
            _ => selectedClass.resultFailure
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