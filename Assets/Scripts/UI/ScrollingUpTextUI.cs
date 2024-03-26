using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingUpTextUI : MonoBehaviour
{
    float textAlpha = 0f;
    public TMP_Text foundText;
    public string textToDisplay = "default";

    bool bRunOnce = false;

    private void Start()
    {
        textToDisplay = "+ " + textToDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        foundText.text = textToDisplay;

        if (!bRunOnce)
        {
            StartCoroutine(TextFadeInFadeOut());
            bRunOnce = true;
        }

        transform.position += new Vector3(0, Time.deltaTime/1, Time.deltaTime/2);
    }

    IEnumerator TextFadeInFadeOut()
    {
        float duration = 1f;

        // Fade In
        while (textAlpha < 1)
        {
            textAlpha += Time.deltaTime / duration;
            foundText.alpha = textAlpha;
            yield return null;
        }

        yield return new WaitForSeconds(duration / 2);

        // Fade Out
        while (textAlpha > 0)
        {
            textAlpha -= Time.deltaTime / duration;
            foundText.alpha = textAlpha;
            yield return null;
        }

        Destroy(gameObject);
    }

}
