using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpeech : MonoBehaviour
{
    public static UISpeech instance;
    private TMPro.TextMeshProUGUI speechText;

    public float wordStayLength;
    public float wordFade;
    private void Awake()
    {
        instance = this;
        speechText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Speak(string dialog, float timeBetweenLetters = 0.1f)
    {
        StopAllCoroutines();
        StartCoroutine(SpeakDelay(dialog, timeBetweenLetters));
    }

    private IEnumerator SpeakDelay(string dialog, float timeBetweenLetters)
    {
        char[] dialogletters = dialog.ToCharArray();
        speechText.text = string.Empty;
        speechText.color = new Color(speechText.color.r, speechText.color.b, speechText.color.g, 1f);

        foreach (char c in dialogletters) 
        {
            speechText.text += c;
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        yield return new WaitForSeconds(wordStayLength);

        float elapsedTime = 0;
        while(elapsedTime < wordFade)
        {
            elapsedTime += Time.deltaTime;
            speechText.color = new Color(speechText.color.r, speechText.color.b, speechText.color.g, 1f - (elapsedTime/wordFade));
            yield return null;
        }

    }
}
