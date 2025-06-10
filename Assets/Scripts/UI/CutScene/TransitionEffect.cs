using System.Collections;
using UnityEngine;

public enum TransitionType { None, FadeIn, SlideIn, ZoomIn, Flash }

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

    public IEnumerator Play(TransitionType type, float duration)
    {
        switch (type)
        {
            case TransitionType.None: yield break;
            case TransitionType.FadeIn: yield return FadeIn(duration); break;
            case TransitionType.SlideIn: yield return SlideIn(duration); break;
            case TransitionType.ZoomIn: yield return ZoomIn(duration); break;
            case TransitionType.Flash: yield return Flash(duration); break;
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
        Vector3 start = transform.localPosition + new Vector3(1000, 0, 0);
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
}