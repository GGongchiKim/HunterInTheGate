using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CutsceneSystem
{
    public class CutsceneController : MonoBehaviour
    {
        [Header("�ƾ� ������")]
        public List<CutPanelData> cutPanels;

        [Header("�� ������ �� �θ�")]
        public GameObject cutPanelPrefab;
        public Transform panelParent;

        [Header("����")]
        public bool autoModeOverride = true;

        [Header("�ƾ� ���� �� ��ȯ")]
        [SerializeField] private string nextDialogueId; // ���� ��ȭ �̺�Ʈ ID

        private GameObject currentPanel;

        /// <summary>
        /// �ƾ��� ������ �� ȣ��Ǵ� �ܺο� �̺�Ʈ
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
            // Instantiate �� ������
            currentPanel = Instantiate(cutPanelPrefab, panelParent);
            var cutPanel = currentPanel.GetComponent<CutPanel>();
            cutPanel.Setup(data); // �̹���, ��ġ, ũ�� �� ����

            // ��ȯ ����
            yield return cutPanel.PlayTransition(data.transitionType, data.transitionDuration);

            // ��ǳ�� ���
            if (data.speeches != null && data.speeches.Length > 0)
            {
                yield return cutPanel.PlaySpeechSequence(data.speeches);
            }

            // ����Ʈ ���
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

            // ���� ���
            if (autoModeOverride && data.autoProceed)
            {
                yield return new WaitForSeconds(data.holdDuration + data.postDelay);
            }
            else
            {
                yield return WaitForClick();
            }

            // ����
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

            // �̺�Ʈ ȣ��
            OnCutsceneComplete?.Invoke();

            // �ƾ� ���� �� ��ȭ �̺�Ʈ�� ��ȯ
            if (!string.IsNullOrEmpty(nextDialogueId))
            {
                SceneTransitionManager.Instance.LoadSceneWithFade("DialogueScene", GamePhase.Event, nextDialogueId);
            }
        }
    }
}