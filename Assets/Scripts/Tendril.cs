using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;

// owner: andy. 
// just a prototype, feel free to change.

public class Tendril : MonoBehaviour
{
    float health = 100f;
    float maxHealth = 100f;
    float minHealth = 10.0f;

    public List<MeshRenderer> tendrilMeshes;
    public float timeToGrow = 5f;
    public float RefreshRate = 0.05f;

    [Range(0, 1)]
    public float minGrow = 0.2f;

    [Range(0, 1)]
    public float maxGrow = 0.97f;

    List<Material> growVineMaterials = new List<Material>();
    private bool fullyGrown;

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

    private IEnumerator SlowUpdate()
    {
        while (Application.isPlaying)
        {
            for (int i = 0; i < growVineMaterials.Count; i++)
            {
                var mat = growVineMaterials[i];
                mat.SetFloat("_Grow", health / 100);
            }
            yield return null;
            yield return null;
        }
    }
    //
    // void Update()
    // {
    //     for (int i = 0; i < growVineMaterials.Count; i++)
    //     {
    //         var mat = growVineMaterials[i];
    //         mat.SetFloat("_Grow", health / 100);
    //     }
    // }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Light")
        {
            Debug.Log("LightDetected");
            if (health > minHealth)
            {
                health -= 2f;
                if (health <= 0f) { health = minHealth; }
            }
            else
            {
                health = minHealth;
            }
        }
    }

    void FixedUpdate()
    {
        if (health < minHealth)
        {
            health = minHealth;
        }
        if (health < maxHealth)
        {
            health += 1f;
            if (health > maxHealth) { health = maxHealth; }
        }
    }
}
