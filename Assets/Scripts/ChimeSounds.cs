using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ChimeSounds : MonoBehaviour
{
    public AudioClip Chime0;
    public AudioClip Chime1;
    public AudioClip Chime2;
    public AudioClip Chime3;
    public AudioClip Chime4;

     AudioSource AudioSource;

    bool bChimeTime = true;

    float fChimeGap = 2.0f;
    int PreviousChimeGap = 1;

    int iPreviousChimeClip = 0;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = Chime1;
        StartCoroutine(InitialPause());
       
    }

    // Update is called once per frame
    void Update()
    {
        ChimeUpdate();
    }


    void ChimeUpdate()
    {
        //exits if already flickering
        if (bChimeTime)
        {
            return;
        }

        //registers flicker start
        bChimeTime = true;

        int rangeMax = 4;
        int rangeMin = 0;
       
        //Long flicker is only available for ON light
        if (PreviousChimeGap == 0)
        {
            rangeMin = 1;
        }

        //starts random flicker length
        int chimeLengthSelection = Random.Range(rangeMin, rangeMax);
        PreviousChimeGap = chimeLengthSelection;
        switch (chimeLengthSelection)
        {
            case 0:
                fChimeGap = 4.0f;
                break;
            case 1:
                fChimeGap = 7.0f;
                break;
            case 2:
                fChimeGap = 11.0f;
                break;
            case 3:
                fChimeGap = 15.0f;
                break;
            default:
                break;
        };
       
        RandomiseChimeClip();

        StartCoroutine(PlayChime(fChimeGap));
        
    }

   void RandomiseChimeClip()
    {
        int rangeMax = 5;
        int rangeMin = 0;

        int chimeClipSelection = Random.Range(rangeMin, rangeMax);

        while (chimeClipSelection == iPreviousChimeClip)
        {
            chimeClipSelection = Random.Range(rangeMin, rangeMax);

        }


        //gets random chime
        iPreviousChimeClip = chimeClipSelection;
        switch (chimeClipSelection)
        {
            case 0:
                AudioSource.clip = Chime0;
                break;
            case 1:
                AudioSource.clip = Chime1;
                break;
            case 2:
                AudioSource.clip = Chime2;
                break;
            case 3:
                AudioSource.clip = Chime3;
                break;
            case 4:
                AudioSource.clip = Chime4;
                break;
            default:
                break;
        };
    }

    IEnumerator PlayChime(float _seconds)
    {
        Debug.Log("audio play!" + _seconds);
        AudioSource.Play();
        yield return new WaitForSeconds(_seconds);
        bChimeTime = false;
        
    }
    IEnumerator InitialPause()
    {
        yield return new WaitForSeconds(15.0f);
        bChimeTime = false;
    }
   
}
