using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_HUDManager : MonoBehaviour
{
    public static C_HUDManager Instance { get; private set; }

    private Coroutine[] flashCoroutines;

    [Header("Player Health")]
    public Slider playerHealthBar;
    public Text playerHealthText;
    public Transform playerSpriteTransform;
    public RectTransform playerHealthUI;

    [Header("Enemy HUD Prefab")]
    public GameObject enemyHUDPrefab;
    public Transform enemyHUDParent;

    [Header("Status Panel")]
    public Transform playerStatusPanel;
    public GameObject statusEffectPrefab;

    [Header("패배 패널")]
    public GameObject defeatPanel;

    [Header("Enemy Intent UI")]
    public List<EnemyIntentUI> enemyIntentSlots;

    [Header("Action Points (AP)")]
    public Image[] apBeads;
    public Color beadDefaultColor = Color.white;
    public Color beadFlashColor = Color.yellow;
    public Color beadInsufficientColor = Color.red;

    [Header("Turn Info")]
    public Text turnCounterText;

    [Header("Turn End Button")]
    public Button TurnEndbutton;

    [Header("Health UI Offset")]
    public Vector3 uiOffset = new Vector3(0, 2f, 0);

    private Camera mainCam;

    private Dictionary<Enemy, RectTransform> enemyHealthUIs = new();
    private Dictionary<Enemy, Transform> enemyStatusPanels = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        flashCoroutines = new Coroutine[apBeads.Length];
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateHealthUIPosition();
    }

    private void UpdateHealthUIPosition()
    {
        UpdateWorldspaceHealthUI(playerSpriteTransform, playerHealthUI);

        foreach (var pair in enemyHealthUIs)
        {
            UpdateWorldspaceHealthUI(pair.Key.transform, pair.Value);
        }
    }

    private void UpdateWorldspaceHealthUI(Transform targetTransform, RectTransform uiElement)
    {
        if (targetTransform != null && uiElement != null)
        {
            SpriteRenderer sr = targetTransform.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                Vector3 footPosition = new Vector3(
                    sr.bounds.center.x,
                    sr.bounds.min.y - 0.05f,
                    sr.bounds.center.z
                );
                Vector3 screenPos = mainCam.WorldToScreenPoint(footPosition);
                uiElement.position = screenPos;
            }
        }
    }

    public void RegisterEnemyHUD(Enemy enemy, GameObject hud)
    {
        RectTransform hpBar = hud.transform.Find("EnemyHpBar")?.GetComponent<RectTransform>();
        Transform statusPanel = hud.transform.Find("EnemyStatusPanel");

        if (hpBar != null)
            enemyHealthUIs[enemy] = hpBar;
        if (statusPanel != null)
            enemyStatusPanels[enemy] = statusPanel;
    }

    public Transform GetEnemyStatusPanel(Enemy enemy)
    {
        return enemyStatusPanels.TryGetValue(enemy, out Transform panel) ? panel : null;
    }

    public void UpdatePlayerHealth(int currentHP, int maxHP, int shield = 0)
    {
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = maxHP;
            playerHealthBar.value = currentHP;

            Color targetColor = shield > 0 ? new Color(0.3f, 0.8f, 1f) : Color.red;
            playerHealthBar.fillRect.GetComponent<Image>().color = targetColor;
        }

        if (playerHealthText != null)
        {
            playerHealthText.text = shield > 0
                ? $"{currentHP} / {maxHP} + {shield}"
                : $"{currentHP} / {maxHP}";
        }
    }

    public void UpdateActionPoints(int currentAP)
    {
        for (int i = 0; i < apBeads.Length; i++)
        {
            apBeads[i].gameObject.SetActive(true);

            if (i < currentAP)
                apBeads[i].color = beadDefaultColor;
            else
                apBeads[i].color = new Color(0.3f, 0.3f, 0.3f, 1f);
        }
    }

    public void UpdateTurn(int turn)
    {
        if (turnCounterText != null)
            turnCounterText.text = "Turn: " + turn;
    }

    public void HighlightAPBeads(int cost)
    {
        int availableAP = CombatContext.Instance.combatPlayer.actionPoints;

        for (int i = 0; i < apBeads.Length; i++)
        {
            if (flashCoroutines[i] != null)
            {
                StopCoroutine(flashCoroutines[i]);
                flashCoroutines[i] = null;
            }

            if (i < availableAP)
            {
                if (i >= availableAP - cost)
                {
                    bool isAffordable = availableAP >= cost;
                    flashCoroutines[i] = StartCoroutine(FlashBead(apBeads[i], isAffordable));
                }
                else
                {
                    apBeads[i].color = beadDefaultColor;
                }
            }
            else
            {
                apBeads[i].color = new Color(0.3f, 0.3f, 0.3f, 1f);
            }
        }
    }

    public void ResetAPBeadColors()
    {
        for (int i = 0; i < apBeads.Length; i++)
        {
            if (flashCoroutines[i] != null)
            {
                StopCoroutine(flashCoroutines[i]);
                flashCoroutines[i] = null;
            }

            if (i < CombatContext.Instance.combatPlayer.actionPoints)
                apBeads[i].color = beadDefaultColor;
            else
                apBeads[i].color = new Color(0.3f, 0.3f, 0.3f, 1f);

            apBeads[i].gameObject.SetActive(true);
        }
    }

    private IEnumerator FlashBead(Image bead, bool isAffordable)
    {
        Color flashColor = isAffordable ? beadFlashColor : Color.red;
        Color originalColor = beadDefaultColor;

        while (true)
        {
            bead.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            bead.color = originalColor;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnTurnEndButton()
    {
        Debug.Log("턴 종료 버튼이 눌렸습니다.");
        TurnManager.Instance.EndTurn();
    }

    public void ShowDefeatPanel()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
    }

    

}
