using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float fCurrentCharge;

    [Header("Audio")]
    public AudioSource flashlightOnAudio;
    public AudioSource outOfChargeAudio;

    [Header("Keybinds")]
    public KeyCode RechargeButton = KeyCode.R;

    public static Flashlight instance;

    public event Action<bool> OnFlashLightToggle;

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
        fCurrentCharge = fMaxCharge;

        playerVisionCone = gameObject.GetComponent<VisionCone>();
        pointsToPlane = gameObject.GetComponentInChildren<PointsToPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        HandleFlashLight();

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
        if (Input.GetKey(RechargeButton))
        {
            RechargeBattery();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ToggleFlashLight();
        }
    }

    void RechargeBattery()
    {
        // if the flashlight is not active allow the player to recharge their flashlight battery
        if (!bFlashLightActive)
        {
            fCurrentCharge += fChargeGainPerSecond * Time.deltaTime;
            fCurrentCharge = Mathf.Clamp(fCurrentCharge, 0, fMaxCharge);
        }
    }

    void ToggleFlashLight()
    {
        if (bFlashLightActive)
        {
            OnFlashLightToggle?.Invoke(false);
            bFlashLightActive = false;

            // play audio
            flashlightOnAudio.Play();
        }
        else if (!bFlashLightActive && fCurrentCharge > 0)
        {
            // invoke event and set flashlightactive bool
            OnFlashLightToggle?.Invoke(true);
            bFlashLightActive = true;

            // play audio
            flashlightOnAudio.Play();
        }
    }

    void HandleFlashLight()
    {
        if (bFlashLightActive)
        {
            // check current charge is more than 0
            if (fCurrentCharge > 0)
            {
                // reduce charge by charge per second and clamp value
                fCurrentCharge -= fChargeGainPerSecond * Time.deltaTime;
                fCurrentCharge = Mathf.Clamp(fCurrentCharge, 0, fMaxCharge);
            }
            else // if battery has run out disable flashlight bool and invoke the action
            {
                OnFlashLightToggle?.Invoke(false);
                bFlashLightActive = false;

                // play audio
                outOfChargeAudio.Play();
            }
        }
    }
}
