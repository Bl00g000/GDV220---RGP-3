using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObtain : MonoBehaviour
{
    public List<AudioClip> obtainClips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (obtainClips.Count == 0) return;
        audioSource.PlayOneShot(obtainClips[Random.Range(0,obtainClips.Count)]);
    }

}
