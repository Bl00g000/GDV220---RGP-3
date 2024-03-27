using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class StreetLampMainMenu : MonoBehaviour
{
    public GameObject lightShaft;

    public GameObject playButton;
    public GameObject exitButton;

    public enum E_LightType
    {
        CONSTANT = 0,
        FLICKER = 1,
        MOTION = 2,
    };

    public E_LightType eLightType = E_LightType.CONSTANT;
    public GameObject SpotLightRoot;

    [Header("Audio")]
    //public AudioSource streetlightOnAudio;
    public AudioSource streetLightFlickerAudio;

    public bool bLightIsOn = true;

    //Flicker variables
    private bool bFlickerInProgress = false;


    private void Awake()
    {
        if (lightShaft)
        {
            lightShaft.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(bLightIsOn)
        {
           // streetlightOnAudio.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (eLightType)
        {
            case E_LightType.CONSTANT:
                ConstantUpdate();
                break;
            case E_LightType.FLICKER:
                FlickerUpdate();
                break;
            case E_LightType.MOTION:
                MotionUpdate();
                break;
             default:
                break;
        }
    }

    void ConstantUpdate()
    {
        
    }

    void FlickerUpdate()
    {
        //exits if already flickering
        if(bFlickerInProgress)
        {
            return;
        }

        //registers flicker start
        bFlickerInProgress = true;

        int rangeMax = 4;
        int rangeMin = 2;

        //Long flicker is only available for ON light
        if(bLightIsOn)
        {
            rangeMax = 4;
        }
        else
        {
            rangeMin = 2;
        }

        //starts random flicker length
        int flickerLengthSelection = Random.Range(rangeMin, rangeMax);
        switch (flickerLengthSelection)
        { 
            case 0:
                StartCoroutine(NanoFlicker());
                break;
            case 1:
                StartCoroutine(ShortFlicker());
                break;
            case 2:
                StartCoroutine(MediumFlicker());
                break;
            case 3:
                StartCoroutine(LongFlicker());
                break;
            default:
                break;
        };
    }

    void MotionUpdate()
    {

    }

    public void ToggleLight(bool changeButtons = true)
    {
        if(bLightIsOn)
        {
            if (changeButtons)
            {
                playButton.SetActive(true);
                exitButton.SetActive(false);
            }

            bLightIsOn = false;
            SpotLightRoot.SetActive(false);
            if (lightShaft) lightShaft.SetActive(false);
            //streetLightFlickerAudio.Play();
        }
        else
        {
            if (changeButtons)
            {
                exitButton.SetActive(true);
                playButton.SetActive(false);
            }

            bLightIsOn = true;
            SpotLightRoot.SetActive(true);
            if (lightShaft) lightShaft.SetActive(true);
            //streetlightOnAudio.Play();
            streetLightFlickerAudio.Play();
        }
    }

    IEnumerator NanoFlicker()
    {
        ToggleLight();
        float flickerLength = 0.05f;
        yield return new WaitForSeconds(flickerLength);
        ToggleLight();
        yield return new WaitForSeconds(flickerLength);
        bFlickerInProgress = false;
    }
    IEnumerator ShortFlicker()
    {
        ToggleLight();
        float flickerLength = 0.2f;
        yield return new WaitForSeconds(flickerLength);

        bFlickerInProgress = false;
    }
    IEnumerator MediumFlicker()
    {
        ToggleLight();
        float flickerLength = 0.8f;
        yield return new WaitForSeconds(flickerLength);

        bFlickerInProgress = false;
    }
    IEnumerator LongFlicker()
    {
        ToggleLight();
        float flickerLength = 2.5f;
        yield return new WaitForSeconds(flickerLength);

        bFlickerInProgress = false;
    }

}
