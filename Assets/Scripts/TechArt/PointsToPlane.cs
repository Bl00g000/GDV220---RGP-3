using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PointsToPlane : MonoBehaviour
{
    [Header("Assigned Variables")]
    public float shadowRadius;

    public GameObject shadowMap;
    public Camera shadowCamera;
    public DecalProjector shadowProjector;
    public int blurRadius;
    public int blurQuality;

    public RawImage testImage;

    [Header("Optional Assigned Variables")]
    public VisionCone visionCone;

    public GameObject startObject;

    [Header("Settings")]
    public float zoneHeight;

    // Found variables
    private MeshRenderer meshRenderer;

    private MeshFilter meshFilter;

    // Created Variables
    private Mesh currentMesh;

    List<GameObject> hits;

    public List<GameObject> touchingObjects;
    RaycastHit[] result;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        touchingObjects = new List<GameObject>();
        currentMesh = new Mesh();
        hits = new List<GameObject>();
        result = new RaycastHit[32];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (visionCone == null) visionCone = FindAnyObjectByType<VisionCone>();
        if (startObject == null) startObject = FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    public bool LightContainsObject(GameObject _obj)
    {
        return touchingObjects.Contains(_obj);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // if (currentMesh != null) Destroy(currentMesh);

        CreateMesh(visionCone.hitPositions);

        // Assign meshes and colliders
        meshFilter.mesh = currentMesh;
        foreach (Transform child in gameObject.transform)
        {
            child.GetComponent<MeshFilter>().mesh = currentMesh;
        }

        // Update shadows

        shadowMap.transform.position = new Vector3(startObject.transform.position.x,
            Camera.main.transform.position.y - 1f,
            startObject.transform.position.z);

        shadowMap.transform.localScale = Vector3.one * shadowRadius * 2f;

        shadowCamera.orthographicSize = shadowRadius / 2f;
        shadowProjector.size = Vector3.one * shadowRadius;

        // Update collisions 
        touchingObjects.Clear();

        Vector3 startPos = startObject.transform.position;

        hits.Clear();

        // Debug.Log(visionCone.hitPositions.Count);
        foreach (Vector3 point in visionCone.hitPositions)
        {
            startPos.y = point.y;
            // RaycastHit[] allHits = Physics.RaycastAll(startPos,
            //     (point - startPos).normalized,
            //     Vector3.Distance(startPos, point));
            // foreach (RaycastHit hit in allHits)
            // {
            //     if (!hits.Contains(hit.transform.gameObject))
            //     {
            //         hits.Add(hit.transform.gameObject);
            //     }
            // }
            // Debug.DrawLine(transform.position, point, Color.green);

            var size = Physics.RaycastNonAlloc(startPos,
                (point - startPos).normalized,
                result,
                Vector3.Distance(startPos, point));
            for (int i = 0; i < size; i++)
            {
                if (!hits.Contains(result[i].transform.gameObject))
                {
                    hits.Add(result[i].transform.gameObject);
                }
            }
        }

        touchingObjects = hits;
    }

    public void CreateMesh(List<Vector3> points)
    {
        // Mesh planeMesh = null;
        // planeMesh = new Mesh();
        currentMesh.Clear();
        // currentMesh = new Mesh();

        Vector3[] newVertices = new Vector3[points.Count + 1];
        for (int i = 0; i < points.Count; i++)
        {
            newVertices[i + 1] = transform.InverseTransformPoint(points[i]);
            newVertices[i + 1].y = transform.InverseTransformPoint(Vector3.up * zoneHeight).y;
        }
        newVertices[0] = transform.InverseTransformPoint(startObject.transform.position);
        newVertices[0].y = transform.InverseTransformPoint(Vector3.up * zoneHeight).y;

        List<int> triangles = new List<int>();
        for (int i = 1; i < points.Count; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        currentMesh.SetVertices(newVertices);
        currentMesh.SetTriangles(triangles.ToArray(), 0);

        currentMesh.RecalculateBounds();
        currentMesh.RecalculateNormals();
        currentMesh.RecalculateTangents();

    }
}
