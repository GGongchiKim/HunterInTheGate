using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Transition UI")]
    [SerializeField] private Image fadeImage;  // ĵ������ ���� ���� �̹��� ��ġ

    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private const string DefaultDataKey = "EventId";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���̵� �ִϸ��̼ǰ� �Բ� �� ��ȯ�� �����ϰ�, GamePhase�� �����ϰ�, ������ ������ ����
    /// </summary>
    /// <param name="sceneName">��ȯ�� �� �̸�</param>
    /// <param name="nextPhase">��ȯ �� ���� ������</param>
    /// <param name="dataKey">SceneDataBridge�� ������ EventId �� (null�̸� ���� ����)</param>
    public void LoadSceneWithFade(string sceneName, GamePhase nextPhase = GamePhase.Event, string dataKey = null)
    {
        StartCoroutine(FadeAndLoad(sceneName, nextPhase, dataKey));
    }

    private IEnumerator FadeAndLoad(string sceneName, GamePhase nextPhase, string dataKey)
    {
        // ���̵� �ƿ�
        yield return StartCoroutine(Fade(1));

        // ���� ������ ����
        GameStateManager.Instance.SetPhase(nextPhase);

        // ������ ����
        if (!string.IsNullOrEmpty(dataKey))
        {
            SceneDataBridge.Instance.SetData(DefaultDataKey, dataKey);
        }

        // �� ��ȯ
        SceneManager.LoadScene(sceneName);

        // ���� �����ӱ��� ��� �� ���̵� ��
        yield return null;
        yield return StartCoroutine(Fade(0));
    }

    /// <summary>
    /// ���İ��� �����Ͽ� ���̵� ��/�ƿ� ó��
    /// </summary>
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // ��Ȯ�� Ÿ�� ���� ����
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}