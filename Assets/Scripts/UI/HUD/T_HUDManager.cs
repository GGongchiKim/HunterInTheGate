using UnityEngine;

public class T_HUDManager : MonoBehaviour
{
    public void OnClickContinue()
    {
        // 튜토리얼 종료 후 아카데미 씬으로 전환
        SceneTransitionManager.Instance.LoadSceneWithFade("AcademyScene", GamePhase.Management);
    }
}