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
        [Header("컷 이미지 및 위치 설정")]
        public Sprite cutImage;
        public Vector2 anchoredPosition = Vector2.zero;
        public Vector2 sizeDelta = new Vector2(800, 600); // 기본 크기

        [Header("컷 전환 방식")]
        public PanelTransitionType transitionType = PanelTransitionType.SlideIn;
        public float transitionDuration = 0.5f;

        [Header("컷 유지 시간")]
        public float holdDuration = 2.0f; // 자동진행일 경우 유지 시간

        [Header("대사 리스트")]
        public SpeechData[] speeches;

        [Header("이 컷 다음 자동 진행 여부")]
        public bool autoProceed = false;

        [Header("이펙트 / 추가 연출 (선택)")]
        public GameObject fxPrefab; // 예: 액션선, 텍스트 이펙트 등
    }
}