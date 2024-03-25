using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightGraphics : MonoBehaviour
{
    [Header("Optional Variables")]
    public Flashlight attachedFlashlight;
    public List<MeshRenderer> visionRenderers = new List<MeshRenderer>();
    public CameraWeapon attachedCameraWeapon;

    public List<GameObject> toggleObjects = new List<GameObject>();

    public Material visionMaterial;
    public Material flashlightMaterial;
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

        if(toggled)
        {
            foreach (MeshRenderer visionRend in visionRenderers)
            {
                visionRend.material = flashlightMaterial;
            }
        }
        else
        {
            foreach (MeshRenderer visionRend in visionRenderers)
            {
                visionRend.material = visionMaterial;
            }
        }
    }
}
