using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject shadowCone;

    [Header("Settings")]
    public float zoneHeight;

    // Found variables
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    // Created Variables
    private Mesh currentMesh;

    public List<Collider> touchingColliders;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        touchingColliders = new List<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (visionCone == null) visionCone = FindAnyObjectByType<VisionCone>();
        if (startObject == null) startObject = FindAnyObjectByType<PlayerMovement>().gameObject;
        if (shadowCone == null) shadowCone = transform.GetChild(0).gameObject;
    }

    private void LateUpdate()
    {
        if (currentMesh != null && currentMesh.vertices.Distinct().Count() > 3)
        {
            transform.GetComponent<MeshCollider>().sharedMesh = currentMesh;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMesh != null) Destroy(currentMesh);

        currentMesh = CreateMesh(visionCone.hitPositions);

        // Assign meshes and colliders
        meshFilter.mesh = currentMesh;
        shadowCone.transform.GetComponent<MeshFilter>().mesh = currentMesh;

        // Update shadows

        shadowMap.transform.position = new Vector3(startObject.transform.position.x, Camera.main.transform.position.y - 1f, startObject.transform.position.z);

        shadowMap.transform.localScale = Vector3.one * shadowRadius * 2f;

        shadowCamera.orthographicSize = shadowRadius / 2f;
        shadowProjector.size = Vector3.one * shadowRadius;
    }

    public Mesh CreateMesh(List<Vector3> points)
    {
        Mesh planeMesh = null;

        planeMesh = new Mesh();
        planeMesh.vertices = new Vector3[points.Count + 1];

        Vector3[] newVertices = new Vector3[points.Count + 1]; 
        for (int i = 0; i < points.Count; i++) 
        {
            newVertices[i+1] = transform.InverseTransformPoint(points[i]);
            newVertices[i + 1].y = transform.InverseTransformPoint(Vector3.up*zoneHeight).y;
        }
        newVertices[0] = transform.InverseTransformPoint(startObject.transform.position);
        newVertices[0].y = transform.InverseTransformPoint(Vector3.up * zoneHeight).y;

        List<int> triangles = new List<int>();
        for (int i = 1; i < points.Count; i++) 
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i+1);
        }

        planeMesh.SetVertices(newVertices);
        planeMesh.SetTriangles(triangles.ToArray(), 0);

        planeMesh.RecalculateBounds();
        planeMesh.RecalculateNormals();
        planeMesh.RecalculateTangents();

        return planeMesh;
    }
}
