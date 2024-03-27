using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFlashlightHit : MonoBehaviour
{
    private EnemyBase EnemyBase;
    private AudioSource m_audioSource;
    private float wantedVolume;
    public float volume;
    public float volumeSpeed;
    public float volumeMax;
    // Start is called before the first frame update
    void Start()
    {
        EnemyBase = transform.root.GetComponent<EnemyBase>();
        m_audioSource = GetComponent<AudioSource>();
        wantedVolume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        wantedVolume = 0;
        if (EnemyBase.bFlashlighted)
        {
            wantedVolume = 1;
        }

        m_audioSource.volume = Mathf.Clamp(Mathf.Lerp(m_audioSource.volume, wantedVolume, Time.deltaTime * volumeSpeed),0,volumeMax);
    }
}
