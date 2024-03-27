using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject interactUI { get; set; }
    [field: SerializeField] public bool bCanInteract { get; set; } = true;
    [field: SerializeField] public bool bInteracting { get; set; } = false;

    public GameObject doorToRotate;

    NavMeshObstacle obstacle;

    TMP_Text interactText;

    public float fOpenDuration = 1f;
    float fNavObstacleChangeTime;

    public float fOpenAngle = -90f;

    [Tooltip("if door starts open set this to false")]
    public bool bInitialInteraction = true;

    [Header("Audio")]
    public AudioSource doorOpen;
    public AudioSource doorClose;

    private void Start()
    {
        fNavObstacleChangeTime = fOpenDuration / 2;
        interactText = gameObject.GetComponentInChildren<TMP_Text>();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
    }

    private void Update()
    {
        if (Flashlight.instance.bHasFlashLight)
        {
            bCanInteract = true;
        }
        else
        {
            bCanInteract = false;
        }

        // if this object isn't the current closest object to the player OR the interact option is in progress disable canvas
        if (PlayerInteract.instance.interactableObject != gameObject)
        {
            interactUI.SetActive(false);
        }

        if (bInitialInteraction) { interactText.text = "(F) Open Door"; }
        else { interactText.text = "(F) Close Door"; }
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

        yield return new WaitForSeconds(0);

        float fElapsedTime = 0f;
        Quaternion initialRotation = doorToRotate.transform.rotation;

        float fOpenMultipler;

        // sets the angle either to the same angle as the initialInteraction to open/close
        // otherwise flip it with -1 multiplier to do the opposite action of the initial action
        // (e.g. if door starts open and then closes with initialInteraction, the flip would be opening)
        if (bInitialInteraction) { fOpenMultipler = 1; doorOpen.Play(); }
        else { fOpenMultipler = -1; doorClose.Play(); }

        // set the rotation target by using the above multipler and multiplying it with the angle
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, fOpenAngle * fOpenMultipler, 0);

        bool bNavmeshChange = true;

        while (fElapsedTime < fOpenDuration)
        {
            doorToRotate.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, fElapsedTime / fOpenDuration);

            if (fElapsedTime > fNavObstacleChangeTime && bNavmeshChange)
            {
                if (bInitialInteraction) { obstacle.carving = false; }
                else { obstacle.carving = true; }

                bNavmeshChange = false;
            }

            fElapsedTime += Time.deltaTime;
            yield return null;
        }

        // flip bClockwise at end of interaction
        bInitialInteraction = !bInitialInteraction;

        // ensure the target rotation is set properly at the end of the loop
        doorToRotate.transform.rotation = targetRotation;
        bInteracting = false;
    }
}

