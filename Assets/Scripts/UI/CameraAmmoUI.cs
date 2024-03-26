using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraAmmoUI : MonoBehaviour
{
    public List<GameObject> bulbs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bulbs.Count; i++)
        {
            if (i >= CameraWeapon.instance.fFilmCount)
            {
                bulbs[i].gameObject.SetActive(false);
            }
            else
            {
                bulbs[i].gameObject.SetActive(true);
            }
              
        }
    }
}
