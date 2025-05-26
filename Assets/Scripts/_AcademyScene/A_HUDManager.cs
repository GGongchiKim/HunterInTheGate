using UnityEngine;
using UnityEngine.UI;

public class A_HUDManager : MonoBehaviour
{
    public static A_HUDManager Instance { get; private set; }

    [Header("능력치 UI 텍스트")]
    public Text strengthText;
    public Text agilityText;
    public Text insightText;
    public Text magicText;
    public Text willPowerText;
    public Text witText;
    public Text charismaText;
    public Text luckText;

    [Header("능력치 슬라이더 바")]
    public Slider strengthBar;
    public Slider agilityBar;
    public Slider insightBar;
    public Slider magicBar;
    public Slider willPowerBar;
    public Slider witBar;
    public Slider charismaBar;
    public Slider luckBar;

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

        strengthText.text = "Strength: " + player.combat.strength;
        agilityText.text = "Agility: " + player.combat.agility;
        insightText.text = "Insight: " + player.combat.insight;
        magicText.text = "Magic: " + player.combat.magic;
        willPowerText.text = "Will Power: " + player.combat.willPower;
        witText.text = "Wit: " + player.combat.wit;
        charismaText.text = "Charisma: " + player.relation.charisma;
        luckText.text = "Luck: " + player.relation.luck;

        strengthBar.value = player.combat.strength;
        agilityBar.value = player.combat.agility;
        insightBar.value = player.combat.insight;
        magicBar.value = player.combat.magic;
        willPowerBar.value = player.combat.willPower;
        witBar.value = player.combat.wit;
        charismaBar.value = player.relation.charisma;
        luckBar.value = player.relation.luck;
    }
}