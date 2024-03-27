using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioStep : MonoBehaviour
{
    private AudioSource m_Source;
    public List<AudioClip> grassStepSounds;
    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }

    void Step()
    {
        Debug.Log("Step");
        if (grassStepSounds.Count == 0) return;

        AudioClip stepClip = grassStepSounds[Random.Range(0, grassStepSounds.Count)];
        m_Source.PlayOneShot(stepClip);
    }
}
