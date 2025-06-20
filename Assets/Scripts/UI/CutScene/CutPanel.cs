using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void Setup(CutPanelData data)
        {
            cutImage.sprite = data.cutImage;
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = data.anchoredPosition;
            rectTransform.sizeDelta = data.sizeDelta;
            rectTransform.SetSiblingIndex(data.sortingOrder);
        }

        public IEnumerator PlayTransition(PanelTransitionType type, float duration)
        {
            var effect = GetComponent<TransitionEffect>();
            if (effect != null)
                yield return effect.Play(type, duration); //ĳ���� ����
        }

        public IEnumerator PlaySpeechSequence(SpeechData[] speeches)
        {
            foreach (var speech in speeches)
            {
                yield return new WaitForSeconds(speech.delayBefore);

                if (speech.isNarration)
                {
                    // �߾� �����̼� �ڽ� ���
                    SpeechBubbleManager.Instance.ShowNarration(speech.text);
                }
                else
                {
                    // ��ǳ�� ���
                    Transform parent = bubbleAnchor != null ? bubbleAnchor : cutImage.transform;
                    Vector3 localPosition = speech.offset;
                    SpeechBubbleManager.Instance.ShowSpeechBubble(speech.speakerName, speech.text, parent, localPosition);
                }

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                SpeechBubbleManager.Instance.Hide();
            }
        }
    }
}