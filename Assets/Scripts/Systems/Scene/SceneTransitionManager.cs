using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem; // DeckSaveData를 위해 필요

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (fadeImage != null)
            {
                Transform root = fadeImage.transform.root;
                if (root != transform)
                {
                    DontDestroyOnLoad(root.gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 페이드 애니메이션과 함께 씬 전환을 수행하고, GamePhase를 설정하고, 선택적 데이터 전달
    /// </summary>
    public void LoadSceneWithFade(string sceneName, GamePhase nextPhase = GamePhase.Event, string dataKey = null)
    {
        StartCoroutine(FadeAndLoad(sceneName, nextPhase, dataKey));
    }

    private IEnumerator FadeAndLoad(string sceneName, GamePhase nextPhase, string dataKey)
    {
        yield return StartCoroutine(Fade(1));

        GameStateManager.Instance.SetPhase(nextPhase);

        if (!string.IsNullOrEmpty(dataKey))
        {
            SceneDataBridge.Instance.SetData(DefaultDataKey, dataKey);
        }

        // 덱 데이터가 없다면 기본 덱을 지정
        if (SceneDataBridge.Instance.HasData("SelectedDeck"))
        {
            DeckSaveData selectedDeck = SceneDataBridge.Instance.GetData<DeckSaveData>("SelectedDeck");
            SceneDataBridge.Instance.SetData("SelectedDeck", selectedDeck); // 재전달
        }
        else
        {
            var defaultDeck = GameContext.Instance.inventory.GetDefaultDeck();
            if (defaultDeck != null)
            {
                Debug.LogWarning("[SceneTransitionManager] 선택된 덱이 없어 기본 덱을 전달합니다.");
                SceneDataBridge.Instance.SetData("SelectedDeck", defaultDeck);
            }
            else
            {
                Debug.LogError("[SceneTransitionManager] 기본 덱조차 존재하지 않습니다. inventory 확인 필요");
            }
        }

        SceneManager.LoadScene(sceneName);

        yield return null;
        yield return StartCoroutine(Fade(0));
    }
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

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}