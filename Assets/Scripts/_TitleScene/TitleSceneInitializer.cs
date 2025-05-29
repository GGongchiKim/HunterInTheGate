using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetPhase(GamePhase.Event);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
