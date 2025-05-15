using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public enum InventoryCardType
    {
        Equipment,
        Artifact
        // BattleCard는 제거: 별도 함수로 분리함
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
        /// 아티팩트 / 장비 카드 프리팹 생성
        /// </summary>
        public GameObject CreateCard<T>(T data, InventoryCardType type, Transform parent, UnityAction onClick = null)
        {
            GameObject prefab = GetPrefab(type);
            if (prefab == null)
            {
                Debug.LogWarning($"[InventoryUIManager] 카드 타입 {type}에 대한 프리팹이 지정되지 않았습니다.");
                return null;
            }

            GameObject card = Instantiate(prefab, parent);
            Debug.Log($"[CreateCard] 생성된 카드: {card.name}, 부모: {parent.name}");

            if (card.TryGetComponent(out InventoryCardUI ui))
            {
                Debug.Log("[CreateCard] InventoryCardUI Setup 진입");
                ui.Setup(data);
            }
            else
            {
                Debug.LogWarning($"[InventoryUIManager] 프리팹에 InventoryCardUI 컴포넌트가 없습니다. 타입: {type}");
            }

            var button = card.GetComponent<UnityEngine.UI.Button>();
            if (button != null && onClick != null)
                button.onClick.AddListener(onClick);

            return card;
        }

        /// <summary>
        /// 전투 카드 (Strike / Grace / Tactic / Item) 프리팹 생성
        /// </summary>
        public GameObject CreateBattleCard(CardData data, Transform parent, UnityAction onClick = null)
        {
            if (battleCardPrefab == null)
            {
                Debug.LogWarning("[InventoryUIManager] 전투 카드 프리팹이 지정되지 않았습니다.");
                return null;
            }

            GameObject card = Instantiate(battleCardPrefab, parent);
            Debug.Log($"[CreateBattleCard] 생성된 카드: {card.name}, 데이터: {data.cardName}");

            if (card.TryGetComponent(out CardUI battleUI))
            {
                Debug.Log("[CreateBattleCard] CardUI Setup 진입");
                battleUI.Setup(data);
            }
            else
            {
                Debug.LogWarning("[InventoryUIManager] 전투 카드 프리팹에 CardUI 컴포넌트가 없습니다.");
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