using UnityEngine;

namespace CutsceneSystem
{
    [System.Serializable]
    public class SpeechData
    {
        public string speakerName;
        [TextArea(2, 5)]
        public string text;

        public Vector2 offset = Vector2.zero; // ��ǳ�� ��ġ ����
        public float delayBefore = 0.2f;      // ��ǳ�� ���� ������
    }
}