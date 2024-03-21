using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    EnemyBase enemyBase;
    public GameObject healthCanvas;
    public MPImage healthBar;

    // Start is called before the first frame update
    void Start()
    {
        enemyBase = transform.GetComponent<EnemyBase>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyBase.fHealth < enemyBase.fMaxHealth)
        {
            healthCanvas.SetActive(true);
        }

        enemyBase = transform.GetComponent<EnemyBase>();
        healthBar.fillAmount = (enemyBase.fHealth / enemyBase.fMaxHealth);
    }
}
