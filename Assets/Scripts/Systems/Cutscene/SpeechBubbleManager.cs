using UnityEngine;

namespace CutsceneSystem
{
    public class SpeechBubbleManager : MonoBehaviour
    {
        public static SpeechBubbleManager Instance { get; private set; }

        [Header("������")]
        public GameObject speechBubblePrefab;
        public GameObject narrationBoxPrefab;

        [Header("UI �θ� (�⺻��)")]
        public Transform uiRoot; // �⺻ �θ� (Canvas ��)

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
        /// ��ǳ�� ��� (�θ� Transform ���� ����)
        /// </summary>
        public void ShowSpeechBubble(string speaker, string text, Transform parent, Vector3 localPosition)
        {
            Hide(); // ���� ����

            currentBubble = Instantiate(speechBubblePrefab, parent);
            var ui = currentBubble.GetComponent<SpeechBubbleUI>();
            ui.Setup(speaker, text, localPosition);
        }

        /// <summary>
        /// �����̼� �ڽ� ��� (�⺻ UI ��Ʈ�� ���)
        /// </summary>
        public void ShowNarration(string text)
        {
            Hide();

            currentBubble = Instantiate(narrationBoxPrefab, uiRoot);
            var ui = currentBubble.GetComponent<NarrationBoxUI>();
            ui.Setup(text);
        }

        /// <summary>
        /// ���� ��ǳ��/�����̼� ����
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