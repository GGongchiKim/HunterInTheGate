using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Transition UI")]
    [SerializeField] private Image fadeImage;  // 캔버스에 투명 검은 이미지 배치

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
    /// 페이드 애니메이션과 함께 씬 전환을 수행하고, GamePhase를 설정하고, 선택적 데이터 전달
    /// </summary>
    /// <param name="sceneName">전환할 씬 이름</param>
    /// <param name="nextPhase">전환 후 게임 페이즈</param>
    /// <param name="dataKey">SceneDataBridge에 저장할 EventId 값 (null이면 저장 안함)</param>
    public void LoadSceneWithFade(string sceneName, GamePhase nextPhase = GamePhase.Event, string dataKey = null)
    {
        StartCoroutine(FadeAndLoad(sceneName, nextPhase, dataKey));
    }

    private IEnumerator FadeAndLoad(string sceneName, GamePhase nextPhase, string dataKey)
    {
        // 페이드 아웃
        yield return StartCoroutine(Fade(1));

        // 게임 페이즈 갱신
        GameStateManager.Instance.SetPhase(nextPhase);

        // 데이터 전달
        if (!string.IsNullOrEmpty(dataKey))
        {
            SceneDataBridge.Instance.SetData(DefaultDataKey, dataKey);
        }

        // 씬 전환
        SceneManager.LoadScene(sceneName);

        // 다음 프레임까지 대기 후 페이드 인
        yield return null;
        yield return StartCoroutine(Fade(0));
    }

    /// <summary>
    /// 알파값을 변경하여 페이드 인/아웃 처리
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

        // 정확한 타겟 알파 설정
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}