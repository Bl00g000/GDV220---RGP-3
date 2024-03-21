using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class FPSUI : MonoBehaviour
{
    private TMPro.TextMeshProUGUI textBox;
    private Coroutine timeCoroutine;
    private bool coroutineRunning = false;
    // Start is called before the first frame update

    private void Awake()
    {
        Application.targetFrameRate = -1;
        textBox = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!coroutineRunning)
        {
            timeCoroutine = StartCoroutine(ShowFPS());
        }
    }

    public IEnumerator ShowFPS()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(0.2f);
        textBox.text = (1.0f / Time.deltaTime).ToString("F0");
        coroutineRunning = false;
    }
}
