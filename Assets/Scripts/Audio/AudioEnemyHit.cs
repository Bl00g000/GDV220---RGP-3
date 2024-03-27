using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnemyHit : MonoBehaviour
{
    private EnemyBase enemyBase;
    private AudioSource m_audioSource;
    public List<AudioClip> hitClips = new List<AudioClip>();
    public float volumeScale;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        enemyBase = transform.parent.parent.GetComponent<EnemyBase>();
    }

    private void OnEnemyDamaged(float damage)
    {
        Debug.Log("enemy Damaged");
        if (hitClips.Count == 0) return;

        AudioClip hitClip = hitClips[Random.Range(0, hitClips.Count)];

        float volume = Mathf.Clamp((damage / enemyBase.fMaxHealth), 0f, 0.2f);
        volume = Mathf.Clamp(volume * volumeScale, 0f, 1f);

        m_audioSource.PlayOneShot(hitClip, volume);
    }
}
