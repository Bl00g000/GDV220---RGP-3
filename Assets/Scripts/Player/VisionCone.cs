using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Transform unitTransform;

    Vector3 v3mousePos;
    Vector3 v3dir;
    
    public LayerMask hitLayers;
    public List<LayerMask> shootThroughLayers;

    public int iRayCount = 30;
    public float fFOVdeg = 45;
    public float fFlashlightRange = 10.0f;
    public float hitOffset;
    public List<Vector3> hitPositions = new List<Vector3>();
    public List<Collider> hitColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        v3dir = unitTransform.forward;
        ConeRayCasts();
    }


    void ConeRayCasts()
    {
        float fAngleStep = DegToRad(fFOVdeg) / iRayCount;
        float fPlayerAngle = Mathf.Atan2(unitTransform.forward.x, unitTransform.forward.z);
        float fInitialAngle = fPlayerAngle - DegToRad(fFOVdeg / 2);

        // Clear the list before populating it with new hit positions
        hitPositions.Clear();

        for (int i = 0; i < iRayCount; i++)
        {
            float fAngleOffset = i * fAngleStep;
            Vector3 rayDirection = new Vector3(Mathf.Sin(fInitialAngle + fAngleOffset), 0, Mathf.Cos(fInitialAngle + fAngleOffset));

            Vector3 playPos = new Vector3(unitTransform.position.x, 0.1f, unitTransform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(playPos, rayDirection, out hit, fFlashlightRange, hitLayers))
            {
                // If there is a hit, add the hit point to the hitpositions
                Debug.DrawRay(playPos, hit.point - playPos, Color.blue);

                // check if the layer requires a hit back
                if(ListContainsLayer(shootThroughLayers, (LayerMask)hit.transform.gameObject.layer))
                {
                    Vector3 hitDirection = (hit.point - playPos).normalized;

                    RaycastHit backHit;
                    Ray backRay = new Ray(hit.point + hitDirection * 5f, -hitDirection);

                    if (hit.collider.Raycast(backRay, out backHit, 5.1f))
                    {
                        Debug.DrawRay(playPos + rayDirection * fFlashlightRange, backRay.direction * Vector3.Distance(backHit.point, playPos + rayDirection * fFlashlightRange), Color.green);
                        Debug.Log("Shot Through!, got new POS!!");

                        hitPositions.Add(backHit.point + backHit.normal* hitOffset);

                        continue;
                    }
                    else
                    {
                        Debug.LogWarning("didnt find hit back, this might be an issue, please tell NATHANIEL HUNTER thanks!!");
                    }
                }

                // adds if no hit back was found or required!
                hitPositions.Add(hit.point + (hit.point - playPos).normalized * hitOffset);

                // Check if the hit collider already exists in the List so that we only add each hit collider once
                if (!hitColliders.Contains(hit.collider))
                {
                    hitColliders.Add(hit.collider);
                }
                continue;
            }
            else
            {
                // If no hit, add point at the end of the range
                hitPositions.Add(playPos + rayDirection * fFlashlightRange); 
                Debug.DrawRay(playPos, rayDirection * fFlashlightRange, Color.red);
            }
        }
    }

    private bool ListContainsLayer(List<LayerMask> _list, LayerMask _layer)
    {
        foreach(LayerMask _listLayer in _list)
        {
            if(_listLayer == (_listLayer | (1 << _layer))) return true;
        }
        return false;
    }

    float DegToRad(float _deg)
    {
        return _deg * Mathf.PI / 180;
    }
}
