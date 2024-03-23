using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateToCam : MonoBehaviour
{
    public Vector3 directionToFace = Vector3.forward;

    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(directionToFace, transform.up);

        transform.rotation = rotation;
    }
}
