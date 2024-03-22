using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Collider[] nearbyCollisions;
    public float fInteractRange = 1.5f;
    GameObject interactableObject = null;
    public KeyCode interactKey = KeyCode.F;

    // Start is called before the first frame update
    void Start()
    {
        
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

            // Check for the closest object that has a IInteractable interface attached
            // AND insure it is the closest IInteractable
            if (_collider.gameObject.GetComponent<IInteractable>() != null && fDistanceFromCurrentInteractable < fClosestObjectDistance)
            {
                interactableObject = _collider.gameObject;

                fClosestObjectDistance = Vector3.Distance(PlayerMovement.instance.transform.position, interactableObject.transform.position);
            }
        }

        if (interactableObject != null)
        {
            
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
