using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MPImage))]
public class UIHealthVignette : MonoBehaviour
{
    private MPImage healthImage;
    public float intensity;
    // Start is called before the first frame update
    void Start()
    {
        healthImage = GetComponent<MPImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float vignetteAmount = Mathf.Clamp(1f - (PlayerData.instance.fCurrentHealth / PlayerData.instance.fMaxHealth), 0, 1f);
        healthImage.color = new Color(vignetteAmount * intensity, 0, 0, 1);
    }
}
