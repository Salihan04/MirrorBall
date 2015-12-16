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
        //Check if LEAPController is connected or not
        if (LEAPcontroller.IsConnected)
            Debug.Log("Leap connected!");
        else
            Debug.Log("Leap is NOT connected!");
    }

    void FixedUpdate()
    {
        //Move ball and ball_mirror using keyboard input
        moveUsingKeyboard();

        //Initialise hand for current frame
        initHand();

        //Move ball and ball_mirror using Leap Motion
        moveUsingLeap();

        //float pitch = hand.Direction.Pitch;
        //Debug.Log("pitch: " + pitch);
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

    void moveUsingKeyboard()
    {
        float moveHorizontal;
        float moveVertical;

        Vector3 movement;
        Vector3 movement_mirror;

        //Get horizontal and vertical input data
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //Compute movement based on input data
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement_mirror = new Vector3(moveHorizontal, 0.0f, -moveVertical);

        //Add force to rigidbody of ball and ball_mirror to move them
        rb_ball.AddForce(movement * speed);
        rb_ball_mirror.AddForce(movement_mirror * speed);
    }

    void initHand()
    {
        Frame currFrame;
        HandList hands;

        currFrame = LEAPcontroller.Frame();   //get current frame
        hands = currFrame.Hands;
        hand = hands[0];
    }

    void moveUpDownUsingLeap()
    {
        //Move ball forward and ball_mirror backward when yaw of hand is > 0.3f
        if (yaw > 0.3f)
        {
            rb_ball.AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed);
            rb_ball_mirror.AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed);
        }
        //Move ball backward and ball_mirror forward when yaw of hand is < -0.3f
        else if (yaw < -0.3f)
        {
            rb_ball.AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed);
            rb_ball_mirror.AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed);
        }
    }

    void moveUsingLeap()
    {
        //Move ball forward and ball_mirror backward using Leap Motion
        moveUpDownUsingLeap();
    }
}
