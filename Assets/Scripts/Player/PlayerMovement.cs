using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Movement")]
    public float currentMoveSpeed = 10.0f;
    public float moveSpeed = 10.0f;
    public float strafeModifier = 0.7f;
    public float reverseModifier = 0.6f;
    public float rotSpeed = 20.0f;
    public float gravity = -9.8f;


    public bool bInTendrils = false;
    public float fTendrilMultiplier = 0.50f;
    public bool bIsWinding = false;
    public float fWindingMultiplier = 0.55f;

    public GameObject respawnLocation;    //Spawn Location object
    public AudioSource tendrilEnterAudio;

    //public Rigidbody trackRigidBody;    //RigidBody
    [HideInInspector]public CharacterController playerController;    //CharacterController

    //public GameObject followTarget;     //Camera followtarget

    public Vector3 moveDirection;      //Move direction declaration
    public Vector3 moveLookDiff = Vector3.zero;      //Move diff declaration
    public Vector3 testInverseVec = Vector3.zero;      //Move diff declaration
    public Vector3 testmovedirvec = Vector3.zero;      //Move diff declaration
    public Vector3 testsubtractedVec3 = Vector3.zero;      //Move diff declaration
    public Vector3 testsubtractedVec4 = Vector3.zero;      //Move diff declaration
    //public Vector3 testsubtractedVec1 = Vector3.zero;      //Move diff declaration
    //public Vector3 testsubtractedVec2 = Vector3.zero;      //Move diff declaration
    
    public Vector3 testaddedvec = Vector3.zero;      //Move diff declaration
   // public Vector3 lookEulers = Vector3.zero;      //Move diff declaration
    public Plane plane = new Plane(Vector3.up, 0);
    public Vector3 mouseWorldPosition;
    public Vector3 lookDirection;      //Look direction declaration


   // public Transform characterTransform;//Character transform

    private float horizontalInput;      //keyboard movement inputs
    private float verticalInput;


    public GameObject UIOverlayObject = null;    //UI overlay

    public bool canMove = true;         //Movement bool - turns off when powered off/dead/in screen.
    public bool canRotate = true;         //Rotation bool - turns off when powered off/dead/in screen.
    public bool knockingBack = false;
    public bool respawning = false;     //respawning bool




    //[Header("Turn things")]
    //public bool IsTurningLeft = false;
    //public bool IsTurningRight = false;
    public float LerpValue = 0.0f;
    public float TargetRotationValue = 0.0f;
    public float modelTurnForgiveness = 7.0f;
    
    // Animation Controller
    private Animator animator;

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

        animator = GetComponentInChildren<Animator>();
        if (!animator)
        {
            Debug.LogWarning("Animator Not Found On Player");
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

        currentMoveSpeed = moveSpeed;


        
        if (bIsWinding)
        {
            currentMoveSpeed *= fWindingMultiplier;
        }
        if(bInTendrils)
        {
            currentMoveSpeed *= fTendrilMultiplier;
        }


       

         

    }

    private void MovePlayer()
    {

        if (!knockingBack)
        {
            SpeedControl();
        }

        //Get movement input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        //Player movement catch (eg no move if respawning etc
        if (canMove && (verticalInput != 0 || horizontalInput != 0))
        {
            //Move direction based on wasd input
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection.Normalize();

            //changes speed depending on direction
            
           
            

            //Turn character according to movement
            
            //if(verticalInput != 0 && horizontalInput != 0)
            //{
            //
            //}
            playerController.Move(moveDirection * Time.deltaTime * currentMoveSpeed);
            //playerController.
            
            // Update Animation Controller (Andy)
            animator.SetFloat("Speed_Blend", 1,0.1f, Time.deltaTime);
             Debug.Log(moveSpeed);

        }
        else if(knockingBack)
        {
            playerController.Move(moveDirection * Time.deltaTime * currentMoveSpeed);
            //playerController.
            currentMoveSpeed *= 0.98f;
            // Update Animation Controller (Andy)
            animator.SetFloat("Speed_Blend", 1, 0.1f, Time.deltaTime);
            Debug.Log(moveSpeed);
        }
        else
        {
            moveDirection = Vector3.zero;
            
            animator.SetFloat("Speed_Blend", 0,0.1f, Time.deltaTime);
            //IsTurningLeft = false;
            //IsTurningRight = false;
        }

        //Gravity
        playerController.Move(Vector3.up * Time.deltaTime * gravity);

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

        if(!canRotate)
        {
            return;
        }
        //Mouse to world point
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out distance))
        {
            mouseWorldPosition = ray.GetPoint(distance);
        }

        //moveDirection.Normalize();
        


        Vector3 newFromDir = playerController.transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z), Vector3.up);
        Quaternion LerpedQuaternion = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        
        //values for turning
        LerpValue = LerpedQuaternion.eulerAngles.y;
        TargetRotationValue = targetRotation.eulerAngles.y;

        //transform.Rotate(lookDirection);
        transform.Rotate(LerpedQuaternion.eulerAngles , Space.Self);//.MoveRotation(LerpedQuaternion);                        // Smooth rotation

        transform.rotation = LerpedQuaternion;


        //Changes the look direction to be the player rotation
        lookDirection = playerController.transform.forward + playerController.transform.right;
        lookDirection = new Vector3(lookDirection.x, 0, lookDirection.z);
        lookDirection.Normalize();

        //lookEulers = transform.rotation.eulerAngles;

        

        //clamp error
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


    //public void PlayInTendrilAudio()
    //{
    //    
    //}


    public void KnockPlayerBack(float _knockbackSpeed, Vector3 _knockbackDirection)
    {
        canMove = false;
        canRotate = false;
        knockingBack = true;
        //set rotation here!!!!!!
        moveDirection = _knockbackDirection;
        currentMoveSpeed = _knockbackSpeed;
        StartCoroutine(KnockbackCrouton());
    }

    IEnumerator KnockbackCrouton()
    {


        yield return new WaitForSeconds(1.2f);
        canMove = true;
        canRotate = true;
        knockingBack = false;
        currentMoveSpeed = moveSpeed;
    }


    private void OnDestroy()
    {
        UIOverlayObject = null;
    }
}
