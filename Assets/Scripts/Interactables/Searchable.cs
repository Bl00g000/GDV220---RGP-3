using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Searchable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    public GameObject searchUI;

    public GameObject objectCanvas;

    public GameObject scrollingTextPF;

    public MPImage radialSearchImage;

    [Header("items that can be found")]
    public bool bHasCamAmmo = false;
    public bool bHasHealth = false;

    [Header("% chance to find items (only if respective item is ticked above)")]
    [Range(0f, 1f)]
    public float fCamAmmoChance = 0;
    [Range(0f, 1f)]
    public float fHealthChance = 0;

    [Header("Searching")]
    public float fSearchTime = 2.0f;
    float fSearchSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // if this object isn't the current closest object to the player OR the interact option is in progress disable canvas
        if (PlayerInteract.instance.interactableObject != gameObject)
        {
            interactUI.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!bInteracting)
        {
            StartCoroutine(Search());
        }
    }

    IEnumerator Search()
    {
        bInteracting = true;
        float fCurrentSearchTime = 0;

        // checks that the player is holding down interact key
        while (!Input.GetKeyUp(PlayerInteract.instance.interactKey))
        {
            bCanInteract = false;
            // enables the searchUI and slowly fills the radial image 
            searchUI.SetActive(true);
            fCurrentSearchTime += Time.deltaTime * fSearchSpeed;

            radialSearchImage.fillAmount = fCurrentSearchTime / fSearchTime;

            // if the interact key is held down for the entire searchtime duration
            // successfully searched the interactable
            if (fCurrentSearchTime >= fSearchTime)
            {
                bInteracting = false;
                searchUI.SetActive(false);

                if (bHasCamAmmo)
                {
                    float chanceToFind = Random.Range(0f, 1f);

                    if (chanceToFind < fCamAmmoChance)
                    {
                        GameObject newFoundText;
                        // here is where you find the selected item
                        if (CameraWeapon.instance.fFilmCount < 3)
                        {
                            CameraWeapon.instance.fFilmCount++;

                            bHasCamAmmo = false;
                            newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position, Quaternion.identity);
                            newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "+ Flash Bulb";
                            yield return new WaitForSeconds(1f);
                        }
                        else
                        {
                            newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position, Quaternion.identity);
                            newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "flash bulb inventory full";
                            yield return new WaitForSeconds(1f);
                        }
                    }
                    else
                    {
                        bHasCamAmmo = false;
                    }
                }

                if (bHasHealth)
                {
                    float chanceToFind = Random.Range(0f, 1f);

                    if (chanceToFind < fCamAmmoChance)
                    {
                        if (PlayerData.instance.iHealthPills < 3)
                        {
                            bHasHealth = false;
                            // here is where you find the selected item

                            PlayerData.instance.iHealthPills++;

                            var newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position, Quaternion.identity);
                            newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "+ Health Pills";
                            yield return new WaitForSeconds(1f);
                        }
                        else
                        {
                            // here is where you find the selected item
                            var newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position, Quaternion.identity);
                            newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "Health Pills inventory full";
                            yield return new WaitForSeconds(1f);
                        }


                    }
                    else
                    {
                        bHasHealth = false;
                    }
                }

                if (bHasHealth || bHasCamAmmo)
                {
                    // disable being able to search the item AGAIN
                    bCanInteract = true;
                }
                break;
            }
            yield return null;
        }

        // if the interact key is let go of before the entire searchtime duration
        // search is cancelled
        if (fCurrentSearchTime <= fSearchTime)
        {
            bInteracting = false;
            searchUI.SetActive(false);

            if (bHasHealth || bHasCamAmmo)
            {
                // disable being able to search the item AGAIN
                bCanInteract = true;
            }

            // if you cancelled the search
            // maybe do something here?
        }

    }
}
