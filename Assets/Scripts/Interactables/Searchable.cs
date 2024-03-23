using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searchable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    public GameObject searchUI;

    public MPImage radialSearchImage;

    public float fSearchTime = 2.0f;
    public float fSearchSpeed = 1.0f;

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
            // enables the searchUI and slowly fills the radial image 
            searchUI.SetActive(true);
            fCurrentSearchTime += Time.deltaTime * fSearchSpeed;

            radialSearchImage.fillAmount = fCurrentSearchTime / fSearchTime;

            // if the interact key is held down for the entire searchtime duration
            // successfully searched the interactable
            if (fCurrentSearchTime >= fSearchTime)
            {
                // disable being able to search the item AGAIN
                bCanInteract = false;

                bInteracting = false;
                searchUI.SetActive(false);
                Debug.Log("Search Successful");
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
            Debug.Log("Search UnSuccessful");
        }

    }
}
