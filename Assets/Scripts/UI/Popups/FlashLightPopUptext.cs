using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashLightPopUptext : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMeshPro;
    bool conditionOne = false;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TMPro.TextMeshProUGUI>();
        textMeshPro.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!conditionOne)
        {
            if (Flashlight.instance.bHasFlashLight)
            {
                textMeshPro.enabled = true;
                if (Flashlight.instance.bFlashLightActive)
                {
                    conditionOne = true;
                    textMeshPro.enabled = false;
                }
            }
        }
        else
        {
            if (Flashlight.instance.fCurrentCharge < Flashlight.instance.fMaxCharge/2f)
            {
                textMeshPro.enabled = true;
                textMeshPro.text = "Use (Scroll Wheel) to charge the FlashLight (While Off)";
                if (!Flashlight.instance.bFlashLightActive && Mathf.Abs(Input.mouseScrollDelta.y) > 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
