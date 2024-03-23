using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    public GameObject doorToRotate;

    public float fOpenDuration = 1f;

    public float fOpenAngle = -90f;
   
    bool bInitialInteraction = true;

    private void Update()
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
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        bInteracting = true;

        float fElapsedTime = 0f;
        Quaternion initialRotation = doorToRotate.transform.rotation;

        float fOpenMultipler;

        // sets the angle either to the same angle as the initialInteraction to open/close
        // otherwise flip it with -1 multiplier to do the opposite action of the initial action
        // (e.g. if door starts open and then closes with initialInteraction, the flip would be opening)
        if (bInitialInteraction) { fOpenMultipler = 1; }
        else { fOpenMultipler = -1;}

        // set the rotation target by using the above multipler and multiplying it with the angle
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, fOpenAngle * fOpenMultipler, 0);

        while (fElapsedTime < fOpenDuration)
        {
            doorToRotate.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, fElapsedTime / fOpenDuration);
            fElapsedTime += Time.deltaTime;
            yield return null;
        }

        // ensure the target rotation is set properly at the end of the loop
        doorToRotate.transform.rotation = targetRotation;
        bInteracting = false;

        // flip bClockwise at end of interaction
        bInitialInteraction = !bInitialInteraction;
    }
}

