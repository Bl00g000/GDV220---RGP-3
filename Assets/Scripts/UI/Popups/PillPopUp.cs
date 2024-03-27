using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PillPopUp : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerData.instance.iHealthPills > 0 && PlayerData.instance.fCurrentHealth <= PlayerData.instance.fMaxHealth - PlayerData.instance.fHealthPillHeal)
        {
            text.enabled = true;
            if (Input.GetKey(KeyCode.R))
            {
                Destroy(gameObject);
            }
        }
    }
}
