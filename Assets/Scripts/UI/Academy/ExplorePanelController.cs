using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    [Header("버튼 모음")]
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

    private void OnGateSelected(GateData gate)
    {
        currentGate = gate;

        // 1. 게이트 아이콘 및 제목
        if (gateIconImage != null)
            gateIconImage.sprite = gate.gateIcon;

        if (gateTitleText != null)
            gateTitleText.text = gate.gateName;

        // 2. 몬스터 이미지 설정
        for (int i = 0; i < monsterImages.Count; i++)
        {
            monsterImages[i].sprite = (i < gate.monsterSprites.Count) ? gate.monsterSprites[i] : null;
        }

        // 3. 아티팩트 정보 설정
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

        // SceneDataKey를 통한 안정적인 키 전달
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