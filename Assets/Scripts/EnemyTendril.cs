using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyTendril : EnemyBase
{
    Tendril tendril;

    public List<MeshRenderer> tendrilMeshes;
    List<Material> growVineMaterials;

    [Range(0, 1)]
    public float minGrow = 0.2f;

    float fPlayerDamage = 1.0f;
    bool bPlayerDamageLockout = false;

    void Awake()
    {
        growVineMaterials = new List<Material>();
    }

    void Start()
    {
        for (int i = 0; i < tendrilMeshes.Count; i++)
        {
            for (int j = 0; j < tendrilMeshes[i].materials.Length; j++)
            {
                if (tendrilMeshes[i].materials[j].HasProperty("_Grow"))
                {
                    tendrilMeshes[i].materials[j].SetFloat("_Grow", minGrow);
                    growVineMaterials.Add(tendrilMeshes[i].materials[j]);
                }
            }
        }

        StartCoroutine(SlowUpdate());
    }

    void Update()
    {
        if (bFlashlighted)
        {

            // Display damage indicator
            bFlashlighted = false;
            
            if (fHealth <= 0f)
            {
                // Spawn Death VFX
                StopAllCoroutines();
                DestroyImmediate(gameObject);
            }
        }
        else
        {
            // test 
            // visualEffect.enabled = false;

            // Regen
            if (fHealth < fMaxHealth)
            {
                fHealth += (fMaxHealth - fHealth) * 0.01f;

            }
        }
    }

    private IEnumerator SlowUpdate()
    {
        while (Application.isPlaying)
        {
            for (int i = 0; i < growVineMaterials.Count; i++)
            {
                var mat = growVineMaterials[i];
                mat.SetFloat("_Grow", fHealth / fMaxHealth);
            }
            yield return null;
            yield return null;
        }
    }

    private void OnTriggerStay(Collider _collision)
    {
        // If player stays in tendril collision box then deal damage over time
        if (_collision.gameObject ==  PlayerData.instance.gameObject)
        {
            // Turn IFrames off for tendril damage
            PlayerData.instance.TakeDamage(fDamage * Time.fixedDeltaTime, false);
        }
    }
}
