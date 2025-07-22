using UnityEngine;

public class T_HUDManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject optionPanel;

    public void OnClickGameStart()
    {
        SceneTransitionManager.Instance.LoadSceneWithFade("CutScene", GamePhase.Event);
    }

    public void OnClickContinue()
    {
        SceneTransitionManager.Instance.LoadSceneWithFade("AcademyScene", GamePhase.Management);
    }

    public void OnClickOption()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(true);
            AudioManager.Instance?.PlaySE("ui_confirm");
        }
        else
        {
            Debug.LogWarning("[T_HUDManager] Option Panel�� ������� �ʾҽ��ϴ�.");
        }
    }
    public void OnClickExit()
    {
        Application.Quit();
    }


}