using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //player characteristics
    public float walkSpeed = 4f;
    public float jumpForce = 1f;

    public AudioSource coinSound;

    //player parameters
    Rigidbody rb;
    Collider cl;
    Vector3 playerSize;

    //camera settings
    public float cameraDistanceZ = 5f;

    //flag to keep track of key pressing
    bool pressedJump = false;

    // Use this for initialization
    void Start () {
        //player parameters initiating
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();

        //get player object size for walking/jumping
        playerSize = cl.bounds.size;

        //set the camera to player position
        CameraFollowPlayer();
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        WalkHandler();
        JumpHandler();
        CameraFollowPlayer();

    }

    void WalkHandler() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis * walkSpeed * Time.deltaTime, 0, vAxis * walkSpeed * Time.deltaTime);
        rb.MovePosition(transform.position + movement);

        //setting moving direction to model
        if (hAxis != 0 || vAxis != 0)
        {
            //gain direction
            Vector3 direction = new Vector3(hAxis, 0, vAxis);

            //can apply direction to transform -> if using regidbody for kinematic objects
            //transform.forward = direction;

            //can apply direct to rigidbody component in object
            rb.rotation = Quaternion.LookRotation(direction);
        }
    }

    void JumpHandler() {
        float jAxis = Input.GetAxis("Jump");
 
        if (jAxis > 0)
        {
            bool isGrounded = CheckGrounded();
            //if we alredy jumping?
            if (!pressedJump && isGrounded)
            {
                //print("! ! ! ! GROUNDED ! ! ! !");
                pressedJump = true;
                Vector3 jumpVector = new Vector3(0, jAxis * jumpForce, 0);
                rb.AddForce(jumpVector, ForceMode.VelocityChange);
            }
        }
        else
        {
            pressedJump = false;
        }
    }

    bool CheckGrounded()
    {
        //print("Grounded?");
        //found 4 corners
        Vector3 corner1 = transform.position + new Vector3(playerSize.x / 2, -playerSize.y / 2 + 0.01f, playerSize.z / 2);
        Vector3 corner2 = transform.position + new Vector3(-playerSize.x / 2, -playerSize.y / 2 + 0.01f, playerSize.z / 2);
        Vector3 corner3 = transform.position + new Vector3(playerSize.x / 2, -playerSize.y / 2 + 0.01f, -playerSize.z / 2);
        Vector3 corner4 = transform.position + new Vector3(-playerSize.x / 2, -playerSize.y / 2 + 0.01f, -playerSize.z / 2);

        //print(corner1 + "\n" + corner2 + "\n" + corner3 + "\n" + corner4);

        bool grounded1 = Physics.Raycast(corner1, -Vector3.up, 0.02f);
        bool grounded2 = Physics.Raycast(corner2, -Vector3.up, 0.02f);
        bool grounded3 = Physics.Raycast(corner3, -Vector3.up, 0.02f);
        bool grounded4 = Physics.Raycast(corner4, -Vector3.up, 0.02f);

        return (grounded1 || grounded2 || grounded3 || grounded4);
    }

    void OnTriggerEnter(Collider other)
    {
        //walk into coin
        if (other.CompareTag("Coin"))
        {
            coinSound.Play();
            GameManager.instance.IncreaseScore(1);
            //play sound and destroy
            //print(coinSound.isActiveAndEnabled);
            Destroy(other.gameObject);
        }
        //Game Over if player running into enemy or fall off
        else if (other.CompareTag("Enemy") || other.CompareTag("Falling"))
        {
            //reset the game
            GameManager.instance.GameOver();
        }
        else if (other.CompareTag("Goal"))
        {
            //send player to the next level
            GameManager.instance.IncreaseLevel();
        }
    }

    void CameraFollowPlayer()
    {
        //gain camera object
        Vector3 cameraPosition = Camera.main.transform.position;


        //modify position by player move
        cameraPosition.z = transform.position.z - cameraDistanceZ;

        Camera.main.transform.position = cameraPosition;
    }


}
