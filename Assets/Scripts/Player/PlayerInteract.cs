using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Collider[] nearbyCollisions;
    public float fInteractRange = 1.5f;
    [HideInInspector] public GameObject interactableObject = null;
    public KeyCode interactKey = KeyCode.F;

    public static PlayerInteract instance;

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

    // Update is called once per frame
    void Update()
    {
        GetNearbyInteractables();
        Interact();
    }

    void GetNearbyInteractables()
    {
        // reset the interactableObject every tick
        interactableObject = null;

        float fClosestObjectDistance = fInteractRange;

        // get all collisions within interact range
        nearbyCollisions = Physics.OverlapSphere(gameObject.GetComponentInParent<PlayerMovement>().transform.position, fInteractRange);

        // check through all colliders in Interact Range
        foreach (Collider _collider in nearbyCollisions)
        {
            // get distance from player to the current collider
            float fDistanceFromCurrentInteractable = Vector3.Distance(PlayerMovement.instance.transform.position, _collider.ClosestPoint(PlayerMovement.instance.transform.position));

            // check for IInteractable interface as well as if the interactable is currently being interacted with OR is interactable at all anymore
            if (_collider.gameObject.GetComponent<IInteractable>() != null && _collider.gameObject.GetComponent<IInteractable>().bCanInteract && !_collider.gameObject.GetComponent<IInteractable>().bInteracting)
            {
                // check if the object is the closest object to the player
                if (fDistanceFromCurrentInteractable < fClosestObjectDistance)
                {
                    interactableObject = _collider.gameObject;

                    fClosestObjectDistance = Vector3.Distance(PlayerMovement.instance.transform.position, interactableObject.transform.position);
                }
            }
        }

        if (interactableObject != null)
        {
            interactableObject.GetComponent<IInteractable>().interactUI.SetActive(true);
        }
    }

    void Interact()
    {
        if (interactableObject != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interactableObject.GetComponent<IInteractable>().Interact();
            }
        }
    }

}
