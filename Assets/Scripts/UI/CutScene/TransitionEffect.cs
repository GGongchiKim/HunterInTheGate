using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class TransitionEffect : MonoBehaviour
    {
        public CanvasGroup canvasGroup;

        private void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public IEnumerator Play(PanelTransitionType type, float duration)
        {
            switch (type)
            {
                case PanelTransitionType.None: yield break;
                case PanelTransitionType.FadeIn: yield return FadeIn(duration); break;
                case PanelTransitionType.SlideIn: yield return SlideIn(duration); break;
                case PanelTransitionType.ZoomIn: yield return ZoomIn(duration); break;
                case PanelTransitionType.Flash: yield return Flash(duration); break;
                case PanelTransitionType.ZoomPan: yield return ZoomPan(duration); break;
                default:
                    Debug.LogWarning($"[TransitionEffect] 지원되지 않는 전환 연출 타입: {type}");
                    break;
            }
        }

        private IEnumerator FadeIn(float duration)
        {
            canvasGroup.alpha = 0;
            float t = 0f;
            while (t < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private IEnumerator SlideIn(float duration)
        {
            Vector3 start = transform.localPosition + new Vector3(0, 200, 0);
            Vector3 end = transform.localPosition;
            transform.localPosition = start;

            float t = 0f;
            while (t < duration)
            {
                transform.localPosition = Vector3.Lerp(start, end, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = end;
        }

        private IEnumerator ZoomIn(float duration)
        {
            transform.localScale = Vector3.zero;
            float t = 0f;
            while (t < duration)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }

        private IEnumerator Flash(float duration)
        {
            canvasGroup.alpha = 0f;
            yield return new WaitForSeconds(duration * 0.2f);
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(duration * 0.2f);
            canvasGroup.alpha = 0.4f;
            yield return new WaitForSeconds(duration * 0.2f);
            canvasGroup.alpha = 1f;
        }

        private IEnumerator ZoomPan(float duration)
        {
            RectTransform rt = GetComponent<RectTransform>();
            if (rt == null)
            {
                Debug.LogError("[TransitionEffect] RectTransform이 필요합니다.");
                yield break;
            }

            Vector3 originalScale = Vector3.one;
            Vector3 zoomedScale = Vector3.one * 2.0f;

            Vector2 originalPos = rt.anchoredPosition;
            Vector2 zoomedPos = originalPos + new Vector2(-rt.rect.width * 0.5f, rt.rect.height * 0.5f); // 좌상단

            float half = duration * 0.5f;
            float t = 0f;

            // 1단계: 확대 + 좌상단으로 이동
            rt.localScale = zoomedScale;
            rt.anchoredPosition = zoomedPos;

            // 2단계: 확대된 상태에서 우하단으로 패닝
            Vector2 panEndPos = originalPos + new Vector2(rt.rect.width * 0.5f, -rt.rect.height * 0.5f);
            while (t < half)
            {
                rt.anchoredPosition = Vector2.Lerp(zoomedPos, panEndPos, t / half);
                t += Time.deltaTime;
                yield return null;
            }

            // 3단계: 크기 복원 + 중앙 위치 복원
            t = 0f;
            while (t < half)
            {
                rt.localScale = Vector3.Lerp(zoomedScale, originalScale, t / half);
                rt.anchoredPosition = Vector2.Lerp(panEndPos, originalPos, t / half);
                t += Time.deltaTime;
                yield return null;
            }

            rt.localScale = originalScale;
            rt.anchoredPosition = originalPos;
        }
    }
}