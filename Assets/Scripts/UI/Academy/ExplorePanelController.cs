using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaveSystem;

public class ExplorePanelController : MonoBehaviour
{
    [Header("Gate ����")]
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

    [Header("�� ���� UI")]
    [SerializeField] private TMP_Dropdown deckDropdown;
    [SerializeField] private TextMeshProUGUI deckPreviewText;
    [SerializeField] private Image deckPreviewSprite;

    [Header("��ư ����")]
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
        // �� ���� �ҷ�����
        deckPresets = GameContext.Instance.inventory.GetAllDeckPresets();
        InitDeckDropdown();

        // ���� ���� �ʱ�ȭ
        foreach (Transform child in gateSlotParent)
            Destroy(child.gameObject);

        // ����Ʈ ���� ����
        foreach (var gate in gateList)
        {
            var slot = Instantiate(gateSlotPrefab, gateSlotParent);
            slot.SetData(gate, OnGateSelected);
        }

        // �⺻ ����
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

        // �⺻ ������ ù ��°
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

        // TODO: deckPreviewSprite.sprite = deck.previewSprite (�ʿ��)
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
            Debug.LogWarning("[ExplorePanel] ���� ���õ� Gate �Ǵ� dialogueEventId�� ��ȿ���� ����.");
            return;
        }

        if (selectedDeck == null)
        {
            Debug.LogWarning("[ExplorePanel] ���õ� ���� �����ϴ�.");
            return;
        }

        // ���� ���õ� ���� activeDeck���� ����
        GameContext.Instance.inventory.SetActiveDeck(selectedDeck);

        // �� ������ ����
        SceneDataBridge.Instance.SetData("SelectedDeck", selectedDeck);
        SceneDataBridge.Instance.SetData("SelectedDeckId", selectedDeck.deckId); // (�ʿ� ��)

        // �� ��ȯ
        SceneTransitionManager.Instance.LoadSceneWithFade(
            sceneName: "DialogueScene",
            nextPhase: GamePhase.Event,
            dataKey: currentGate.dialogueEventId
        );
    }

    private void OnClickReturn()
    {
        gameObject.SetActive(false); // �ܼ� �г� ��Ȱ��ȭ
    }
}