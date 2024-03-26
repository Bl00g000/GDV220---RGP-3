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

    // Start is called before the first frame update
    void Start()
    {
        //if(PlayerData.instance)
        //{
            PlayerData.instance.healthBarUI = this;
        //}
        HideCanvas();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(healthBar && PlayerData.instance)
        {

            if(bShowingHealthBar)
            {

                healthBar.fillAmount = (PlayerData.instance.fCurrentHealth / PlayerData.instance.fMaxHealth);

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
