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

        hitPositions.Clear(); // Clear the list before populating it with new hit positions

        for (int i = 0; i < iRayCount; i++)
        {
            float fAngleOffset = i * fAngleStep;
            Vector3 rayDirection = new Vector3(Mathf.Sin(fInitialAngle + fAngleOffset), 0, Mathf.Cos(fInitialAngle + fAngleOffset));

            RaycastHit hit;
            if (Physics.Raycast(unitTransform.position, rayDirection, out hit, fFlashlightRange, hitLayers))
            {
                hitPositions.Add(hit.point);
                Debug.DrawRay(unitTransform.position, hit.point - unitTransform.position, Color.magenta);
            }
            else
            {
                hitPositions.Add(unitTransform.position + rayDirection * fFlashlightRange); // If no hit, add point at the end of the range
                Debug.DrawRay(unitTransform.position, rayDirection * fFlashlightRange, Color.red);
            }
        }
    }


    float DegToRad(float _deg)
    {
        return _deg * Mathf.PI / 180;
    }
}
