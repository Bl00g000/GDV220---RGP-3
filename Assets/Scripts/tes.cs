using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tes : MonoBehaviour
{
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position + Vector3.up*5, Vector3.down, 10, 1 << LayerMask.NameToLayer("LightMap")))
        {
            Debug.Log("In LIGHT");
        }
    }
}
