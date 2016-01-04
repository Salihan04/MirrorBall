using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject ball;

	void LateUpdate () {
        transform.position = ball.transform.position;
	}
}
