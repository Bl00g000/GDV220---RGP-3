using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISearchingText : MonoBehaviour
{
    TMPro.TextMeshProUGUI searchText;
    public float textSpeed;

    public int maxDots = 3;

    private int currentDots = 0;
    private float elaspedTime;
    // Start is called before the first frame update
    void Start()
    {
        searchText = GetComponent<TMPro.TextMeshProUGUI>();
        searchText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        elaspedTime += Time.deltaTime;

        if (elaspedTime < textSpeed) return;

        elaspedTime = 0;
        if (currentDots < maxDots)
        {
            searchText.text += ".";
            currentDots++;
        }
        else 
        {
            searchText.text = string.Empty;
            currentDots = 0;
        }
    }
}
