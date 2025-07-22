using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem; // DeckSaveData�� ���� �ʿ�

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
    /// ���̵� �ִϸ��̼ǰ� �Բ� �� ��ȯ�� �����ϰ�, GamePhase�� �����ϰ�, ������ ������ ����
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

        // �� �����Ͱ� ���ٸ� �⺻ ���� ����
        if (SceneDataBridge.Instance.HasData("SelectedDeck"))
        {
            DeckSaveData selectedDeck = SceneDataBridge.Instance.GetData<DeckSaveData>("SelectedDeck");
            SceneDataBridge.Instance.SetData("SelectedDeck", selectedDeck); // ������
        }
        else
        {
            var defaultDeck = GameContext.Instance.inventory.GetDefaultDeck();
            if (defaultDeck != null)
            {
                Debug.LogWarning("[SceneTransitionManager] ���õ� ���� ���� �⺻ ���� �����մϴ�.");
                SceneDataBridge.Instance.SetData("SelectedDeck", defaultDeck);
            }
            else
            {
                Debug.LogError("[SceneTransitionManager] �⺻ ������ �������� �ʽ��ϴ�. inventory Ȯ�� �ʿ�");
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