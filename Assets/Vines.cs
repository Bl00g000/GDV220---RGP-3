using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Vines : MonoBehaviour
{
    float health = 100f;

    public List<MeshRenderer> growVinesMeshes;
    public float timeToGrow = 5f;
    public float RefreshRate = 0.05f;
    [Range(0,1)]
    public float minGrow = 0.2f;
    [Range(0,1)]
    public float maxGrow = 0.97f;
    
    

    List<Material> growVineMaterials = new List<Material>();
    private bool fullyGrown;

    void Start()
    {
        for (int i = 0; i < growVinesMeshes.Count; i++)
        {
            for (int j = 0; j < growVinesMeshes[i].materials.Length; j++)
            {
                if (growVinesMeshes[i].materials[j].HasProperty("_Grow"))
                {
                    growVinesMeshes[i].materials[j].SetFloat("_Grow",minGrow);
                    growVineMaterials.Add(growVinesMeshes[i].materials[j]);
                }
            }
        }
    }


    void Update()
    {
        for (int i = 0; i < growVineMaterials.Count; i++)
        {
            var mat = growVineMaterials[i];
            mat.SetFloat("_Grow", health/100);
        }
    }
    // IEnumerator GrowVines(Material mat)
    // {
    //     float growValue = mat.GetFloat("_Grow");
    //     if (!fullyGrown)
    //     {
    //         while (growValue)
    //         {
    //             
    //         } 
    //     }
    // }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Light")
        {
            Debug.Log("LightDetected");
            if (health > 0f)
            {
                health -= 2f;
                if (health < 0f) health = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (health < 100f)
        {
            health += 1f;
            if (health > 100f) health = 100f;
        }
    }

}
