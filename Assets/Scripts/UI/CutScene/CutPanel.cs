using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CutsceneSystem
{
    public class CutPanel : MonoBehaviour
    {
        [Header("UI ����")]
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

            // �߰��� ��ȯ ��Ŀ� ���� �ִϸ��̼� �غ�
        }

        public IEnumerator PlayTransition(PanelTransitionType type, float duration)
        {
            // ��: SlideIn, FadeIn ��
            yield return null;
        }
    }
}