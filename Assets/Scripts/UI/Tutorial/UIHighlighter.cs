using System.Collections;
using UnityEngine;

/// <summary>
/// UI 강조 효과를 위한 깜빡임 외곽선 컨트롤러.
/// 강조 대상 오브젝트의 자식에 붙이며, 깜빡임 효과를 제어한다.
/// </summary>
public class UIHighlighter : MonoBehaviour
{
    [Header("강조 외곽선 오브젝트")]
    [SerializeField] private GameObject highlightBorder;

    [Header("깜빡임 간격 (초)")]
    [SerializeField] private float blinkInterval = 0.5f;

    [Header("시작 시 자동 강조 여부")]
    [SerializeField] private bool startBlinkOnAwake = false;

    private Coroutine blinkRoutine;

    private void Awake()
    {
        if (highlightBorder != null)
            highlightBorder.SetActive(false);
    }

    private void Start()
    {
        if (startBlinkOnAwake)
            StartBlinking();
    }

    /// <summary>
    /// 외곽선 깜빡임 시작
    /// </summary>
    public void StartBlinking()
    {
        if (highlightBorder == null)
        {
            Debug.LogWarning($"[{name}] highlightBorder가 지정되지 않았습니다.");
            return;
        }

        highlightBorder.SetActive(true);

        if (blinkRoutine == null)
            blinkRoutine = StartCoroutine(Blink());
    }

    /// <summary>
    /// 외곽선 깜빡임 중단 및 숨김
    /// </summary>
    public void StopBlinking()
    {
        if (highlightBorder != null)
            highlightBorder.SetActive(false);

        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }
    }

    /// <summary>
    /// 깜빡임 상태 확인
    /// </summary>
    public bool IsBlinking() => blinkRoutine != null;

    private IEnumerator Blink()
    {
        while (true)
        {
            highlightBorder.SetActive(!highlightBorder.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void OnDisable()
    {
        // 오브젝트 풀링 대비 안전하게 리셋
        StopBlinking();
    }
}
