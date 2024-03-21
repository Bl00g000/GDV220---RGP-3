using System;
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

    public float fMaxCharge = 10.0f;
    public float fChargeGainPerSecond = 2.5f;
    public float fChargeDrainPerSecond = 2.5f;
    float fCurrentCharge;

    [Header("Keybinds")]
    public KeyCode RechargeButton = KeyCode.R;

    public event Action<bool> FlashLightToggle;

    // Start is called before the first frame update
    void Start()
    {
        fCurrentCharge = fMaxCharge;

        playerVisionCone = gameObject.GetComponent<VisionCone>();
        pointsToPlane = gameObject.GetComponentInChildren<PointsToPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        RechargeBattery();

        
        
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

    void HandleInputs()
    {
        // Recharge flashlight battery if held down and flashlight isn't currently on
        if (Input.GetKeyDown(RechargeButton))
        {
            RechargeBattery();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ActivateFlashLight();
        }
        if (Input.GetMouseButtonUp(1))
        {
            DeactivateFlashLight();
        }
    }

    void RechargeBattery()
    {
        if (!bFlashLightActive)
        {
            Mathf.Clamp(fCurrentCharge += fChargeGainPerSecond * Time.deltaTime, 0, fMaxCharge);
        }
    }

    void ActivateFlashLight()
    {
        FlashLightToggle?.Invoke(true);
        bFlashLightActive = true;
    }

    void DeactivateFlashLight()
    {
        FlashLightToggle?.Invoke(false);
        bFlashLightActive = false;
    }
}
