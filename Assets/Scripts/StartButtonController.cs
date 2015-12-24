using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour {

	public void Click()
    {
        SceneManager.LoadScene("Level1");
    }
}
