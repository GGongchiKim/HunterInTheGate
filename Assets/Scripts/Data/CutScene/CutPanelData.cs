using UnityEngine;

namespace CutsceneSystem
{
    [CreateAssetMenu(fileName = "CutPanelData", menuName = "Cutscene/Cut Panel Data")]
    public class CutPanelData : ScriptableObject
    {
        [Header("컷 이미지 및 배치")]
        public Sprite cutImage;

        [Tooltip("패널의 위치 지정 (Canvas 기준)")]
        public Vector2 anchoredPosition = Vector2.zero;

        [Tooltip("패널 크기 설정")]
        public Vector2 sizeDelta = new Vector2(800, 600);

        [Tooltip("Z값이 클수록 위에 배치됨 (멀티컷 연출 시 사용)")]
        public int sortingOrder = 0;

        [Header("전환 연출 설정")]
        public PanelTransitionType transitionType = PanelTransitionType.SlideIn;

        [Tooltip("전환 애니메이션 재생 시간")]
        public float transitionDuration = 0.5f;

        [Tooltip("컷 유지 시간 (자동 진행 시 사용)")]
        public float holdDuration = 2.0f;

        [Tooltip("말풍선이 끝난 후 자동으로 다음 컷으로 진행할지 여부")]
        public bool autoProceed = false;

        [Header("대사 설정")]
        public SpeechData[] speeches;

        [Header("이펙트 및 연출")]
        [Tooltip("이 컷 재생 시 추가할 이펙트 프리팹")]
        public GameObject fxPrefab;

        [Tooltip("이펙트 위치 오프셋 (컷 중심 기준)")]
        public Vector2 fxOffset = Vector2.zero;

        [Tooltip("이펙트 등장 딜레이")]
        public float fxDelay = 0f;

        [Header("컷 종료 후 딜레이 (선택)")]
        [Tooltip("이 컷이 끝난 후 다음 컷 전까지 대기 시간")]
        public float postDelay = 0f;
    }
}