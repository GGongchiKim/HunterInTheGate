using UnityEngine;
using UnityEngine.UI;

public class ArcademyUIManager : MonoBehaviour
{
    public static ArcademyUIManager Instance { get; private set; }

    [Header("�ɷ�ġ UI �ؽ�Ʈ")]
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

    // �ɷ�ġ UI ������Ʈ
    public void UpdateStatsUI()
    {
        // AcademyContext�� �ɷ�ġ�� UI�� ����
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