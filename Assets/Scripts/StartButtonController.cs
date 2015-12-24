using UnityEngine;
using System.Collections;

public class StartButtonController : MonoBehaviour {

	public void Click()
    {
        Application.LoadLevel("Level1");
    }
}
