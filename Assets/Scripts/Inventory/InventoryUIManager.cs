using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public enum InventoryCardType
    {
        Equipment,
        Artifact
        // BattleCard�� ����: ���� �Լ��� �и���
    }

    public class InventoryUIManager : MonoBehaviour
    {
        public static InventoryUIManager Instance { get; private set; }

        [Header("Card Prefabs")]
        [SerializeField] private GameObject equipmentCardPrefab;
        [SerializeField] private GameObject artifactCardPrefab;
        [SerializeField] private GameObject battleCardPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// ��Ƽ��Ʈ / ��� ī�� ������ ����
        /// </summary>
        public GameObject CreateCard<T>(T data, InventoryCardType type, Transform parent, UnityAction onClick = null)
        {
            GameObject prefab = GetPrefab(type);
            if (prefab == null)
            {
                Debug.LogWarning($"[InventoryUIManager] ī�� Ÿ�� {type}�� ���� �������� �������� �ʾҽ��ϴ�.");
                return null;
            }

            GameObject card = Instantiate(prefab, parent);
            Debug.Log($"[CreateCard] ������ ī��: {card.name}, �θ�: {parent.name}");

            if (card.TryGetComponent(out InventoryCardUI ui))
            {
                Debug.Log("[CreateCard] InventoryCardUI Setup ����");
                ui.Setup(data);
            }
            else
            {
                Debug.LogWarning($"[InventoryUIManager] �����տ� InventoryCardUI ������Ʈ�� �����ϴ�. Ÿ��: {type}");
            }

            var button = card.GetComponent<UnityEngine.UI.Button>();
            if (button != null && onClick != null)
                button.onClick.AddListener(onClick);

            return card;
        }

        /// <summary>
        /// ���� ī�� (Strike / Grace / Tactic / Item) ������ ����
        /// </summary>
        public GameObject CreateBattleCard(CardData data, Transform parent, UnityAction onClick = null)
        {
            if (battleCardPrefab == null)
            {
                Debug.LogWarning("[InventoryUIManager] ���� ī�� �������� �������� �ʾҽ��ϴ�.");
                return null;
            }

            GameObject card = Instantiate(battleCardPrefab, parent);
            Debug.Log($"[CreateBattleCard] ������ ī��: {card.name}, ������: {data.cardName}");

            if (card.TryGetComponent(out CardUI battleUI))
            {
                Debug.Log("[CreateBattleCard] CardUI Setup ����");
                battleUI.Setup(data);
            }
            else
            {
                Debug.LogWarning("[InventoryUIManager] ���� ī�� �����տ� CardUI ������Ʈ�� �����ϴ�.");
            }

            var button = card.GetComponent<UnityEngine.UI.Button>();
            if (button != null && onClick != null)
                button.onClick.AddListener(onClick);

            return card;
        }

        private GameObject GetPrefab(InventoryCardType type)
        {
            return type switch
            {
                InventoryCardType.Equipment => equipmentCardPrefab,
                InventoryCardType.Artifact => artifactCardPrefab,
                _ => null
            };
        }
    }
}