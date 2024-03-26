using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PillCountUI : MonoBehaviour
{
    public List<GameObject> pills = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pills.Count; i++)
        {
            if (i >= PlayerData.instance.iHealthPills)
            {
                pills[i].gameObject.SetActive(false);
            }
            else
            {
                pills[i].gameObject.SetActive(true);
            }
              
        }
    }
}
