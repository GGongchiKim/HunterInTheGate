using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanelController : MonoBehaviour
{

    public void OnClickTitle()
    {
        SceneTransitionManager.Instance.LoadSceneWithFade("TitleScene", GamePhase.Event);
    }

    public void OnClickExit() 
    {
        Application.Quit();
    }


}
