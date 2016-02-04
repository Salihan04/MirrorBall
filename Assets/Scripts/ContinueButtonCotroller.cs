using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ContinueButtonCotroller : MonoBehaviour {

	public void Click()
    {
        SceneManager.LoadScene("Level1");
    }
}
