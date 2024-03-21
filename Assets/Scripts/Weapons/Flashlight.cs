using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    VisionCone playerVisionCone;
    PointsToPlane pointsToPlane;
    public float fFlashLightDPS = 2.0f;
    Collider[] collisions;
    public bool bFlashLightActive = false;

    // Start is called before the first frame update
    void Start()
    {
        playerVisionCone = gameObject.GetComponent<VisionCone>();
        pointsToPlane = gameObject.GetComponentInChildren<PointsToPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            bFlashLightActive = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            bFlashLightActive = false;
        }
        
        collisions = Physics.OverlapSphere(gameObject.GetComponentInChildren<PlayerMovement>().transform.position, playerVisionCone.fFlashlightRange + 10);
        CheckForEnmiesHit();
    }

    void CheckForEnmiesHit()
    {
        // check whether the flashlight on otherwise skip function
        if (!bFlashLightActive) { return; }

        // check through all nearby colliders and check if they are in the light currently
        foreach (var _collider in collisions) 
        {
            if (pointsToPlane.LightContainsObject(_collider.gameObject))
            {
                if (_collider.CompareTag("Enemy"))
                {
                    Debug.Log("enemy hit");
                    EnemyBase enemyBase = _collider.GetComponent<EnemyBase>();
                    enemyBase.bFlashlighted = true;
                    enemyBase.fHealth -= fFlashLightDPS * Time.deltaTime;
                }
            }
        }
    }
}
