using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerHit : MonoBehaviour
{
    private AudioSource m_audioSource;
    public List<AudioClip> hitClips = new List<AudioClip>();
    public float volumeScale;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        PlayerData.instance.OnPlayerDamaged += OnPlayerDamaged;
    }

    private void OnPlayerDamaged(float damage) 
    {
        if (hitClips.Count == 0) return;

        AudioClip hitClip = hitClips[Random.Range(0, hitClips.Count)];

        float volume = Mathf.Clamp((damage / PlayerData.instance.fMaxHealth), 0f,0.2f);
        volume = Mathf.Clamp(volume * volumeScale, 0f, 1f);

        m_audioSource.PlayOneShot(hitClip, volume);
    }
}
