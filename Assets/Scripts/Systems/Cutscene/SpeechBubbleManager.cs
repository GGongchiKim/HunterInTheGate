using UnityEngine;

namespace CutsceneSystem
{
    public class SpeechBubbleManager : MonoBehaviour
    {
        public static SpeechBubbleManager Instance { get; private set; }

        [Header("프리팹")]
        public GameObject speechBubblePrefab;
        public GameObject narrationBoxPrefab;

        [Header("UI 부모 (기본값)")]
        public Transform uiRoot; // 기본 부모 (Canvas 등)

        private GameObject currentBubble;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// 말풍선 출력 (부모 Transform 지정 가능)
        /// </summary>
        public void ShowSpeechBubble(string speaker, string text, Transform parent, Vector3 localPosition)
        {
            Hide(); // 기존 제거

            currentBubble = Instantiate(speechBubblePrefab, parent);
            var ui = currentBubble.GetComponent<SpeechBubbleUI>();
            ui.Setup(speaker, text, localPosition);
        }

        /// <summary>
        /// 나레이션 박스 출력 (기본 UI 루트에 출력)
        /// </summary>
        public void ShowNarration(string text)
        {
            Hide();

            currentBubble = Instantiate(narrationBoxPrefab, uiRoot);
            var ui = currentBubble.GetComponent<NarrationBoxUI>();
            ui.Setup(text);
        }

        /// <summary>
        /// 현재 말풍선/나레이션 제거
        /// </summary>
        public void Hide()
        {
            if (currentBubble != null)
            {
                Destroy(currentBubble);
                currentBubble = null;
            }
        }
    }
}