using System.Collections;
using UnityEngine;

/// <summary>
/// UI ���� ȿ���� ���� ������ �ܰ��� ��Ʈ�ѷ�.
/// ���� ��� ������Ʈ�� �ڽĿ� ���̸�, ������ ȿ���� �����Ѵ�.
/// </summary>
public class UIHighlighter : MonoBehaviour
{
    [Header("���� �ܰ��� ������Ʈ")]
    [SerializeField] private GameObject highlightBorder;

    [Header("������ ���� (��)")]
    [SerializeField] private float blinkInterval = 0.5f;

    [Header("���� �� �ڵ� ���� ����")]
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
    /// �ܰ��� ������ ����
    /// </summary>
    public void StartBlinking()
    {
        if (highlightBorder == null)
        {
            Debug.LogWarning($"[{name}] highlightBorder�� �������� �ʾҽ��ϴ�.");
            return;
        }

        highlightBorder.SetActive(true);

        if (blinkRoutine == null)
            blinkRoutine = StartCoroutine(Blink());
    }

    /// <summary>
    /// �ܰ��� ������ �ߴ� �� ����
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
    /// ������ ���� Ȯ��
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
        // ������Ʈ Ǯ�� ��� �����ϰ� ����
        StopBlinking();
    }
}
