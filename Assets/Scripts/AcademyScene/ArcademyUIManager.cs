using UnityEngine;
using UnityEngine.UI;

public class ArcademyUIManager : MonoBehaviour
{
    public static ArcademyUIManager Instance { get; private set; }

    [Header("능력치 UI 텍스트")]
    public Text strengthText;
    public Text agilityText;
    public Text insightText;
    public Text magicText;
    public Text willPowerText;
    public Text witText;
    public Text charismaText;
    public Text luckText;

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

    // 능력치 UI 업데이트
    public void UpdateStatsUI()
    {
        // AcademyContext의 능력치로 UI를 갱신
        strengthText.text = "Strength: " + GameContext.Instance.player.strength;
        agilityText.text = "Agility: " + GameContext.Instance.player.agility;
        insightText.text = "Insight: " + GameContext.Instance.player.insight;
        magicText.text = "Magic: " + GameContext.Instance.player.magic;
        willPowerText.text = "Will Power: " + GameContext.Instance.player.willPower;
        witText.text = "Wit: " + GameContext.Instance.player.wit;
        charismaText.text = "Charisma: " + GameContext.Instance.player.charisma;
        luckText.text = "Luck: " + GameContext.Instance.player.luck;
    }
}