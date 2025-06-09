using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            switch (type)
            {
                case PanelTransitionType.None:
                    yield break;

                case PanelTransitionType.FadeIn:
                    yield return FadeIn(duration);
                    break;

                case PanelTransitionType.SlideIn:
                    yield return SlideIn(duration);
                    break;

                case PanelTransitionType.ZoomIn:
                    yield return ZoomIn(duration);
                    break;

                case PanelTransitionType.Flash:
                    yield return FlashEffect(duration);
                    break;
            }
        }

        public IEnumerator PlaySpeechSequence(SpeechData[] speeches)
        {
            foreach (var speech in speeches)
            {
                yield return new WaitForSeconds(speech.delayBefore);

                if (speech.isNarration)
                {
                    // 중앙 나레이션 박스 출력
                    SpeechBubbleManager.Instance.ShowNarration(speech.text);
                }
                else
                {
                    // 말풍선 출력 (CutPanel의 이미지 하위에 로컬 좌표로 생성)
                    Transform parent = cutImage.transform;
                    Vector3 localPosition = speech.offset; // 로컬 기준으로 오프셋 적용
                    SpeechBubbleManager.Instance.ShowSpeechBubble(speech.speakerName, speech.text, parent, localPosition);
                }

                // 사용자 입력 대기
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                // 다음 대사를 위해 말풍선 또는 나레이션 제거
                SpeechBubbleManager.Instance.Hide();
            }
        }

        #region Transition Effects

        private IEnumerator FadeIn(float duration)
        {
            canvasGroup.alpha = 0f;
            float time = 0f;
            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private IEnumerator SlideIn(float duration)
        {
            Vector2 start = rectTransform.anchoredPosition + new Vector2(1000, 0);
            Vector2 end = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = start;

            float time = 0f;
            while (time < duration)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(start, end, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchoredPosition = end;
        }

        private IEnumerator ZoomIn(float duration)
        {
            transform.localScale = Vector3.zero;
            float time = 0f;
            while (time < duration)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }

        private IEnumerator FlashEffect(float duration)
        {
            canvasGroup.alpha = 0f;
            yield return new WaitForSeconds(duration * 0.2f);
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(duration * 0.2f);
            canvasGroup.alpha = 0.4f;
            yield return new WaitForSeconds(duration * 0.2f);
            canvasGroup.alpha = 1f;
        }

        #endregion
    }
}