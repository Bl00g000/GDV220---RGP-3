using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Transform unitTransform;

    Vector3 v3mousePos;
    Vector3 v3dir;
    
    public LayerMask hitLayers;

    public int iRayCount = 30;
    public float fVisionFOVdeg = 45;
    public float fFlashlightFOVdeg = 70;
    public float fVisionRange = 12.0f;
    public float fFlashlightRange = 10.0f;
    public float hitOffset;
    public List<Vector3> hitPositions = new List<Vector3>();
    public float fZoneHeight;

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
        

        // Clear the list before populating it with new hit positions
        hitPositions.Clear();

        if (!Flashlight.instance.bFlashLightActive)
        {
            for (int i = 0; i < iRayCount; i++)
            {
                float fAngleStep = DegToRad(fVisionFOVdeg) / iRayCount;
                float fPlayerAngle = Mathf.Atan2(unitTransform.forward.x, unitTransform.forward.z);
                float fInitialAngle = fPlayerAngle - DegToRad(fVisionFOVdeg / 2);

                float fAngleOffset = i * fAngleStep;
                Vector3 rayDirection = new Vector3(Mathf.Sin(fInitialAngle + fAngleOffset), 0, Mathf.Cos(fInitialAngle + fAngleOffset));

                Vector3 playPos = new Vector3(unitTransform.position.x, transform.position.y + fZoneHeight, unitTransform.position.z);

                RaycastHit hit;
                if (Physics.Raycast(playPos, rayDirection, out hit, fVisionRange, hitLayers))
                {
                    // If there is a hit, add the hit point to the hitpositions
                    Debug.DrawRay(playPos, hit.point - playPos, Color.blue);

                    // adds if no hit back was found or required!
                    hitPositions.Add(hit.point + (hit.point - playPos).normalized * hitOffset);
                    continue;
                }
                else
                {
                    // If no hit, add point at the end of the range
                    hitPositions.Add(playPos + rayDirection * fVisionRange);
                    Debug.DrawRay(playPos, rayDirection * fVisionRange, Color.red);
                }
            }
        }
        else
        {
            for (int i = 0; i < iRayCount; i++)
            {
                float fAngleStep = DegToRad(fFlashlightFOVdeg) / iRayCount;
                float fPlayerAngle = Mathf.Atan2(unitTransform.forward.x, unitTransform.forward.z);
                float fInitialAngle = fPlayerAngle - DegToRad(fFlashlightFOVdeg / 2);

                float fAngleOffset = i * fAngleStep;
                Vector3 rayDirection = new Vector3(Mathf.Sin(fInitialAngle + fAngleOffset), 0, Mathf.Cos(fInitialAngle + fAngleOffset));

                Vector3 playPos = new Vector3(unitTransform.position.x, transform.position.y + fZoneHeight, unitTransform.position.z);

                RaycastHit hit;
                if (Physics.Raycast(playPos, rayDirection, out hit, fFlashlightRange, hitLayers))
                {
                    // If there is a hit, add the hit point to the hitpositions
                    Debug.DrawRay(playPos, hit.point - playPos, Color.blue);

                    // adds if no hit back was found or required!
                    hitPositions.Add(hit.point + (hit.point - playPos).normalized * hitOffset);
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
