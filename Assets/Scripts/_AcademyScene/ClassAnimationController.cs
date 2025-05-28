using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassAnimationController : MonoBehaviour
{
    [Header("애니메이션 관련")]
    public GameObject classAnimationPanel;
    public GameObject classRewardPanel;
    public ClassRewardManager rewardManager;
    public Image cardFrontImage;
    public Image cardBackImage;

    [Header("카드 회전용")]
    public Transform cardHolderTransform;
    public float flipDuration = 1.0f;

    [Header("결과 능력치 UI")]
    public TMP_Text resultStatLabel1;
    public Slider resultStatBar1;
    public TMP_Text resultStatText1;
    public TMP_Text resultStatLabel2;
    public Slider resultStatBar2;
    public TMP_Text resultStatText2;

    [Header("외부 연동용 콜백")]
    public Action<bool> onToggleClassUIPanels;  // 클래스 패널 켜고 끄기용 콜백

    private ClassData currentClass;

    public void StartClassRoutine(ClassData classData)
    {
        currentClass = classData;
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        onToggleClassUIPanels?.Invoke(false);  // 수업 UI 숨기기
        classAnimationPanel.SetActive(true);
        classRewardPanel.SetActive(false);

        SetInitialResultStatsFromPlayer();

        // 카드 앞면 표시
        cardFrontImage.sprite = currentClass.frontImageSprite;
        cardFrontImage.gameObject.SetActive(true);
        cardBackImage.gameObject.SetActive(false);
        cardHolderTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSeconds(0.5f);

        // 카드 뒤집기
        ClassResult result = RollClassResult();
        yield return StartCoroutine(FlipCard(GetResultSprite(result)));

        // 능력치 적용 및 애니메이션
        float multiplier = GetMultiplier(result);
        var finalMods = ApplyStatMultiplier(currentClass.statModifiers, multiplier);
        GameContext.Instance.academyPlayer.ApplyModifiers(finalMods);
        yield return new WaitForEndOfFrame();
        ShowResultStatAnimation(finalMods);

        yield return new WaitForSeconds(1.0f); // 연출 감상

        classAnimationPanel.SetActive(false);
        classRewardPanel.SetActive(true);
        rewardManager.ShowRewards(currentClass);
    }

    private void SetInitialResultStatsFromPlayer()
    {
        var player = GameContext.Instance.academyPlayer;

        if (currentClass.statModifiers.Count > 0)
        {
            var stat = currentClass.statModifiers[0].statType;
            int value = GetStatValue(player, stat);
            resultStatLabel1.text = stat.ToString();
            resultStatText1.text = value.ToString();
            resultStatBar1.maxValue = 999f;
            resultStatBar1.value = value;
        }

        if (currentClass.statModifiers.Count > 1)
        {
            var stat = currentClass.statModifiers[1].statType;
            int value = GetStatValue(player, stat);
            resultStatLabel2.text = stat.ToString();
            resultStatText2.text = value.ToString();
            resultStatBar2.maxValue = 999f;
            resultStatBar2.value = value;
        }
    }

    private void ShowResultStatAnimation(List<StatModifier> modifiers)
    {
        var player = GameContext.Instance.academyPlayer;

        for (int i = 0; i < modifiers.Count && i < 2; i++)
        {
            var mod = modifiers[i];
            int to = GetStatValue(player, mod.statType);
            int from = to - mod.amount;

            if (i == 0)
            {
                resultStatLabel1.text = mod.statType.ToString();
                StartCoroutine(AnimateSlider(resultStatBar1, from, to, resultStatText1));
            }
            else
            {
                resultStatLabel2.text = mod.statType.ToString();
                StartCoroutine(AnimateSlider(resultStatBar2, from, to, resultStatText2));
            }
        }
    }

    private IEnumerator AnimateSlider(Slider slider, int from, int to, TMP_Text text)
    {
        slider.maxValue = 999f;
        slider.value = from;
        text.text = from.ToString();

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

    private IEnumerator FlipCard(Sprite resultSprite)
    {
        float half = flipDuration / 2f;
        float t = 0f;

        while (t < half)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(0f, 90f, t / half);
            cardHolderTransform.localRotation = Quaternion.Euler(0f, y, 0f);
            yield return null;
        }

        cardFrontImage.gameObject.SetActive(false);
        cardBackImage.sprite = resultSprite;
        cardBackImage.gameObject.SetActive(true);

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(90f, 180f, t / half);
            cardHolderTransform.localRotation = Quaternion.Euler(0f, y, 0f);
            yield return null;
        }

        cardHolderTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);
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
        int roll = UnityEngine.Random.Range(0, 100);

        if (stress <= 30) return roll < 20 ? ClassResult.GreatSuccess : roll < 90 ? ClassResult.Success : ClassResult.Failure;
        if (stress <= 70) return roll < 10 ? ClassResult.GreatSuccess : roll < 70 ? ClassResult.Success : ClassResult.Failure;
        return roll < 5 ? ClassResult.GreatSuccess : roll < 50 ? ClassResult.Success : ClassResult.Failure;
    }

    private Sprite GetResultSprite(ClassResult result)
    {
        return result switch
        {
            ClassResult.Success => currentClass.resultSuccess,
            ClassResult.GreatSuccess => currentClass.resultGreatSuccess,
            ClassResult.Failure => currentClass.resultFailure,
            _ => currentClass.resultFailure
        };
    }

    private float GetMultiplier(ClassResult result)
    {
        return result switch
        {
            ClassResult.Success => 1f,
            ClassResult.GreatSuccess => 1.5f,
            ClassResult.Failure => 0.5f,
            _ => 1f
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

    private enum ClassResult { Success, GreatSuccess, Failure }
}