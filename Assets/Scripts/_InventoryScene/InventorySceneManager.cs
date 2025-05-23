using UnityEngine;

namespace Inventory
{
    public enum InventoryTabType
    {
        Equipment = 0,
        Artifact = 1,
        Deck = 2
    }

    public class InventorySceneManager : MonoBehaviour
    {
        [Header("Panel References")]
        [SerializeField] private GameObject equipmentPanel;
        [SerializeField] private GameObject artifactPanel;
        [SerializeField] private GameObject deckPanel;

        [Header("Panel Managers")]
        [SerializeField] private EquipmentPanelManager equipManager;
        [SerializeField] private ArtifactPanelManager artifactManager;
        [SerializeField] private DeckPanelManager deckManager;

        private void Start()
        {
            OpenTab(InventoryTabType.Deck);
        }

        /// <summary>
        /// 버튼에서 연결하기 위한 매개변수 없는 함수들
        /// </summary>
        public void OpenEquipmentTab() => OpenTab(InventoryTabType.Equipment);
        public void OpenArtifactTab() => OpenTab(InventoryTabType.Artifact);
        public void OpenDeckTab() => OpenTab(InventoryTabType.Deck);

        /// <summary>
        /// 지정된 탭을 열고 해당 패널을 초기화
        /// </summary>
        private void OpenTab(InventoryTabType tabType)
        {
            equipmentPanel.SetActive(false);
            artifactPanel.SetActive(false);
            deckPanel.SetActive(false);

            switch (tabType)
            {
                case InventoryTabType.Equipment:
                    equipmentPanel.SetActive(true);
                    equipManager.RefreshPanel();
                    break;
                case InventoryTabType.Artifact:
                    artifactPanel.SetActive(true);
                    artifactManager.RefreshPanel();
                    break;
                case InventoryTabType.Deck:
                    deckPanel.SetActive(true);
                    deckManager.RefreshPanel();
                    break;
            }
        }
    }
}