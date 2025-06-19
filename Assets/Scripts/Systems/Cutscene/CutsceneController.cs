using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CutsceneSystem
{
    public class CutsceneController : MonoBehaviour
    {
        [Header("컷씬 데이터")]
        public List<CutPanelData> cutPanels;

        [Header("컷 프리팹 및 부모")]
        public GameObject cutPanelPrefab;
        public Transform panelParent;

        [Header("설정")]
        public bool autoModeOverride = true;

        [Header("컷씬 종료 후 전환")]
        [SerializeField] private string nextDialogueId; // 다음 대화 이벤트 ID

        private GameObject currentPanel;

        /// <summary>
        /// 컷씬이 끝났을 때 호출되는 외부용 이벤트
        /// </summary>
        public event Action OnCutsceneComplete;

        private void Start()
        {
            StartCoroutine(PlayCutscene());
        }

        public IEnumerator PlayCutscene()
        {
            foreach (var panelData in cutPanels)
            {
                yield return PlayPanel(panelData);
            }

            OnCutsceneEnd();
        }

        private IEnumerator PlayPanel(CutPanelData data)
        {
            // Instantiate 컷 프리팹
            currentPanel = Instantiate(cutPanelPrefab, panelParent);
            var cutPanel = currentPanel.GetComponent<CutPanel>();
            cutPanel.Setup(data); // 이미지, 위치, 크기 등 설정

            // 전환 연출
            yield return cutPanel.PlayTransition(data.transitionType, data.transitionDuration);

            // 말풍선 출력
            if (data.speeches != null && data.speeches.Length > 0)
            {
                yield return cutPanel.PlaySpeechSequence(data.speeches);
            }

            // 이펙트 출력
            if (data.fxPrefab != null)
            {
                yield return new WaitForSeconds(data.fxDelay);
                Instantiate(
                    data.fxPrefab,
                    currentPanel.transform.position + (Vector3)data.fxOffset,
                    Quaternion.identity,
                    currentPanel.transform
                );
            }

            // 진행 방식
            if (autoModeOverride && data.autoProceed)
            {
                yield return new WaitForSeconds(data.holdDuration + data.postDelay);
            }
            else
            {
                yield return WaitForClick();
            }

            // 정리
            Destroy(currentPanel);
        }

        private IEnumerator WaitForClick()
        {
            while (!Input.GetMouseButtonDown(0))
                yield return null;
        }

        private void OnCutsceneEnd()
        {
            Debug.Log("Cutscene finished.");

            // 이벤트 호출
            OnCutsceneComplete?.Invoke();

            // 컷씬 종료 후 대화 이벤트로 전환
            if (!string.IsNullOrEmpty(nextDialogueId))
            {
                SceneTransitionManager.Instance.LoadSceneWithFade("DialogueScene", GamePhase.Event, nextDialogueId);
            }
        }
    }
}