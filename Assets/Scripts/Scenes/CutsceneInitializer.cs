using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutsceneSystem
{
    public class CutsceneInitializer : MonoBehaviour
    {
        [Header("컷씬 컨트롤러")]
        public CutsceneController cutsceneController;

        [Header("컷씬 종료 후 이동할 씬 이름")]
        public string nextSceneName = "TitleScene";

        private void Start()
        {
            if (cutsceneController != null)
            {
                cutsceneController.OnCutsceneComplete += HandleCutsceneComplete;
            }
            else
            {
                Debug.LogWarning("CutsceneController가 연결되지 않았습니다.");
            }
        }

        private void HandleCutsceneComplete()
        {
            // 나중에 페이드아웃 연출 등 삽입 가능
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
                Debug.LogWarning("다음 씬 이름이 설정되지 않았습니다.");
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