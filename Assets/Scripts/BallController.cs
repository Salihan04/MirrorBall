using UnityEngine;
using System.Collections;
using Leap;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour {

    private GameObject ball;
    private GameObject ball_mirror;
    private Rigidbody rb_ball;
    private Rigidbody rb_ball_mirror;
    private Controller LEAPcontroller;
    private Hand hand;
    private Behaviour halo;
    private GameObject ballPointLight;
    private GameObject ballMirrorPointLight;

    private float pitch;
    private float yaw;
    private bool isGlowing;
    private float countdown;
    
    public float speed;

    void Start()
    {
        isGlowing = false;

        ball = GameObject.Find("/Ball");
        ball_mirror = GameObject.Find("/Ball_Mirror");
        rb_ball = ball.GetComponent<Rigidbody>();
        rb_ball_mirror = ball_mirror.GetComponent<Rigidbody>();

        ballPointLight = GameObject.Find("/Ball/Point light");
        ballMirrorPointLight = GameObject.Find("/Ball_Mirror/Point light");

        LEAPcontroller = new Controller();
        //Check if LEAPController is connected or not
        if (LEAPcontroller.IsConnected)
            Debug.Log("Leap connected!");
        else
            Debug.Log("Leap is NOT connected!");

        halo = (Behaviour)ball_mirror.GetComponent("Halo");
    }

    void FixedUpdate()
    {
        //Move ball and ball_mirror using keyboard input
        moveUsingKeyboard();

        //Initialise hand for current frame
        initHand();

        //Move ball and ball_mirror using Leap Motion
        moveUsingLeap();
    }

    void Update()
    {
        if (LEAPcontroller.Frame().Hands.Count == 0)
        {
            Debug.Log("No hands detected!");
            pitch = 0.0f;
            //Debug.Log("pitch: " + pitch);
            yaw = 0.0f;
            //Debug.Log("yaw: " + yaw);
        }
        else if (LEAPcontroller.Frame().Hands.Count > 0)
        {
            Debug.Log("Hand detected!");
            pitch = hand.Direction.Pitch;
            //Debug.Log("pitch: " + pitch);
            yaw = hand.Direction.Yaw;
            //Debug.Log("yaw: " + yaw);
        }

        glow();
        if(isGlowing)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0)
            {
                halo.enabled = false;
                ballPointLight.GetComponent<Light>().enabled = false;
                ballMirrorPointLight.GetComponent<Light>().enabled = false;
                isGlowing = false;
            }
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
        //Move ball forward and ball_mirror backward when pitch of hand is > 1.0f
        if (pitch > 1.0f)
        {
            rb_ball.AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed);
            rb_ball_mirror.AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed);
        }
        //Move ball backward and ball_mirror forward when pitch of hand is < -1.0f
        else if (pitch < -1.0f)
        {
            rb_ball.AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed);
            rb_ball_mirror.AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed);
        }
    }

    void moveLeftRightUsingLeap()
    {
        //Move both ball and ball_mirror right when yaw > 1.0f
        if(yaw > 1.0f)
        {
            rb_ball.AddForce(new Vector3(0.5f, 0.0f, 0.0f) * speed);
            rb_ball_mirror.AddForce(new Vector3(0.5f, 0.0f, 0.0f) * speed);
        }
        //Move both ball and ball_mirror left when yaw < -1.0f
        //Easier
        else if (yaw < -1.0f)
        {
            rb_ball.AddForce(new Vector3(-0.5f, 0.0f, 0.0f) * speed);
            rb_ball_mirror.AddForce(new Vector3(-0.5f, 0.0f, 0.0f) * speed);
        }
    }

    void moveUsingLeap()
    {
        //Move ball forward and ball_mirror backward and vice versa using Leap Motion
        moveUpDownUsingLeap();
        //Move ball and ball_mirror left or right using Leap Motion
        moveLeftRightUsingLeap();
    }

    void glow()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level3"))
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                halo.enabled = true;
                ballPointLight.GetComponent<Light>().enabled = true;
                ballMirrorPointLight.GetComponent<Light>().enabled = true;
                isGlowing = true;
                StartTimer(10.0f);
            }
        }
    }

    void StartTimer(float time)
    {
        countdown = time;
    }
}
