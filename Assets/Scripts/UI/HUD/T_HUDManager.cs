using UnityEngine;

public class T_HUDManager : MonoBehaviour
{
    public void OnClickContinue()
    {
        // Ʃ�丮�� ���� �� ��ī���� ������ ��ȯ
        SceneTransitionManager.Instance.LoadSceneWithFade("AcademyScene", GamePhase.Management);
    }
}