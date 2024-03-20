using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (visionCone == null) visionCone = FindAnyObjectByType<VisionCone>();
        if (startObject == null) startObject = FindAnyObjectByType<PlayerMovement>().gameObject;
        if (shadowCone == null) shadowCone = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentMesh != null) Destroy(currentMesh);

        currentMesh = CreateMesh(visionCone.hitPositions);
        meshFilter.mesh = currentMesh;
        shadowCone.transform.GetComponent<MeshFilter>().mesh = currentMesh;


        // Update shadows

        shadowMap.transform.position = new Vector3(startObject.transform.position.x, Camera.main.transform.position.y - 1f, startObject.transform.position.z);

        shadowMap.transform.localScale = Vector3.one * shadowRadius * 2f;

        shadowCamera.orthographicSize = shadowRadius / 2f;
        shadowProjector.size = Vector3.one * shadowRadius;
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
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

        return planeMesh;
    }
}
