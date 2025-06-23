using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI cNameText;
    public TextMeshProUGUI cDescriptionText;
    public TextMeshProUGUI cCostText;
    public Image cCardSprite;

    private CardData cardData;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 originalPosition;
    private Transform originalParent;

    private bool isInteractable = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(CardData data)
    {
        if (data == null)
        {
            Debug.LogError("Card 데이터가 null입니다.");
            return;
        }
        Debug.Log($"[CardUI.Setup] 카드 설정: {data.cardName}");

        cardData = data;
        cNameText.text = cardData.cardName;
        cCostText.text = cardData.cardCost.ToString();
        cDescriptionText.text = cardData.cardDescription;

        if (cardData.cardSprite != null)
        {
            cCardSprite.sprite = cardData.cardSprite;
            cCardSprite.preserveAspect = true;
            cCardSprite.rectTransform.sizeDelta = new Vector2(100, 150);
        }
        else
        {
            cCardSprite.sprite = null;
        }
    }

    public CardData GetCardData() => cardData;

    public void SetInteractable(bool canInteract) => isInteractable = canInteract;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        StartDragVisuals();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractable || canvas == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, cam, out Vector2 localPoint))
        {
            rectTransform.position = Input.mousePosition;
        }

        C_HUDManager.Instance?.HighlightAPBeads(cardData.cardCost);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        ResetDragVisuals();

        GameObject uiTarget = GetUIRaycastTarget(eventData);
        GameObject worldTarget = GetWorldRaycastTarget();

        HandleDrop(uiTarget, worldTarget);
    }

    private void StartDragVisuals()
    {
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
        }
    }

    private void ResetDragVisuals()
    {
        if (canvas != null)
        {
            canvas.overrideSorting = false;
            canvas.sortingOrder = 0;
        }

        transform.SetParent(originalParent, true);
        C_HUDManager.Instance?.ResetAPBeadColors();
    }

    private GameObject GetUIRaycastTarget(PointerEventData eventData)
    {
        List<RaycastResult> uiHits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, uiHits);

        foreach (RaycastResult hit in uiHits)
        {
            if (hit.gameObject.CompareTag("HandArea"))
            {
                return hit.gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// 카드 UI 이미지의 RaycastTarget을 켜거나 끕니다.
    /// </summary>
    public void SetRaycastBlock(bool isBlocked)
    {
        // 현재 오브젝트 및 자식 전체에서 Image와 TMP_Text 컴포넌트 검색
        var images = GetComponentsInChildren<Image>(includeInactive: true);
        foreach (var img in images)
        {
            img.raycastTarget = !isBlocked;
        }

        var texts = GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
        foreach (var text in texts)
        {
            text.raycastTarget = !isBlocked;
        }
    }


    private GameObject GetWorldRaycastTarget()
    {
        Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.zero);

        return hit2D.collider != null ? hit2D.collider.gameObject : null;
    }

    private void HandleDrop(GameObject uiTarget, GameObject worldTarget)
    {
        switch (GameStateManager.Instance.CurrentPhase)
        {
            case GamePhase.Combat:
                HandleCombatDrop(uiTarget, worldTarget);
                break;

            case GamePhase.Management:
                HandleInventoryDrop(uiTarget);
                break;

            default:
                ReturnToOriginalPosition();
                break;
        }
    }

    private void HandleCombatDrop(GameObject uiTarget, GameObject worldTarget)
    {
        bool used = false;

        if (cardData.cardEffect != null)
        {
            if (worldTarget != null && cardData.cardEffect.IsValidTarget(worldTarget))
            {
                used = cardData.cardEffect.ExecuteEffect(CombatContext.Instance, cardData, worldTarget);
            }
            else if (cardData.cardEffect.AllowsGlobalDrop())
            {
                used = cardData.cardEffect.ExecuteEffect(CombatContext.Instance, cardData, null);
            }
        }

        if (used)
        {
            if (!TutorialCombatHelper.IsCardUsable(cardData.cardId))
            {
                Debug.Log($"[CardUI] 튜토리얼 제한으로 인해 카드 사용 불가: {cardData.cardId}");
                ReturnToOriginalPosition();
                return;
            }

            Debug.Log($"[CardUI] 카드 사용됨: {cardData.cardId}");
            TutorialCombatHelper.OnCardUsed(cardData.cardId);
            HandManager.Instance.RemoveCardFromHand(gameObject, cardData);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private void HandleInventoryDrop(GameObject uiTarget)
    {
        if (uiTarget != null && uiTarget.TryGetComponent<DeckPresetUI>(out var presetSlot))
        {
            presetSlot.AddCard(cardData.cardId);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private void ReturnToOriginalPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}
