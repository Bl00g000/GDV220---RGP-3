using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    //public PlayerHealthBarUI healthBarUI;

    public GameObject scrollingTextPF;

    public float fMaxHealth = 100.0f;
    public float fCurrentHealth = 0.0f;
    public bool bCanTakeDamage = true;

    public int iHealthPills = 0;

    public float fHealthPillHeal = 20.0f;

    public bool bShowingHealthBar = true;
    public bool bHealthIsChanging = false;


    private int iUICroutonCounter = 0;

    // Events

    public event Action<float> OnPlayerDamaged;

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
            var newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position + new Vector3(0.0f, 0.0f, 1.0f), Quaternion.identity);
            newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "+ Health Pills";
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

    public void TakeDamage(float _damage, bool _shouldLockoutDamage = true)
    {
        if(!bCanTakeDamage)
        {
            return;
        }
        //means that damage always subtracts hp
        if(_damage < 0)
        {
            _damage *= -1.0f;
        }

        //Does the damage
        fCurrentHealth -= _damage;

        // evokes event
        OnPlayerDamaged?.Invoke(_damage);

        if (_shouldLockoutDamage)
        {
            bCanTakeDamage = false;
            StartCoroutine(LockoutDamage());
        }

        //healthBarUI.ShowCanvas();
        //start coroutine
        //StartCoroutine(HideHealthBarCrouton());

        if (fCurrentHealth <= 0.0f)
        {
            //PlayerMovement.instance.canMove = false;
            GameManager.instance.GameOver(false);
        }
    }

    public void Heal(float _healthRestored)
    {
        //heal always heals
        if(_healthRestored < 0)
        {
            _healthRestored *= -1.0f;
        }

        //does the healing
        fCurrentHealth += _healthRestored;
        //healthBarUI.ShowCanvas();
        //start coroutine
        //StartCoroutine(HideHealthBarCrouton());
        //UI effect?

        var newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position + new Vector3(0.0f, 0.0f, 1.0f), Quaternion.identity);
        newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "- Health Pills";


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
   public void AddHealthPillToInventory(int _quantity)
    {
        iHealthPills += _quantity;
        //UI?
        ////sound?
    }

    void LerpHealthChange()
    {

    }

 //timer to hide health bar
    //IEnumerator HideHealthBarCrouton()
    //{
    //    iUICroutonCounter++;
    //    yield return new WaitForSeconds(2.0f);
    //    iUICroutonCounter--;
    //    if(iUICroutonCounter == 0)
    //    {
    //        healthBarUI.HideCanvas();
    //    }
    //}

    //timer to allow damage again
    IEnumerator LockoutDamage()
    {
        yield return new WaitForSeconds(1.2f);
        bCanTakeDamage = true;
    }
}
