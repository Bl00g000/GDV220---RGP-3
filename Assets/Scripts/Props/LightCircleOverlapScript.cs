using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCircleOverlapScript : MonoBehaviour
{
    Collider[] nearbyCollisions;
    float fRadius = 0.0f;
    MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        fRadius =  meshFilter.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckCircleCollisions();

    }

    void CheckCircleCollisions()
    {
        // get all collisions within interact range
        nearbyCollisions = Physics.OverlapSphere(gameObject.transform.position, fRadius);



        // check through all colliders in Interact Range
        foreach (Collider _collider in nearbyCollisions)
        {
            if (_collider.gameObject.GetComponent<EnemyBase>())
            {
                _collider.gameObject.GetComponent<EnemyBase>().bFlashlighted = true;
                
                // Do damage to tendrils
                if (_collider.gameObject.GetComponent<EnemyTendril>() != null)
                {
                    _collider.gameObject.GetComponent<EnemyTendril>().TakeDamage(0.5f * Time.deltaTime);
                }
            }
        }
    }
}
