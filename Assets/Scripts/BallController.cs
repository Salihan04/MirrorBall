﻿using UnityEngine;
using Leap;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

    private GameObject ball;
    private GameObject ball_mirror;
    private GameObject ballPointLight;
    private GameObject ballMirrorPointLight;
    private Rigidbody rb_ball;
    private Rigidbody rb_ball_mirror;
    private Controller LEAPcontroller;
    private Hand hand;
    private Finger indexFinger;
    private Behaviour halo;
    private Renderer ballMirrorRenderer;
    private Text countdownText;
    private RawImage lightIcon;

    private float pitch;
    private float countdown;
    private bool isGlowing;

    public Material[] materials;
    public float speed;

    //Use this for initialization
    void Start()
    {
        isGlowing = false;

        //Initialise variables
        ball = GameObject.Find("/Ball");
        ball_mirror = GameObject.Find("/Ball_Mirror");
        rb_ball = ball.GetComponent<Rigidbody>();
        rb_ball_mirror = ball_mirror.GetComponent<Rigidbody>();

        ballPointLight = GameObject.Find("/Ball/Point light");
        ballMirrorPointLight = GameObject.Find("/Ball_Mirror/Point light");

        ballMirrorRenderer = ball_mirror.GetComponent<Renderer>();

        if(SceneManager.GetActiveScene().name.Equals("Level3") || SceneManager.GetActiveScene().name.Equals("Level4"))
        {
            GameObject canvas = GameObject.Find("Canvas");
            Text[] texts = canvas.GetComponentsInChildren<Text>();
            countdownText = texts[1];
            lightIcon = canvas.GetComponentInChildren<RawImage>();
        }

        LEAPcontroller = new Controller();
        //Check if LEAPController is connected or not
        if (LEAPcontroller.IsConnected)
            Debug.Log("Leap connected!");
        else
            Debug.Log("Leap is NOT connected!");

        halo = (Behaviour)ball_mirror.GetComponent("Halo");
    }

    //FixedUpdate is called every fixed framerate frame
    //Use this when Physics is involved
    void FixedUpdate()
    {
        //Move ball and ball_mirror using keyboard input
        moveUsingKeyboard();

        //Initialise hand for current frame
        initHand();

        //Move ball and ball_mirror using Leap Motion
        moveUsingLeap();
    }

    //Update is called once per frame
    void Update()
    {
        if (LEAPcontroller.Frame().Hands.Count == 0)
        {
            //If there are no hands, pitch = 0
            pitch = 0.0f;
        }
        else if (LEAPcontroller.Frame().Hands.Count > 0)
        {
            //Otherwise get the hand's pitch
            pitch = hand.Direction.Pitch;
        }

        //Simulate glowing ball in scene
        glow();
        if (isGlowing)
        {
            //Reduce the countdown value and update the countdown text
            countdown -= Time.deltaTime;
            countdownText.text = ((int)countdown).ToString();
            if (countdown <= 0)
            {
                //Countdown finish
                //Disable Ball_Mirror's Halo and Point light as well as Ball's Point light
                //Also change Ball_Mirror's material to Ball Color
                halo.enabled = false;
                ballPointLight.GetComponent<Light>().enabled = false;
                ballMirrorPointLight.GetComponent<Light>().enabled = false;
                ballMirrorRenderer.material = materials[0];
                isGlowing = false;

                //Reset countdown text
                countdownText.text = "10";

                //Reset alpha of light icon and countdown text to original
                Color color = countdownText.color;
                color.a = 100.0f / 255.0f;
                countdownText.color = color;

                color = lightIcon.color;
                color.a = 100.0f / 255.0f;
                lightIcon.color = color;
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

        //Get current frame
        currFrame = LEAPcontroller.Frame();   

        //Get Hand object from current frame
        hands = currFrame.Hands;
        hand = hands[0];

        //Get index finger of hand
        indexFinger = hand.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0];
    }

    void moveUpDownUsingLeap()
    {
        //Move ball forward and ball_mirror backward when pitch of hand is > 1.0f
        //and when the palm is facing away from the player
        if (pitch > 1.0f && hand.PalmNormal.z <= -0.7f)
        {
            rb_ball.AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed * 2.0f);
            rb_ball_mirror.AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed * 2.0f);
        }
        //Move ball backward and ball_mirror forward when pitch of hand is < -1.0f
        //and when the palm is facing the player
        else if (pitch < -1.0f && hand.PalmNormal.z >= 0.7f)
        {
            rb_ball.AddForce(new Vector3(0.0f, 0.0f, -0.5f) * speed * 2.0f);
            rb_ball_mirror.AddForce(new Vector3(0.0f, 0.0f, 0.5f) * speed * 2.0f);
        }
    }

    void moveLeftRightUsingLeap()
    {
        //First check that index finger is extended
        if (onlyIndexFingerExtended(hand.Fingers))
        {
            //Move ball right if index finger is pointing right and the palm is facing the player
            if (indexFinger.Direction.x >= 0.7 && hand.PalmNormal.z >= 0.7f)
            {
                pitch = 0.0f;

                rb_ball.AddForce(new Vector3(0.5f, 0.0f, 0.0f) * speed * 2.0f);
                rb_ball_mirror.AddForce(new Vector3(0.5f, 0.0f, 0.0f) * speed * 2.0f);
            }
            //Move ball left if index is pointing left and the palm is facing the player
            else if (indexFinger.Direction.x <= -0.7 && hand.PalmNormal.z >= 0.7f)
            {
                pitch = 0.0f;

                rb_ball.AddForce(new Vector3(-0.5f, 0.0f, 0.0f) * speed * 2.0f);
                rb_ball_mirror.AddForce(new Vector3(-0.5f, 0.0f, 0.0f) * speed * 2.0f);
            }
        }
    }

    void moveUsingLeap()
    {
        if (hand.GrabStrength == 0 && hand.SphereRadius > 35.0f)
        {
            //Move ball forward and ball_mirror backward and vice versa using Leap Motion
            moveUpDownUsingLeap();
            //Move ball and ball_mirror left or right using Leap Motion
            moveLeftRightUsingLeap();
        }
    }

    //Function to activate the ball's glow
    void glow()
    {
        //Only Level3 and Level4 have this feature
        if (SceneManager.GetActiveScene().name.Equals("Level3") || SceneManager.GetActiveScene().name.Equals("Level4"))
        {
            if ((Input.GetKeyUp(KeyCode.Space) || (hand.GrabStrength == 1 && hand.SphereRadius < 35.0f)) && !isGlowing)
            {
                //Max out alpha of countdown text and light icon
                Color color = countdownText.color;
                color.a = 1.0f;
                countdownText.color = color;

                color = lightIcon.color;
                color.a = 1.0f;
                lightIcon.color = color;

                //Enable Ball_Mirror's Halo and Point light as well as Ball's Point light
                //Also change Ball_Mirror's material to Glow
                //Start timer for 10 seconds
                halo.enabled = true;
                ballPointLight.GetComponent<Light>().enabled = true;
                ballMirrorPointLight.GetComponent<Light>().enabled = true;
                ballMirrorRenderer.material = materials[1];
                isGlowing = true;

                //Time is 11 seconds because I want the countdown to display from 10 down to 0
                StartTimer(11.0f);
            }
        }
    }

    void StartTimer(float time)
    {
        countdown = time;
    }

    //Function to check if only the index finger is extended and the other fingers are not
    bool onlyIndexFingerExtended(FingerList fingerList)
    {
        if (indexFinger.IsExtended &&
            !fingerList.FingerType(Finger.FingerType.TYPE_THUMB)[0].IsExtended &&
            !fingerList.FingerType(Finger.FingerType.TYPE_MIDDLE)[0].IsExtended &&
            !fingerList.FingerType(Finger.FingerType.TYPE_RING)[0].IsExtended &&
            !fingerList.FingerType(Finger.FingerType.TYPE_PINKY)[0].IsExtended)
            return true;

        return false;
    }
}