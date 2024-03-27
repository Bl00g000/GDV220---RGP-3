using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MPImage))]
public class UIHealthVignette : MonoBehaviour
{
    private MPImage healthImage;
    public float intensity;
    public float resetSpeed = 1f;
    public float damageScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        healthImage = GetComponent<MPImage>();
        PlayerData.instance.OnPlayerDamaged += OnTakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        float wantedAmount = Mathf.Clamp(1f - (PlayerData.instance.fCurrentHealth / PlayerData.instance.fMaxHealth), 0, 1f);
        float vignetteAmount = Mathf.Lerp(healthImage.color.r, wantedAmount * intensity, Time.deltaTime*resetSpeed);
        healthImage.color = new Color(vignetteAmount, 0, 0, 1);
    }

    private void OnTakeDamage(float damage)
    {
        float percentage = damage / PlayerData.instance.fMaxHealth;
        healthImage.color += new Color(1 * percentage * damageScale, 0, 0);
    }
}
