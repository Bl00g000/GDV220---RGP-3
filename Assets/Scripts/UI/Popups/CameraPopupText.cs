using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraPopupText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private float elapsedTime;

    private void Start()
    {
        elapsedTime = 0;
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = false;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (CameraWeapon.instance.bHasCamera)
        {
            elapsedTime += Time.deltaTime;
            text.enabled = true;

            if (elapsedTime > 5f || Input.GetMouseButtonDown(0))
            {
                Destroy(gameObject);
            }
        }
    }
}
