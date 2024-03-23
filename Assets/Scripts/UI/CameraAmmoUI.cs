using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraAmmoUI : MonoBehaviour
{
    TMP_Text cameraText;

    // Start is called before the first frame update
    void Start()
    {
        cameraText = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraText.text = CameraWeapon.instance.fFilmCount.ToString();
    }
}
