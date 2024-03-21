using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBatteryUI : MonoBehaviour
{
    public MPImage chargeBar;

    // Update is called once per frame
    void Update()
    {
        chargeBar.fillAmount = (Flashlight.instance.fCurrentCharge / Flashlight.instance.fMaxCharge);
    }
}
