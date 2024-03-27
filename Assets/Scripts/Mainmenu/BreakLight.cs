using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BreakLight : MonoBehaviour
{
    public StreetLampMainMenu streetLight;
    public AudioSource breakAudio;
    private float startIntensity;
    public Light blight;
    public Light spotLight;
    public GameObject exitbutton2;
    public TMPro.TextMeshProUGUI text;
    public void Break()
    {
        streetLight.eLightType = StreetLampMainMenu.E_LightType.CONSTANT;
        if(streetLight.bLightIsOn)
        {
            streetLight.ToggleLight();
        }

        breakAudio.Play();

        blight.gameObject.SetActive(true);
        startIntensity = blight.intensity;
        streetLight.AddComponent<Rigidbody>().AddForce(streetLight.transform.right*100f - streetLight.transform.forward * 100f);
        StartCoroutine(turnBackOnlight());
        text.text = ":O";
    }

    public void Update()
    {
        if(blight.gameObject.activeSelf == true)
        {
            blight.intensity -= startIntensity * Time.deltaTime * 1.5f;
        }
    }

    private IEnumerator turnBackOnlight()
    {
        yield return new WaitForSeconds(2f);
        streetLight.ToggleLight(false);
        spotLight.intensity = 17f;
        spotLight.range = 2.5f;
        spotLight.innerSpotAngle = 6f;
        spotLight.spotAngle = 60f;
        exitbutton2.SetActive(true);
        text.text = ">:(";
    }
}
