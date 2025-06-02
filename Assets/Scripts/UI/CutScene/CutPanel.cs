using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CutsceneSystem
{
    public class CutPanel : MonoBehaviour
    {
        [Header("UI 참조")]
        public RectTransform maskRect;
        public Image cutImage;
        public Transform bubbleAnchor;
        public GameObject fxLayer;

        private RectTransform rectTransform;

        public void Setup(CutPanelData data)
        {
            cutImage.sprite = data.cutImage;
            rectTransform = GetComponent<RectTransform>();

            rectTransform.anchoredPosition = data.anchoredPosition;
            rectTransform.sizeDelta = data.sizeDelta;

            // 추가로 전환 방식에 따라 애니메이션 준비
        }

        public IEnumerator PlayTransition(PanelTransitionType type, float duration)
        {
            // 예: SlideIn, FadeIn 등
            yield return null;
        }
    }
}