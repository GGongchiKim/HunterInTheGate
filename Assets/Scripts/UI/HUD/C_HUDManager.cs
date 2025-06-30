using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Deck/Discard Count UI")]
    [SerializeField] private TextMeshProUGUI deckCountText;
    [SerializeField] private TextMeshProUGUI discardCountText;

    [Header("Deck Viewer Panel")]
    [SerializeField] private GameObject deckViewerPanel;


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

    private void Start()
    {
        // 기존 코드 유지 + 덱 카운트 이벤트 연결
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnDeckChanged += UpdateDeckAndDiscardCount;
            DeckManager.Instance.OnDiscardChanged += UpdateDeckAndDiscardCount;
        }

        UpdateDeckAndDiscardCount();
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
            if (pair.Key == null) continue; // 🔐 예외 방지: 이미 파괴된 Enemy
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

    /// <summary>
    /// 적이 사망하거나 제거될 때 해당 적의 HUD 정보 정리
    /// </summary>
    public void UnregisterEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        if (enemyHealthUIs.ContainsKey(enemy))
        {
            enemyHealthUIs.Remove(enemy);
            Debug.Log($"[HUDManager] 적 HP UI 해제됨: {enemy.name}");
        }

        if (enemyStatusPanels.ContainsKey(enemy))
        {
            enemyStatusPanels.Remove(enemy);
            Debug.Log($"[HUDManager] 적 상태 패널 해제됨: {enemy.name}");
        }

        // Intent UI까지 관리 중이라면 이 부분도 필요할 수 있음
        // enemyIntentSlots.RemoveAll(slot => slot.linkedEnemy == enemy);
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

    private void OnDestroy()
    {
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnDeckChanged -= UpdateDeckAndDiscardCount;
            DeckManager.Instance.OnDiscardChanged -= UpdateDeckAndDiscardCount;
        }
    }

    /// <summary>
    /// 외부에서 생성된 DeckViewerPanel을 등록함 (비활성 상태)
    /// </summary>
    public void SetDeckViewerPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogWarning("[C_HUDManager] SetDeckViewerPanel() 호출됨 - 전달된 panel이 null입니다.");
            return;
        }

        deckViewerPanel = panel;
        deckViewerPanel.SetActive(false);

        Debug.Log("[C_HUDManager] DeckViewerPanel이 성공적으로 등록되었습니다.");
    }


    public void UpdateDeckAndDiscardCount()
    {
        if (deckCountText != null && DeckManager.Instance != null)
        {
            int count = DeckManager.Instance.GetCombatDeck().Count;
            deckCountText.text = count.ToString();
        }

        if (discardCountText != null && DeckManager.Instance != null)
        {
            int count = DeckManager.Instance.GetDiscardPile().Count;
            discardCountText.text = count.ToString();
        }
    }

    public void OpenDeckView()
    {
        if (C_DeckViewerManager.Instance != null)
            C_DeckViewerManager.Instance.OpenDeckOnly();
        else
            Debug.LogWarning("DeckViewerManager가 존재하지 않습니다.");
    }

    public void OpenDiscardView()
    {
        if (C_DeckViewerManager.Instance != null)
            C_DeckViewerManager.Instance.OpenDiscardOnly();
        else
            Debug.LogWarning("DeckViewerManager가 존재하지 않습니다.");
    }


}
