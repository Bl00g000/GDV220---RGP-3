using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyTendril : EnemyBase
{
    private VisualEffect visualEffect;
    Tendril tendril;

    public List<MeshRenderer> tendrilMeshes;
    List<Material> growVineMaterials;

    [Range(0, 1)]
    public float minGrow = 0.2f;

    void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
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
            visualEffect.enabled = true;

            // Display damage indicator
            bFlashlighted = false;
            
            if (fHealth <= 0f)
            {
                // Spawn Death VFX
                StopAllCoroutines();
                visualEffect.enabled = false;
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
}
