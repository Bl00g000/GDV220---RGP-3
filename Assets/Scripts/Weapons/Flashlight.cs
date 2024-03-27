using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public VisionCone flashlightVisionCone;
    public PointsToPlane pointsToPlane;
    public float fFlashLightDPS = 2.0f;
    Collider[] collisions;
    public bool bFlashLightActive = false;
    public bool bHasFlashLight = false;
    private float startFloorIntensity;
    public Light floorLight;

    public float fMaxCharge = 10.0f;
    public float fChargeGainMultiplier = 10.0f;
    public float fChargeDrainPerSecond = 2.5f;
    public float fCurrentCharge;

    protected bool bFlickering = false;
    protected bool bPendingOffButIsFlickering = false;
    protected bool bHasFlickered0 = false;
    protected bool bHasFlickered1 = false;
    protected bool bHasFlickered2 = false;

    [Header("Audio")]
    public AudioSource flashlightOnAudio;
    public AudioSource outOfChargeAudio;
    public AudioSource windUpAudio;
    public AudioSource flickerAudio;

    [Header("Keybinds")]
    public KeyCode RechargeButton = KeyCode.R;

    public static Flashlight instance;

    public event Action<bool> OnFlashLightToggle;

    private void Awake()
    {
        startFloorIntensity = floorLight.intensity;
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

        pointsToPlane = gameObject.transform.parent.GetComponentInChildren<PointsToPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0f) { return; }
        if (!bHasFlashLight) { return; }

        // Get the mouse wheel movement speed
        float scrollSpeed = Input.mouseScrollDelta.y;

        // Do something with the scroll speed
        //Debug.Log("Mouse Wheel Speed: " + scrollSpeed);

        HandleInputs();
        HandleFlashLight();
        RechargeBattery();

        collisions = Physics.OverlapSphere(gameObject.GetComponentInParent<PlayerMovement>().transform.position, flashlightVisionCone.fFlashlightRange + 10);
        CheckForEnmiesHit();

        CheckChargeMilestones();


        
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
                    EnemyBase enemyBase = _collider.GetComponent<EnemyBase>();
                    enemyBase.bFlashlighted = true;

                    // Do damage if the enemy is a tendril
                    if (enemyBase.gameObject.GetComponent<EnemyTendril>() != null)
                    {
                        enemyBase.TakeDamage(fFlashLightDPS * Time.deltaTime);
                    }
                }
            }
        }
    }

    void HandleInputs()
    {
        if (GameManager.instance.bIsPaused) return;

        // Recharge flashlight battery if held down and flashlight isn't currently on
        if (Input.GetKey(RechargeButton))
        {
            RechargeBattery();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(bFlickering)
            {
                //bPendingOffButIsFlickering = true;
                EndFlickerCrouton();
            }
            else
            {
                ToggleFlashLight();
            }
            
        }
    }

    void RechargeBattery()
    {
        // if the flashlight is not active allow the player to recharge their flashlight battery
        if (!bFlashLightActive)
        {
            
            if (!windUpAudio.isPlaying && MathF.Abs(Input.mouseScrollDelta.y) > 0) // TODO: THIS NEEDS REWORKING THE SOUND SUCKS WOOHOO
            { 
                windUpAudio.Play();
                if (fCurrentCharge >= 0.05f)
                {
                    bHasFlickered2 = false;
                }
                if (fCurrentCharge >= 0.25f)
                {
                    bHasFlickered1 = false;
                }
                if (fCurrentCharge >= 0.5f)
                {
                    bHasFlickered0 = false;
                }
            }

            fCurrentCharge += MathF.Abs(Input.mouseScrollDelta.y * fChargeGainMultiplier) * Time.deltaTime;
            fCurrentCharge = Mathf.Clamp(fCurrentCharge, 0, fMaxCharge);

            
        }
    }

    void ToggleFlashLight(bool _playSound = true)
    {
        if (bFlashLightActive)
        {
            bFlashLightActive = false;
            OnFlashLightToggle?.Invoke(false);
            

            if(_playSound)
            {
                // play audio
                flashlightOnAudio.Play();
            }
           
        }
        else if (!bFlashLightActive && fCurrentCharge > 0)
        {
            floorLight.intensity = Mathf.Lerp(0f, startFloorIntensity, fCurrentCharge / fMaxCharge);

            // invoke event and set flashlightactive bool
            bFlashLightActive = true;
            OnFlashLightToggle?.Invoke(true);
            

            if (_playSound)
            {
                // play audio
                flashlightOnAudio.Play();
            }
        }
        else
        {
            if (_playSound)
            {
                outOfChargeAudio.Play();
            }
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
                fCurrentCharge -= fChargeDrainPerSecond * Time.deltaTime;
                fCurrentCharge = Mathf.Clamp(fCurrentCharge, 0, fMaxCharge);

                float fCurrentInternsity = fCurrentCharge / fMaxCharge;

                floorLight.intensity = Mathf.Lerp(0f, startFloorIntensity, fCurrentInternsity);
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


    //0 for tiny flicker, 1 for med flicker, 2 for super flicker
    void Flicker(int _flickerCategory)
    {
        if(bFlickering || !bFlashLightActive)
        {
            return;
        }
        

        switch (_flickerCategory)
        {
            case 0:
                bFlickering = true;
                StartCoroutine(FlickerCrouton0());
                break;
            case 1:
                bFlickering = true;
                StartCoroutine(FlickerCrouton1());
                break;
            case 2:
                bFlickering = true;
                StartCoroutine(FlickerCrouton2());
                break;
            default:
                break;
        
        };

    }

    void CheckChargeMilestones()
    {
        if(bFlickering)
        {
            return;
        }
        if (fCurrentCharge / fMaxCharge < 0.5 && bHasFlickered0 == false)
        {
            bHasFlickered0 = true;
            Flicker(0);
        }
        else if(fCurrentCharge / fMaxCharge <= 0.25 && bHasFlickered1 == false)
        {
            bHasFlickered1 = true;
            Flicker(1);
        }
        else if (fCurrentCharge / fMaxCharge <= 0.06 && bHasFlickered2 == false)
        {
            bHasFlickered2 = true;
            Flicker(2);
        }
    }

    void EndFlickerCrouton()
    {
        bFlickering = false;
        if (bFlashLightActive)
        {
            ToggleFlashLight();
        }
        
    }

    IEnumerator FlickerCrouton0()
    {
        ToggleFlashLight(false); //off
        flickerAudio.Play();
        yield return new WaitForSeconds(0.02f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//on
        bFlickering = false;
    }

    IEnumerator FlickerCrouton1()
    {
        ToggleFlashLight(false); //off
        flickerAudio.Play();
        yield return new WaitForSeconds(0.03f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//on
        yield return new WaitForSeconds(0.1f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//off
        flickerAudio.Play();
        yield return new WaitForSeconds(0.05f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//on
        bFlickering = false;
    }

    IEnumerator FlickerCrouton2()
    {
        ToggleFlashLight(false); //off
        flickerAudio.Play();
        yield return new WaitForSeconds(0.03f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//on
        yield return new WaitForSeconds(0.1f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//off
        flickerAudio.Play();
        yield return new WaitForSeconds(0.4f);
        if (!bFlickering) { yield break; }

        ToggleFlashLight(false);//on
        yield return new WaitForSeconds(0.3f);
        if (!bFlickering) { yield break; }
        ToggleFlashLight(false);//off
        fCurrentCharge = 0;
        outOfChargeAudio.Play();
        bFlickering = false;
        
    }

}
