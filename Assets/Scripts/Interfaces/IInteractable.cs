using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    GameObject interactUI { get; set; }
    bool bCanInteract { get; set; }
    bool bInteracting { get; set; }

    // This is what happens when the player interacts with the object
    void Interact();
}
