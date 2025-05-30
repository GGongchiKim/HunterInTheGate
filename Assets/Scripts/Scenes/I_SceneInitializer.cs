using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
    public enum InventoryTabType
    {
        Equipment = 0,
        Artifact = 1,
        Deck = 2
    }

    public class I_SceneInitializer : MonoBehaviour
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

        public void OpenEquipmentTab() => OpenTab(InventoryTabType.Equipment);
        public void OpenArtifactTab() => OpenTab(InventoryTabType.Artifact);
        public void OpenDeckTab() => OpenTab(InventoryTabType.Deck);

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

        /// <summary>
        /// 인벤토리 씬을 닫을 때, 현재 덱 프리셋을 PlayerInventory에 반영(임시 저장)하고 아카데미 씬으로 이동
        /// </summary>
        public void CloseScene()
        {
            
            SceneManager.LoadScene("AcademyScene");
        }      
    }
}