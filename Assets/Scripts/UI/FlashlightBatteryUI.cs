using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBatteryUI : MonoBehaviour
{
    public MPImage chargeBar;
    public MPImage backgroundBar;
    public MPImage flashlightOnIcon;
    public TMPro.TextMeshProUGUI batteryCharge;
    private bool flashlightFound;
    private void Start()
    {
        chargeBar.gameObject.SetActive(false);
        backgroundBar.gameObject.SetActive(false);
        flashlightOnIcon.gameObject.SetActive(false);
        flashlightFound = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Flashlight.instance == null) return;
        if (!flashlightFound) FlashlightSetup();

        batteryCharge.text = (Flashlight.instance.fCurrentCharge / Flashlight.instance.fMaxCharge * 100).ToString("F0") + "%";
        chargeBar.fillAmount = (Flashlight.instance.fCurrentCharge / Flashlight.instance.fMaxCharge);
        flashlightOnIcon.color = new Color(flashlightOnIcon.color.r, flashlightOnIcon.color.g, flashlightOnIcon.color.b, Flashlight.instance.fCurrentCharge / Flashlight.instance.fMaxCharge);
    }

    private void FlashlightSetup()
    {
        flashlightFound = true;
        chargeBar.gameObject.SetActive(true);
        backgroundBar.gameObject.SetActive(true);
        Flashlight.instance.OnFlashLightToggle += ToggleBeam;
    }
    private void ToggleBeam(bool on)
    {
        flashlightOnIcon.gameObject.SetActive(on);
    }
}
