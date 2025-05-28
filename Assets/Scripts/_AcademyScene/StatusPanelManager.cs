using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanelManager : MonoBehaviour
{
    public static StatusPanelManager Instance { get; private set; }

    [Header("전투 능력치 UI 텍스트")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI magicText;
    public TextMeshProUGUI insightText;
    public TextMeshProUGUI willPowerText;
    public TextMeshProUGUI witText;

    [Header("관계 능력치 UI 텍스트")]
    public TextMeshProUGUI charismaText;
    public TextMeshProUGUI fameText;
    public TextMeshProUGUI luckText;

    [Header("컨디션 능력치 UI 텍스트")]
    public TextMeshProUGUI stressText;

    [Header("전투 능력치 슬라이더 바")]
    public Slider strengthBar;
    public Slider agilityBar;
    public Slider magicBar;
    public Slider insightBar;
    public Slider willPowerBar;
    public Slider witBar;

    [Header("관계 능력치 슬라이더 바")]
    public Slider charismaBar;
    public Slider fameBar;
    public Slider luckBar;

    [Header("컨디션 능력치 슬라이더 바")]
    public Slider StressBar;

    private const float maxStatValue = 999f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        var player = GameContext.Instance.academyPlayer;

        player.combat.OnStatsChanged += UpdateStatsUI;
        player.relation.OnStatsChanged += UpdateStatsUI;
        player.condition.OnStatsChanged += UpdateStatsUI;

        UpdateStatsUI(); // 초기값 표시
    }

    // 능력치 UI 업데이트
    public void UpdateStatsUI()
    {
        var player = GameContext.Instance.academyPlayer;

        strengthText.text = player.combat.strength.ToString();
        agilityText.text = player.combat.agility.ToString();
        insightText.text = player.combat.insight.ToString();
        magicText.text = player.combat.magic.ToString();
        willPowerText.text = player.combat.willPower.ToString();
        witText.text = player.combat.wit.ToString();
        charismaText.text = player.relation.charisma.ToString();
        fameText.text = player.relation.fame.ToString();
        luckText.text = player.relation.luck.ToString();
        stressText.text = player.condition.stress.ToString();

        strengthBar.value = (float)player.combat.strength / maxStatValue * strengthBar.maxValue;
        agilityBar.value = (float)player.combat.agility / maxStatValue * agilityBar.maxValue;
        insightBar.value = (float)player.combat.insight / maxStatValue * insightBar.maxValue;
        magicBar.value = (float)player.combat.magic / maxStatValue * magicBar.maxValue;
        willPowerBar.value = (float)player.combat.willPower / maxStatValue * willPowerBar.maxValue;
        witBar.value = (float)player.combat.wit / maxStatValue * witBar.maxValue;

        charismaBar.value = player.relation.charisma;
        fameBar.value = (float)player.relation.fame / maxStatValue * fameBar.maxValue;
        luckBar.value = (float)player.relation.luck;

        StressBar.value = player.condition.stress; // 스트레스는 0~100 기준 유지
    }
}