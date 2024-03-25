using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightGraphics : MonoBehaviour
{
    [Header("Optional Variables")]
    public Flashlight attachedFlashlight;
    public CameraWeapon attachedCameraWeapon;

    public List<GameObject> toggleObjects = new List<GameObject>();

    void Awake()
    {
        if(attachedFlashlight == null) attachedFlashlight = transform.root.GetComponentInChildren<Flashlight>();
        if(attachedCameraWeapon == null) attachedCameraWeapon = transform.root.GetComponentInChildren<CameraWeapon>();
    }

    private void Start()
    {
        attachedFlashlight.OnFlashLightToggle += OnFlashlightToggle;
        attachedCameraWeapon.OnCameraFlash += OnFlashlightToggle;
        
    }

    // Update is called once per frame
    void OnFlashlightToggle(bool toggled)
    {
        foreach (GameObject toggleObj in toggleObjects)
        {
            toggleObj.SetActive(toggled);
        }
    }
}
