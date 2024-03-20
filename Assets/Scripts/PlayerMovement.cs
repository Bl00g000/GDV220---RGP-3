using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Data;
using UnityEngine.UIElements;
//using MPUIKIT;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Movement")]
    public float moveSpeed = 10.0f;
    public float rotSpeed = 10.0f;

    public GameObject respawnLocation;    //Spawn Location object

    //public Rigidbody trackRigidBody;    //RigidBody
    public CharacterController playerController;    //CharacterController

    //public GameObject followTarget;     //Camera followtarget

    private Vector3 moveDirection;      //Move direction declaration

   // public Transform characterTransform;//Character transform

    private float horizontalInput;      //keyboard movement inputs
    private float verticalInput;


    public GameObject UIOverlayObject = null;    //UI overlay

    protected bool canMove = true;         //Movement bool - turns off when powered off/dead/in screen.
    public bool respawning = false;     //respawning bool




    //[Header("Turn things")]
    //public bool IsTurningLeft = false;
    //public bool IsTurningRight = false;
    public float LerpValue = 0.0f;
    public float TargetRotationValue = 0.0f;
    public float modelTurnForgiveness = 7.0f;

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();


        //reset location
       // trackRigidBody.position = respawnLocation.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("c"))
        {
           // UIOverlayObject.gameObject.GetComponent<InGameMenu>().ToggleCamControls();
        }


        if (canMove)
        {
            
        }


        //Tick timers (fog, respawn)
        TickTimers();

        //Get movement input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //prevents overspeeding
        SpeedControl();
    }

    //Move player is here
    private void FixedUpdate()
    {

        //Player movement catch (eg no move if respawning etc
        if (canMove && (verticalInput != 0 || horizontalInput != 0))
        {
            // calculate movement direction
            moveDirection = playerController.transform.forward * verticalInput + playerController.transform.right * horizontalInput;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            moveDirection.Normalize();
            //moveDirection = new Vector3
            //Turn character according to movement
            RotateCharacter();
            //if(verticalInput != 0 && horizontalInput != 0)
            //{
            //
            //}
            playerController.Move(moveDirection * Time.deltaTime * moveSpeed);

            //Move player
            //trackRigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {

            //IsTurningLeft = false;
            //IsTurningRight = false;
        }

        //SOUNDS PLAYING AND STOPING
        //if (trackRigidBody.velocity.magnitude > 0.1f)
        //{
        //    //movementAudio.SetActive(true);
        //}
        //else
        //{
        //   // movementAudio.SetActive(false);
        //}
    }


    private void SpeedControl() //From bloo
    {
        //Vector3 flatVel = new Vector3(trackRigidBody.velocity.x, 0f, trackRigidBody.velocity.z);
        //
        //// limit velocity if needed
        //if (flatVel.magnitude > moveSpeed)
        //{
        //    Vector3 limitedVel = flatVel.normalized * moveSpeed;
        //    trackRigidBody.velocity = new Vector3(limitedVel.x, trackRigidBody.velocity.y, limitedVel.z);
        //}
    }


    private void RotateCharacter()
    {
        //moveDirection.Normalize();
        //
        //Vector3 newFromDir = trackRigidBody.transform.forward;
        //
        //Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        //Quaternion LerpedQuaternion = Quaternion.Lerp(trackRigidBody.rotation, targetRotation, Time.deltaTime * rotSpeed);
        //
        ////values for turning
        //LerpValue = LerpedQuaternion.eulerAngles.y;
        //TargetRotationValue = targetRotation.eulerAngles.y;
        //
        //
        //trackRigidBody.MoveRotation(LerpedQuaternion);                        // Smooth rotation
        //
        ////clamp error
        //if ((TargetRotationValue > 40 && TargetRotationValue < 50) || (TargetRotationValue > -2 && TargetRotationValue < 2))
        //{
        //    if (LerpValue > 200)
        //    {
        //        TargetRotationValue += 360;
        //    }
        //}
        //
        ////Bool checks for character animation
        //if (LerpValue < TargetRotationValue && Mathf.Abs(LerpValue - TargetRotationValue) > modelTurnForgiveness)
        //{
        //   // IsTurningLeft = false;
        //   // IsTurningRight = true;
        //}
        //else if (LerpValue > TargetRotationValue && Mathf.Abs(LerpValue - TargetRotationValue) > modelTurnForgiveness)
        //{
        //    //IsTurningLeft = true;
        //    //IsTurningRight = false;
        //}
        //else
        //{
        //    //IsTurningLeft = false;
        //    //IsTurningRight = false;
        //}
    }

    //Timer ticking tick tock tick tock tick tock its one o'clock
    private void TickTimers()
    {
        
    }


 

    public void Victory()
    {
        canMove = false;
        //do a lil dance
       // UIOverlayObject.gameObject.GetComponent<InGameMenu>().VictoryMessage();

    }

    public void PlayerDefeat()
    {
        //terminal = null;
        canMove = false;
        //do a lil sad face
       // UIOverlayObject.gameObject.GetComponent<InGameMenu>().DefeatMessage();
    }

    private void OnDestroy()
    {
        UIOverlayObject = null;
    }
}
