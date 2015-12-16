using UnityEngine;
using System.Collections;
using Leap;

public class BallController : MonoBehaviour {

    private GameObject ball;
    private GameObject ball_mirror;
    private Rigidbody rb_ball;
    private Rigidbody rb_ball_mirror;
    private Controller LEAPcontroller;
    private Hand hand;

    private float yaw;

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
        float moveHorizontal;
        float moveVertical;

        Vector3 movement;
        Vector3 movement_mirror;
        Frame currFrame;
        Frame prevFrame;
        HandList hands;

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement_mirror = new Vector3(moveHorizontal, 0.0f, -moveVertical);

        rb_ball.AddForce(movement * speed);
        rb_ball_mirror.AddForce(movement_mirror * speed);

        currFrame = LEAPcontroller.Frame();   //get current frame
        prevFrame = LEAPcontroller.Frame(1);  //get previous frame
        hands = currFrame.Hands;
        hand = hands[0];

        //float pitch = hand.Direction.Pitch;
        //Debug.Log("pitch: " + pitch);

        if (yaw > 0.3f)
        {
            ball.GetComponent<Rigidbody>().WakeUp();
            ball.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed);
        }
        else if (yaw < -0.3f)
        {
            ball.GetComponent<Rigidbody>().WakeUp();
            ball.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed);
        }

        //float rotationAroundXAxis = hand.RotationAngle(prevFrame, Vector.XAxis);
        //Debug.Log("rotation x-axis: " + rotationAroundXAxis);
        //if (rotationAroundXAxis > 0)
        //    ball.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, 0.1f) * speed);
        //else
        //    ball.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, -0.1f) * speed);
    }

    void Update()
    {
        if (LEAPcontroller.Frame().Hands.Count == 0)
        {
            Debug.Log("No hands detected!");
            yaw = 0.0f;
            Debug.Log("yaw: " + yaw);
        }
        else if (LEAPcontroller.Frame().Hands.Count > 0)
        {
            Debug.Log("Hand detected!");
            yaw = hand.Direction.Yaw;
            Debug.Log("yaw: " + yaw);
        }
    }
}
