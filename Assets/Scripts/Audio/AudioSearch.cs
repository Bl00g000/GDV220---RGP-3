using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSearch : MonoBehaviour
{
    public List<AudioClip> searchClips = new List<AudioClip>();
    private Searchable searchObj;
    private AudioSource searchSource;
    // Start is called before the first frame update
    void Start()
    {
        searchObj = transform.parent.GetComponent<Searchable>();
        searchSource = GetComponent<AudioSource>();
        searchObj.OnSearching += OnSearch;
    }

    // Update is called once per frame
    void OnSearch()
    {
        if (searchClips.Count == 0) return;

        searchSource.PlayOneShot(searchClips[Random.Range(0,searchClips.Count)]);
    } 
}
