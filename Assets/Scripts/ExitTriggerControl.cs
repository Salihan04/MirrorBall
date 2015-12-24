using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitTriggerControl : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        SceneManager.LoadScene("Credit Screen");
    }
}
