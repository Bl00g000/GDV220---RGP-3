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
     

    BoxCollider boxCollider;
    float startingSizeZ = 1f;
    float startingCenterZ = 1f;
    void Awake()
    {
        growVineMaterials = new List<Material>();
        boxCollider =  GetComponent<BoxCollider>();
        if (boxCollider)
        {
            startingSizeZ = boxCollider.size.z;
            startingCenterZ = boxCollider.center.z;
        }
        bCanDie = false;
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
            
            //if (fHealth <= 0f)
            //{
            //    // Spawn Death VFX
            //    StopAllCoroutines();
            //    DestroyImmediate(gameObject);
            //}
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

        if (!Flashlight.instance.pointsToPlane.LightContainsObject(gameObject) || !Flashlight.instance.bFlashLightActive)
        {
            bFlashlighted = false;
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


                //Debug.Log(fHealth/fMaxHealth);
                // resize
                boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y,fHealth/fMaxHealth * startingSizeZ);
                boxCollider.center = new Vector3(boxCollider.center.x,
                    boxCollider.center.y,
                    startingCenterZ * fHealth/fMaxHealth);
            }
            yield return null;
            yield return null;
        }
    }
   

    private void OnTriggerEnter(Collider _collision)
    {
        // If player stays in tendril collision box then deal damage over time
        if (_collision.gameObject == PlayerData.instance.gameObject)
        {
            //makes player be in the tentacles for movespeed
            PlayerMovement.instance.tendrilEnterAudio.Play();
            
        }
    }
    private void OnTriggerStay(Collider _collision)
    {
        // If player stays in tendril collision box then deal damage over time
        if (_collision.gameObject ==  PlayerData.instance.gameObject)
        {
            PlayerMovement.instance.bInTendrils = true;
            // Turn IFrames off for tendril damage
            PlayerData.instance.TakeDamage(fDamage * Time.fixedDeltaTime, false);
        }
    }
    private void OnTriggerExit(Collider _collision)
    {
        // If player stays in tendril collision box then deal damage over time
        if (_collision.gameObject == PlayerData.instance.gameObject)
        {
            //makes player be in the tentacles for movespeed
            PlayerMovement.instance.bInTendrils = false;
        }
    }

    private void OnDestroy()
    {
        
        PlayerMovement.instance.bInTendrils = false;
        

        
    }
}
