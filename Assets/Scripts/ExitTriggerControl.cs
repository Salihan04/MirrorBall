using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitTriggerControl : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Level1":
                SceneManager.LoadScene("Level2");
                break;
            case "Level2":
                SceneManager.LoadScene("Level3");
                break;
            default:
                SceneManager.LoadScene("Credit Screen");
                break;
        }
    }
}
