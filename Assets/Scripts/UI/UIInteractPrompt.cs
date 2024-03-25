using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class UIInteractPrompt : MonoBehaviour
{
    public static UIInteractPrompt instance;

    public GameObject testFlashlight;
    public GameObject testCamera;

    public GameObject spawnLocation;
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI blurbText;

    public float rotationSpeed;
    public float rotateDistance;

    private GameObject spawnedObject;
    private bool interacting;
    private bool active;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // for testing

        //Interact(testFlashlight, Vector3.one, "Flashlight", "cool thingy again i guess");
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            if(Input.GetMouseButtonDown(0) && Vector2.Distance(new Vector2(0.5f,0.5f), Camera.main.ScreenToViewportPoint(Input.mousePosition)) < rotateDistance)
            {
                interacting = true;
            }

            if(Input.GetMouseButton(0) && interacting)
            {
                spawnedObject.transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * rotationSpeed, Space.Self);
                spawnedObject.transform.Rotate(Vector3.forward, -Input.GetAxis("Mouse X") * rotationSpeed, Space.World);
            }
            else
            {
                interacting = false;
            }
        }
    }

    public void Interact(GameObject objectToSpawn, Vector3 objectScale, string objectName, string objectBlurb)
    {
        foreach(Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(true);
        }

        active = true;
        spawnedObject = Instantiate(objectToSpawn, spawnLocation.transform);
        spawnedObject.transform.localRotation = Quaternion.identity;
        spawnedObject.transform.localScale = objectScale;
        foreach (Transform t in spawnedObject.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = spawnLocation.layer;
        }

        nameText.text = objectName;
        blurbText.text = objectBlurb;
    }    

    public void Close()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(false);
        }

        active = false;
    }
}
