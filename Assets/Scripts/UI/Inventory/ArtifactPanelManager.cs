using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Inventory
{
    public class ArtifactPanelManager : MonoBehaviour
    {
        [Header("아티팩트 카드 표시 영역")]
        [SerializeField] private Transform cardParent;

        [Header("설명 패널")]
        [SerializeField] private GameObject descriptionPanel;
        [SerializeField] private Image artifactIcon;             
        [SerializeField] private TextMeshProUGUI artifactNameText;
        [SerializeField] private TextMeshProUGUI artifactDescText;

        private readonly List<GameObject> spawnedCards = new();

        private void Start()
        {
            RefreshPanel();
        }

        public void RefreshPanel()
        {
            ShowArtifacts(PlayerInventory.Instance.GetAllArtifacts());
        }

        private void ShowArtifacts(List<ArtifactData> artifacts)
        {
            ClearCards();

            foreach (var data in artifacts)
            {
                GameObject card = InventoryUIManager.Instance.CreateCard(
                    data: data,
                    type: InventoryCardType.Artifact,
                    parent: cardParent,
                    onClick: () => OnArtifactClicked(data)
                );

                if (card != null)
                    spawnedCards.Add(card);
            }
        }

        private void OnArtifactClicked(ArtifactData data)
        {
            descriptionPanel.SetActive(true);

            artifactNameText.text = data.displayName;
            artifactDescText.text = data.description;

            if (artifactIcon != null)
            {
                artifactIcon.sprite = data.icon;
                artifactIcon.preserveAspect = true;
            }
        }

        private void ClearCards()
        {
            foreach (var card in spawnedCards)
                Destroy(card);
            spawnedCards.Clear();
        }

        public void Close() => gameObject.SetActive(false);
        public void Open()
        {
            gameObject.SetActive(true);
            RefreshPanel();
        }
    }
}