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
    public float fFOVdeg = 45;
    public float fFlashlightRange = 10.0f;

    private List<Vector3> hitPositions = new List<Vector3>();

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
                hitPositions.Add(hit.point);
                Debug.DrawRay(playPos, hit.point - playPos, Color.blue);
            }
            else
            {
                // If no hit, add point at the end of the range
                hitPositions.Add(playPos + rayDirection * fFlashlightRange); 
                Debug.DrawRay(playPos, rayDirection * fFlashlightRange, Color.red);
            }
        }
    }


    float DegToRad(float _deg)
    {
        return _deg * Mathf.PI / 180;
    }
}
