using UnityEngine;
using UnityEngine.SceneManagement;

public class T_HUDManager : MonoBehaviour
{
    public void OnClickContinue()
    {
        SceneManager.LoadScene("AcademyScene");
    }
}