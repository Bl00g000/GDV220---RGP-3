using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    VisionCone playerVisionCone;
    public float fFlashLightDPS = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerVisionCone = gameObject.GetComponent<VisionCone>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEnmiesHit();
    }

    void CheckForEnmiesHit()
    {
        if (playerVisionCone.hitColliders.Count > 0)
        {
            foreach (Collider _collider in playerVisionCone.hitColliders)
            {
                if (_collider.CompareTag("Enemy"))
                {
                    EnemyBase enemyBase = _collider.GetComponent<EnemyBase>();
                    enemyBase.bFlashlighted = true;
                    enemyBase.fHealth -= fFlashLightDPS * Time.deltaTime;
                }
            }
        }
    }
}
