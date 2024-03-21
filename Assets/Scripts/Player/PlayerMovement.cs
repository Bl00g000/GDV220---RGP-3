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
    public float rotSpeed = 20.0f;
    public float gravity = -9.8f;

    public GameObject respawnLocation;    //Spawn Location object

    //public Rigidbody trackRigidBody;    //RigidBody
    [HideInInspector]public CharacterController playerController;    //CharacterController

    //public GameObject followTarget;     //Camera followtarget

    public Vector3 moveDirection;      //Move direction declaration
   // public Vector3 moveDirection;      //Move direction declaration
    public Plane plane = new Plane(Vector3.up, 0);
    public Vector3 mouseWorldPosition;
    public Vector3 lookDirection;      //Look direction declaration

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

        RotatePlayer();

        MovePlayer();
        

        //prevents overspeeding
       // SpeedControl();
    }

    //Move player is here
    private void FixedUpdate()
    {

        
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

    private void MovePlayer()
    {
        //Get movement input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        //Player movement catch (eg no move if respawning etc
        if (canMove && (verticalInput != 0 || horizontalInput != 0))
        {
            // calculate movement direction
            //W MOVES TOWARDS CHARCATER FORWARD
            moveDirection = playerController.transform.forward * verticalInput + playerController.transform.right * horizontalInput;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

            //W MOVES TOWARD 
            //////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            //UNCOMMENT THIS TO SEE HOW MOVING TOWARDS THE MOUSE WORKS!!!!
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            //////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////
            moveDirection.Normalize();



            //Turn character according to movement
            
            //if(verticalInput != 0 && horizontalInput != 0)
            //{
            //
            //}
            playerController.Move(moveDirection * Time.deltaTime * moveSpeed);

        }
        else
        {

            //IsTurningLeft = false;
            //IsTurningRight = false;
        }

       // playerController.Move(Vector3.up * Time.deltaTime * gravity);

        //SOUNDS PLAYING AND STOPpING
        //if (trackRigidBody.velocity.magnitude > 0.1f)
        //{
        //    //movementAudio.SetActive(true);
        //}
        //else
        //{
        //   // movementAudio.SetActive(false);
        //}

    }

    private void RotatePlayer()
    {

        
        //Mouse to world point
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out distance))
        {
            mouseWorldPosition = ray.GetPoint(distance);
        }

        moveDirection.Normalize();
        


        Vector3 newFromDir = playerController.transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z), Vector3.up);
        Quaternion LerpedQuaternion = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        
        //values for turning
        LerpValue = LerpedQuaternion.eulerAngles.y;
        TargetRotationValue = targetRotation.eulerAngles.y;

        //transform.Rotate(lookDirection);
        transform.Rotate(LerpedQuaternion.eulerAngles , Space.Self);//.MoveRotation(LerpedQuaternion);                        // Smooth rotation

        transform.rotation = LerpedQuaternion;



        //clamp error
        if ((TargetRotationValue > 40 && TargetRotationValue < 50) || (TargetRotationValue > -2 && TargetRotationValue < 2))
        {
            if (LerpValue > 200)
            {
                TargetRotationValue += 360;
            }
        }
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
