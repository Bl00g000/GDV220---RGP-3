using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameInteraction : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    //public GameObject EndGameUI;

    public GameObject scrollingTextPF;

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
            bCanInteract = false;
            UIInteractPrompt.instance.Interact(UIInteractPrompt.instance.testCorpse, Vector3.one, "A BODY...", "OH NO YOUR FRIEND IS DEAD!");
            UIInteractPrompt.instance.corpseEndGameButton.SetActive(true);
        }
    }

    public void CloseInteractWindow()
    {
        UIInteractPrompt.instance.Close();
        //EndGameUI.SetActive(true);
        //PlayerData.instance.AddHealthPillToInventory(1);
        // here is where you find the selected item

         var newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position, Quaternion.identity);
        newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "oh no he dead :(";

        Destroy(gameObject);
    }
}