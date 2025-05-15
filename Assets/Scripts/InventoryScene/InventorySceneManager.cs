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
        /// ��ư���� �����ϱ� ���� �Ű����� ���� �Լ���
        /// </summary>
        public void OpenEquipmentTab() => OpenTab(InventoryTabType.Equipment);
        public void OpenArtifactTab() => OpenTab(InventoryTabType.Artifact);
        public void OpenDeckTab() => OpenTab(InventoryTabType.Deck);

        /// <summary>
        /// ������ ���� ���� �ش� �г��� �ʱ�ȭ
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