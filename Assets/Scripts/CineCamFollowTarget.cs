using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CineCamFollowTarget : MonoBehaviour
{
    //Target rail location. resets to here every frame. 
    //Base: 0,15,0
    public Vector3 vec3FollowTargetPosition = new Vector3(0.0f, 25.0f, -5.0f);

    //Refences to other things
    private CharacterController playerController;
    private CinemachineVirtualCamera cinecam;

    // Start is called before the first frame update
    void Start()
    {
        cinecam = GetComponent<CinemachineVirtualCamera>();
        playerController = PlayerMovement.instance.GetComponent<CharacterController>();
        PositionCamera();
    }

    // Update is called once per frame
    void Update()
    {
        //PositionCamera();
    }



    public void PositionCamera()
    {
        transform.position = playerController.transform.position + vec3FollowTargetPosition;
    }
}
