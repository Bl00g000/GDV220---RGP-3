using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public PlayerHealthBarUI healthBarUI;

    public float fMaxHealth = 100.0f;
    public float fCurrentHealth = 0.0f;

    public int iHealthPills = 0;

    public float fHealthPillHeal = 20.0f;

    public bool bShowingHealthBar = true;
    public bool bHealthIsChanging = false;

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //healthBarUI = GetComponent<PlayerHealthBarUI>();
        fCurrentHealth = fMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //TEST DAMAGE ON Y PRESS
        if(Input.GetKeyDown("y"))
        {
            TakeDamage(5);
        }
        //TEST addign health pill on U PRESS
        if (Input.GetKeyDown("u"))
        {
            AddHealthPillToInventory(1);
        }

        //use health pill on spacebar
        if (Input.GetKeyDown("r"))
        {
            if(fCurrentHealth < fMaxHealth && iHealthPills > 0)
            {
                UseHealthPill();
            }
        }


    }

    void TakeDamage(float _damage)
    {
        //means that damage always subtracts hp
        if(_damage < 0)
        {
            _damage *= -1.0f;
        }

        //Does the damage
        fCurrentHealth -= _damage;
        healthBarUI.ShowCanvas();
        //start coroutine


        if (fCurrentHealth <= 0.0f)
        {
            //PlayerMovement.instance.canMove = false;
            GameManager.instance.GameOver();
        }
    }

    void Heal(float _healthRestored)
    {
        //heal always heals
        if(_healthRestored < 0)
        {
            _healthRestored *= -1.0f;
        }

        //does the healing
        fCurrentHealth += _healthRestored;
        healthBarUI.ShowCanvas();
        //start coroutine
        //UI effect?

        if (fCurrentHealth > fMaxHealth)
        {
            fCurrentHealth = fMaxHealth;
        }
    }

    //Uses health pill
    void UseHealthPill()
    {
        iHealthPills--;
        Heal(fHealthPillHeal);

        //Sound effect
        //UI effect?
    }

    //Adds health pill to inventory
    void AddHealthPillToInventory(int _quantity)
    {
        iHealthPills += _quantity;
        //UI?
        ////sound?
    }

    void LerpHealthChange()
    {

    }

   // IEnumerator HideHealthBarCoroutine()
   // {
   //     
   // }
   //
}