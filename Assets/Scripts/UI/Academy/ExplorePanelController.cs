using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    [Header("��ư ����")]
    [SerializeField] private Button attendButton;
    [SerializeField] private Button returnButton;

    private void OnEnable()
    {
        Initialize();
        attendButton.onClick.AddListener(OnClickAttend);
        returnButton.onClick.AddListener(OnClickReturn);
    }

    public void Initialize()
    {
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

    private void OnGateSelected(GateData gate)
    {
        currentGate = gate;

        // 1. ����Ʈ ������ �� ����
        if (gateIconImage != null)
            gateIconImage.sprite = gate.gateIcon;

        if (gateTitleText != null)
            gateTitleText.text = gate.gateName;

        // 2. ���� �̹��� ����
        for (int i = 0; i < monsterImages.Count; i++)
        {
            monsterImages[i].sprite = (i < gate.monsterSprites.Count) ? gate.monsterSprites[i] : null;
        }

        // 3. ��Ƽ��Ʈ ���� ����
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

        // SceneDataKey�� ���� �������� Ű ����
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