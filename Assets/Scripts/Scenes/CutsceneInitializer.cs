using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutsceneSystem
{
    public class CutsceneInitializer : MonoBehaviour
    {
        [Header("�ƾ� ��Ʈ�ѷ�")]
        public CutsceneController cutsceneController;

        [Header("�ƾ� ���� �� �̵��� �� �̸�")]
        public string nextSceneName = "TitleScene";

        private void Start()
        {
            if (cutsceneController != null)
            {
                cutsceneController.OnCutsceneComplete += HandleCutsceneComplete;
            }
            else
            {
                Debug.LogWarning("CutsceneController�� ������� �ʾҽ��ϴ�.");
            }
        }

        private void HandleCutsceneComplete()
        {
            // ���߿� ���̵�ƿ� ���� �� ���� ����
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("���� �� �̸��� �������� �ʾҽ��ϴ�.");
            }
        }

        private void OnDestroy()
        {
            if (cutsceneController != null)
            {
                cutsceneController.OnCutsceneComplete -= HandleCutsceneComplete;
            }
        }
    }
}