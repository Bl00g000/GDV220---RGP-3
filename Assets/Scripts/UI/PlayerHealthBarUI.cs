using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarUI : MonoBehaviour
{
    //PlayerData playerData;
    //EnemyBase enemyBase;
    public GameObject healthCanvas;
    public MPImage healthBar;
    public bool bShowingHealthBar = true;

    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        //if(PlayerData.instance)
        //{
            PlayerData.instance.healthBarUI = this;
        //}
        HideCanvas();

        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(healthBar && PlayerData.instance)
        {

            if(bShowingHealthBar)
            {

                healthBar.fillAmount = 1f-(PlayerData.instance.fCurrentHealth / PlayerData.instance.fMaxHealth);

                //float fillAmount = Mathf.Clamp(1f - (PlayerData.instance.fCurrentHealth / PlayerData.instance.fMaxHealth),0,1f);
                //rect.anchoredPosition = new Vector3((-(rect.sizeDelta.x / 2f)) + (rect.sizeDelta.x * fillAmount), rect.anchoredPosition.y, 0);
            }
        }
    }

   public void ShowCanvas()
    {
        if (healthCanvas)
        {
            healthCanvas.SetActive(true);
        }
        bShowingHealthBar = true;
    }
    public void HideCanvas()
    {
       
        if (healthCanvas)
        {
            healthCanvas.SetActive(false);
        }
        bShowingHealthBar = false;
    }
}
