using UnityEngine;

public class T_HUDManager : MonoBehaviour
{
    public void OnClickGameStart()
    {
        // Ʃ�丮�� ���� �� ��ī���� ������ ��ȯ
        SceneTransitionManager.Instance.LoadSceneWithFade("CutScene", GamePhase.Event);
    }


    public void OnClickContinue()
    {
        // Ʃ�丮�� ���� �� ��ī���� ������ ��ȯ
        SceneTransitionManager.Instance.LoadSceneWithFade("AcademyScene", GamePhase.Management);
    }
}