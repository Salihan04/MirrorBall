using UnityEngine;
using System.Collections;
using Leap;

public class BallController : MonoBehaviour {

    private GameObject ball;
    private GameObject ball_mirror;
    private Rigidbody rb_ball;
    private Rigidbody rb_ball_mirror;

    private Controller LEAPcontroller;

    public float speed;

    void Start()
    {
        ball = GameObject.Find("/Ball");
        ball_mirror = GameObject.Find("/Ball_Mirror");
        rb_ball = ball.GetComponent<Rigidbody>();
        rb_ball_mirror = ball_mirror.GetComponent<Rigidbody>();

        LEAPcontroller = new Controller();
        if (LEAPcontroller.IsConnected)
            Debug.Log("Leap connected!");
        else
            Debug.Log("Leap is NOT connected!");
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movement_mirror = new Vector3(moveHorizontal, 0.0f, -moveVertical);

        rb_ball.AddForce(movement * speed);
        rb_ball_mirror.AddForce(movement_mirror * speed);
    }
}
