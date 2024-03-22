using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public GameObject objectsCanvas { get; set; }

    public float fOpenDuration = 1f;

    public float fOpenAngle = -90f;
   
    bool bIsOpening = false;
    bool bInitialInteraction = true;

    public void Interact()
    {
        if (!bIsOpening)
        {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        bIsOpening = true;

        float fElapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;

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
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, fElapsedTime / fOpenDuration);
            fElapsedTime += Time.deltaTime;
            yield return null;
        }

        // ensure the target rotation is set properly at the end of the loop
        transform.rotation = targetRotation;
        bIsOpening = false;

        // flip bClockwise at end of interaction
        bInitialInteraction = !bInitialInteraction;
    }
}

