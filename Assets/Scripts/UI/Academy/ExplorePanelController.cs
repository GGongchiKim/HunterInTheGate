using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaveSystem;

public class ExplorePanelController : MonoBehaviour
{
    [Header("Gate 관련")]
    [SerializeField] private Transform gateSlotParent;
    [SerializeField] private GateSlot gateSlotPrefab;
    [SerializeField] private List<GateData> gateList;
    private GateData currentGate;

    [Header("Gate Info UI")]
    [SerializeField] private Image gateIconImage;
    [SerializeField] private TextMeshProUGUI gateTitleText;
    [SerializeField] private List<Image> monsterImages;

    [Header("Artifact Info")]
    [SerializeField] private List<TextMeshProUGUI> artifactTitleTexts;
    [SerializeField] private List<Image> artifactImages;
    [SerializeField] private List<TextMeshProUGUI> artifactDescriptions;

    [Header("덱 관련 UI")]
    [SerializeField] private TMP_Dropdown deckDropdown;
    [SerializeField] private TextMeshProUGUI deckPreviewText;
    [SerializeField] private Image deckPreviewSprite;

    [Header("버튼 모음")]
    [SerializeField] private Button attendButton;
    [SerializeField] private Button returnButton;

    private List<DeckSaveData> deckPresets;
    private DeckSaveData selectedDeck;

    private void OnEnable()
    {
        Initialize();
        attendButton.onClick.AddListener(OnClickAttend);
        returnButton.onClick.AddListener(OnClickReturn);
    }

    public void Initialize()
    {
        // 덱 정보 불러오기
        deckPresets = GameContext.Instance.inventory.GetAllDeckPresets();
        InitDeckDropdown();

        // 기존 슬롯 초기화
        foreach (Transform child in gateSlotParent)
            Destroy(child.gameObject);

        // 게이트 슬롯 생성
        foreach (var gate in gateList)
        {
            var slot = Instantiate(gateSlotPrefab, gateSlotParent);
            slot.SetData(gate, OnGateSelected);
        }

        // 기본 선택
        if (gateList.Count > 0)
            OnGateSelected(gateList[0]);
    }

    private void InitDeckDropdown()
    {
        if (deckDropdown == null || deckPresets == null) return;

        deckDropdown.ClearOptions();
        List<string> optionNames = new();

        foreach (var deck in deckPresets)
            optionNames.Add(deck.deckName);

        deckDropdown.AddOptions(optionNames);
        deckDropdown.onValueChanged.AddListener(OnDeckDropdownChanged);

        // 기본 선택은 첫 번째
        if (deckPresets.Count > 0)
        {
            selectedDeck = deckPresets[0];
            UpdateDeckDisplay(selectedDeck);
        }
    }

    private void OnDeckDropdownChanged(int index)
    {
        if (index >= 0 && index < deckPresets.Count)
        {
            selectedDeck = deckPresets[index];
            UpdateDeckDisplay(selectedDeck);
        }
    }

    private void UpdateDeckDisplay(DeckSaveData deck)
    {
        if (deckPreviewText != null)
            deckPreviewText.text = deck.deckName;

        // TODO: deckPreviewSprite.sprite = deck.previewSprite (필요시)
    }

    private void OnGateSelected(GateData gate)
    {
        currentGate = gate;

        if (gateIconImage != null)
            gateIconImage.sprite = gate.gateIcon;

        if (gateTitleText != null)
            gateTitleText.text = gate.gateName;

        for (int i = 0; i < monsterImages.Count; i++)
        {
            monsterImages[i].sprite = (i < gate.monsterSprites.Count) ? gate.monsterSprites[i] : null;
        }

        for (int i = 0; i < artifactTitleTexts.Count; i++)
        {
            bool hasData = (i < gate.artifacts.Count);
            artifactTitleTexts[i].text = hasData ? gate.artifacts[i].title : "";
            artifactImages[i].sprite = hasData ? gate.artifacts[i].image : null;
            artifactDescriptions[i].text = hasData ? gate.artifacts[i].description : "";
        }
    }

    private void OnClickAttend()
    {
        if (currentGate == null || string.IsNullOrEmpty(currentGate.dialogueEventId))
        {
            Debug.LogWarning("[ExplorePanel] 현재 선택된 Gate 또는 dialogueEventId가 유효하지 않음.");
            return;
        }

        if (selectedDeck == null)
        {
            Debug.LogWarning("[ExplorePanel] 선택된 덱이 없습니다.");
            return;
        }

        // 현재 선택된 덱을 activeDeck으로 지정
        GameContext.Instance.inventory.SetActiveDeck(selectedDeck);

        // 덱 데이터 전달
        SceneDataBridge.Instance.SetData("SelectedDeck", selectedDeck);
        SceneDataBridge.Instance.SetData("SelectedDeckId", selectedDeck.deckId); // (필요 시)

        // 씬 전환
        SceneTransitionManager.Instance.LoadSceneWithFade(
            sceneName: "DialogueScene",
            nextPhase: GamePhase.Event,
            dataKey: currentGate.dialogueEventId
        );
    }

    private void OnClickReturn()
    {
        gameObject.SetActive(false); // 단순 패널 비활성화
    }
}