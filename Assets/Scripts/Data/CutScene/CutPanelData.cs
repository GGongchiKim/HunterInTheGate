using UnityEngine;

namespace CutsceneSystem
{
    [CreateAssetMenu(fileName = "CutPanelData", menuName = "Cutscene/Cut Panel Data")]
    public class CutPanelData : ScriptableObject
    {
        [Header("�� �̹��� �� ��ġ")]
        public Sprite cutImage;

        [Tooltip("�г��� ��ġ ���� (Canvas ����)")]
        public Vector2 anchoredPosition = Vector2.zero;

        [Tooltip("�г� ũ�� ����")]
        public Vector2 sizeDelta = new Vector2(800, 600);

        [Tooltip("Z���� Ŭ���� ���� ��ġ�� (��Ƽ�� ���� �� ���)")]
        public int sortingOrder = 0;

        [Header("��ȯ ���� ����")]
        public PanelTransitionType transitionType = PanelTransitionType.SlideIn;

        [Tooltip("��ȯ �ִϸ��̼� ��� �ð�")]
        public float transitionDuration = 0.5f;

        [Tooltip("�� ���� �ð� (�ڵ� ���� �� ���)")]
        public float holdDuration = 2.0f;

        [Tooltip("��ǳ���� ���� �� �ڵ����� ���� ������ �������� ����")]
        public bool autoProceed = false;

        [Header("��� ����")]
        public SpeechData[] speeches;

        [Header("����Ʈ �� ����")]
        [Tooltip("�� �� ��� �� �߰��� ����Ʈ ������")]
        public GameObject fxPrefab;

        [Tooltip("����Ʈ ��ġ ������ (�� �߽� ����)")]
        public Vector2 fxOffset = Vector2.zero;

        [Tooltip("����Ʈ ���� ������")]
        public float fxDelay = 0f;

        [Header("�� ���� �� ������ (����)")]
        [Tooltip("�� ���� ���� �� ���� �� ������ ��� �ð�")]
        public float postDelay = 0f;
    }
}