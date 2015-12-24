using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartButtonController : MonoBehaviour {

    public void Click()
    {
        SceneManager.LoadScene("Splash Screen");
    }
}
