using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPickup : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    public GameObject cameraUI;

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
            UIInteractPrompt.instance.Interact(UIInteractPrompt.instance.testCamera, Vector3.one, "Camera", "SAY CHEESE! :3");
            UIInteractPrompt.instance.cameraPickupButton.SetActive(true);
        }
    }

    public void CloseInteractWindow()
    {
        UIInteractPrompt.instance.Close();
        cameraUI.SetActive(true);
        CameraWeapon.instance.bHasCamera = true;

        var newFoundText = Instantiate(scrollingTextPF, gameObject.transform.position, Quaternion.identity);
        newFoundText.GetComponent<ScrollingUpTextUI>().textToDisplay = "Camera";

        Destroy(gameObject);
    }
}
