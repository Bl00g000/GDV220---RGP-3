using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateCamera : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
        transform.Rotate(Vector3.right,180,Space.World);
    }
}
