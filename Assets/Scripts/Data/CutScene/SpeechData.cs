using UnityEngine;

namespace CutsceneSystem
{
    [System.Serializable]
    public class SpeechData
    {
        public string speakerName;
        [TextArea(2, 5)]
        public string text;

        public Vector2 offset = Vector2.zero; // 말풍선 위치 조정
        public float delayBefore = 0.2f;      // 말풍선 등장 딜레이
    }
}