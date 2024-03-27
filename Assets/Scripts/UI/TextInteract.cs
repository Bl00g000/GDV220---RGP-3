using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteract : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    public string text;

    private void Update()
    {
        if (PlayerInteract.instance.interactableObject != gameObject)
        {
            interactUI.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!bInteracting)
        {
            DisplayText();
        }
    }

    public void DisplayText()
    {
        UISpeech.instance.Speak(text);
        bCanInteract = false;
    }
}
