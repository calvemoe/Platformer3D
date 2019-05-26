using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //player characteristics
    [SerializeField]
    private float walkSpeed = 4f;
    [SerializeField]
    private float jumpForce = 1f;

    [SerializeField]
    private AudioSource coinSound;

    //player parameters
    Rigidbody rb;
    Collider cl;
    Vector3 playerSize;
    private Vector3 topLeftCorner;
    private Vector3 topRightCorner;
    private Vector3 bottomLeftCorner;
    private Vector3 bottomRightCorner;

    //camera settings
    [SerializeField]
    private float cameraDistanceZ = 5f;
    private Transform cameraTransform;

    //flag to keep track of key pressing
    private bool pressedJump = false;

   // Use this for initialization
    void Start () {
        //cache camera transform
        cameraTransform = Camera.main.transform;

        //player parameters initiating
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        
        //get player object size and corners for walking/jumping
        playerSize = cl.bounds.size;
        topLeftCorner = new Vector3(-playerSize.x / 2, -playerSize.y / 2 + 0.01f, playerSize.z / 2);
        topRightCorner = new Vector3(playerSize.x / 2, -playerSize.y / 2 + 0.01f, playerSize.z / 2);
        bottomLeftCorner = new Vector3(-playerSize.x / 2, -playerSize.y / 2 + 0.01f, -playerSize.z / 2);
        bottomRightCorner = new Vector3(playerSize.x / 2, -playerSize.y / 2 + 0.01f, -playerSize.z / 2);

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
        //get moving inputs
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //move player and setting moving direction to model if there is some inputs
        if (hAxis != 0 || vAxis != 0) {
            //gain movement vector
            Vector3 movement = new Vector3(hAxis * walkSpeed * Time.deltaTime, 0, vAxis * walkSpeed * Time.deltaTime);
            rb.MovePosition(transform.position + movement);

            //gain direction
            Vector3 direction = new Vector3(hAxis, 0, vAxis);

            //can apply direct to rigidbody component in object
            rb.rotation = Quaternion.LookRotation(direction);
        }
    }

    void JumpHandler() {
        float jAxis = Input.GetAxis("Jump");
 
        if (jAxis > 0) {
            bool isGrounded = CheckGrounded();

            //if we alredy jumping?
            if (!pressedJump && isGrounded) {
                pressedJump = true;
                Vector3 jumpVector = new Vector3(0, jAxis * jumpForce, 0);
                rb.AddForce(jumpVector, ForceMode.VelocityChange);
            }
        }
        else
            pressedJump = false;
    }

    bool CheckGrounded() {
        //found 4 corners
        Vector3 corner1 = transform.position + topLeftCorner;
        Vector3 corner2 = transform.position + topRightCorner;
        Vector3 corner3 = transform.position + bottomLeftCorner;
        Vector3 corner4 = transform.position + bottomRightCorner;

        return (Physics.Raycast(corner1, -Vector3.up, 0.02f)
             || Physics.Raycast(corner2, -Vector3.up, 0.02f) 
             || Physics.Raycast(corner3, -Vector3.up, 0.02f) 
             || Physics.Raycast(corner4, -Vector3.up, 0.02f));
    }

    void OnTriggerEnter(Collider other) {
        //walk into coin
        if (other.CompareTag("Coin")) {
            coinSound.Play();
            GameManager.instance.IncreaseScore(1);

            //play sound and destroy
            Destroy(other.gameObject);
        }
        //Game Over if player running into enemy or fall off
        else if (other.CompareTag("Enemy") || other.CompareTag("Falling")) {
            //reset the game
            GameManager.instance.GameOver();
        }
        else if (other.CompareTag("Goal")) {
            //send player to the next level
            GameManager.instance.IncreaseLevel();
        }
    }

    void CameraFollowPlayer() {
        //gain camera object
        Vector3 cameraPosition = cameraTransform.position;


        //modify position by player move
        cameraPosition.z = transform.position.z - cameraDistanceZ;

        Camera.main.transform.position = cameraPosition;
    }


}
