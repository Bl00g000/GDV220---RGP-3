using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechTrigger : MonoBehaviour
{
    public string text;
    public float timeBetweenLetters;
    public bool deleteAfterMessage;

    public GameObject trigger;

    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (trigger != null)
        {

        }

        if (other.transform.root == PlayerData.instance.transform)
        {
            UISpeech.instance.Speak(text, timeBetweenLetters);
            if (deleteAfterMessage) Destroy(gameObject);
        }
    }
}
