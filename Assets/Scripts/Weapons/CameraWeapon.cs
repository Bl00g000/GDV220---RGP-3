using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWeapon : MonoBehaviour
{
    VisionCone playerVisionCone;
    PointsToPlane pointsToPlane;
    Collider[] collisions;
    public float fFilmCount = 2;

    public Light cameraFlashLight;
    public float fFlashMaxIntensity = 500f;

    public float fCameraFlashDamage = 10f;

    public static CameraWeapon instance;

    public float fFlashInSpeed = 10;
    public float fFlashOutSpeed = 10;

    bool bCanTakePicture = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerVisionCone = gameObject.GetComponentInParent<VisionCone>();
        pointsToPlane = gameObject.transform.parent.GetComponentInChildren<PointsToPlane>();
    } 

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

        // get colliders in range
        collisions = Physics.OverlapSphere(gameObject.GetComponentInParent<PlayerMovement>().transform.position, playerVisionCone.fFlashlightRange + 10);
    }

    void HandleInputs()
    {
        // Recharge flashlight battery if held down and flashlight isn't currently on
        if (Input.GetMouseButtonDown(0))
        {
            SnapPicture();
        }
    }

    void SnapPicture()
    {
        if (bCanTakePicture)
        {
            // check whether the flashlight on otherwise skip function
            if (fFilmCount <= 0) { return; }

            fFilmCount--;

            // check through all nearby colliders and check if they are in the light currently
            foreach (var _collider in collisions)
            {
                if (pointsToPlane.LightContainsObject(_collider.gameObject))
                {
                    if (_collider.CompareTag("Enemy"))
                    {
                        Debug.Log("enemy hit");
                        EnemyBase enemyBase = _collider.GetComponent<EnemyBase>();
                        enemyBase.bFlashlighted = true;
                        enemyBase.fHealth -= fCameraFlashDamage;
                    }
                }
            }

            bCanTakePicture = false;
            StartCoroutine(CameraFlash());
        }
    }

    IEnumerator CameraFlash()
    {
        while (cameraFlashLight.intensity < fFlashMaxIntensity)
        {
            cameraFlashLight.intensity += Time.deltaTime * fFlashInSpeed;

            yield return null;
        }

        yield return new WaitUntil(() => cameraFlashLight.intensity >= fFlashMaxIntensity);

        while (cameraFlashLight.intensity > 500)
        {
            cameraFlashLight.intensity -= Time.deltaTime * fFlashOutSpeed;

            yield return null;
        }

        while (cameraFlashLight.intensity > 100)
        {
            cameraFlashLight.intensity -= Time.deltaTime * (fFlashOutSpeed / 2);

            yield return null;
        }

        while (cameraFlashLight.intensity > 0)
        {
            cameraFlashLight.intensity -= Time.deltaTime * (fFlashOutSpeed / 2 / 2);

            yield return null;
        }

        yield return new WaitUntil(() => cameraFlashLight.intensity <= 0);

        bCanTakePicture = true;
    }
}
