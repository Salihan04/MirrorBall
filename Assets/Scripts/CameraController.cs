using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject ball;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = ball.transform.position;
	}
}
