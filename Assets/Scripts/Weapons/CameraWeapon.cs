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

    [Header("Audio")]
    public AudioSource cameraFlashAudio;
    float fInitialVolume;
    float fCurrentDuration = 0;

    public event Action<bool> OnCameraFlash;

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

        // audio volume and fade time
        fInitialVolume = cameraFlashAudio.volume;
    } 

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        CameraAudioFadeOut();

        // get colliders in range
        collisions = Physics.OverlapSphere(gameObject.GetComponentInParent<PlayerMovement>().transform.position, playerVisionCone.fFlashlightRange + 10);
    }

    void HandleInputs()
    {
        if (GameManager.instance.bIsPaused) return;

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

            // check through all nearby colliders and check if they are in the light currently
            foreach (var _collider in collisions)
            {
                if (pointsToPlane.LightContainsObject(_collider.gameObject))
                {

                    // Do things to enemy when they are in flash here
                    if (_collider.CompareTag("Enemy"))
                    {
                        EnemyBase enemyBase = _collider.GetComponent<EnemyBase>();
                        enemyBase.bFlashlighted = true;
                        enemyBase.fHealth -= fCameraFlashDamage;
                    }
                }
            }

            cameraFlashAudio.Play();
            fFilmCount--;
            bCanTakePicture = false;
            StartCoroutine(CameraFlash());
        }
    }

    IEnumerator CameraFlash()
    {
        OnCameraFlash?.Invoke(true);

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

        if (!transform.root.GetComponentInChildren<Flashlight>().bFlashLightActive)
        {
            OnCameraFlash?.Invoke(false);
        }
    }

    void CameraAudioFadeOut()
    {
        if (!bCanTakePicture)
        {
            fCurrentDuration += Time.deltaTime;

            float targetVolume = Mathf.Lerp(fInitialVolume, 0, fCurrentDuration / cameraFlashAudio.clip.length);

            // at about the half way point of the camera sound reduce pitch to remove weird aftersound
            if (fCurrentDuration > 0.4f)
            {
                cameraFlashAudio.pitch = 0f;
            }

            // set the volume
            cameraFlashAudio.volume = targetVolume;
        }
        else
        {
            // if flash is done reset values
            cameraFlashAudio.Stop();
            cameraFlashAudio.volume = fInitialVolume;
            cameraFlashAudio.pitch = 1f;
            fCurrentDuration = 0;
        }
    }
}
