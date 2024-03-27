using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPill : MonoBehaviour
{
    public AudioClip pillClip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerData.instance.OnPlayerConsumePill += OnPillConsume;
    }

    // Update is called once per frame
    void OnPillConsume()
    {
        audioSource.PlayOneShot(pillClip);
    }
}
