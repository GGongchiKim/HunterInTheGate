using UnityEngine;

namespace CutsceneSystem
{
    public enum PanelTransitionType
    {
        None,
        SlideIn,
        FadeIn,
        ZoomIn,
        SplitAdd
    }

    [CreateAssetMenu(fileName = "CutPanelData", menuName = "Cutscene/Cut Panel Data")]
    public class CutPanelData : ScriptableObject
    {
        [Header("�� �̹��� �� ��ġ ����")]
        public Sprite cutImage;
        public Vector2 anchoredPosition = Vector2.zero;
        public Vector2 sizeDelta = new Vector2(800, 600); // �⺻ ũ��

        [Header("�� ��ȯ ���")]
        public PanelTransitionType transitionType = PanelTransitionType.SlideIn;
        public float transitionDuration = 0.5f;

        [Header("�� ���� �ð�")]
        public float holdDuration = 2.0f; // �ڵ������� ��� ���� �ð�

        [Header("��� ����Ʈ")]
        public SpeechData[] speeches;

        [Header("�� �� ���� �ڵ� ���� ����")]
        public bool autoProceed = false;

        [Header("����Ʈ / �߰� ���� (����)")]
        public GameObject fxPrefab; // ��: �׼Ǽ�, �ؽ�Ʈ ����Ʈ ��
    }
}