using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightGraphics : MonoBehaviour
{
    [Header("Optional Variables")]
    public Flashlight attachedFlashlight;

    public List<GameObject> toggleObjects = new List<GameObject>();
    public List<GameObject> toggleInverseObjects = new List<GameObject>();
    void Awake()
    {
        if(attachedFlashlight == null) attachedFlashlight = transform.root.GetComponent<Flashlight>();
    }

    private void Start()
    {
        attachedFlashlight.OnFlashLightToggle += OnFlashlightToggle;
    }

    // Update is called once per frame
    void OnFlashlightToggle(bool toggled)
    {
        foreach (GameObject toggleObj in toggleObjects)
        {
            toggleObj.SetActive(toggled);
        }

        foreach (GameObject toggleObj in toggleInverseObjects)
        {
            toggleObj.SetActive(!toggled);
        }
    }
}
