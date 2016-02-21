using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlaytestControls : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            SceneManager.LoadScene("Level1");
        if (Input.GetKeyUp(KeyCode.Alpha2))
            SceneManager.LoadScene("Level2");
        if (Input.GetKeyUp(KeyCode.Alpha3))
            SceneManager.LoadScene("Level3");
        if (Input.GetKeyUp(KeyCode.Alpha4))
            SceneManager.LoadScene("Level4");
        if (Input.GetKeyUp(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
