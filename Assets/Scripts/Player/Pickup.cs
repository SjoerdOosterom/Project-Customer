using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject myHands; // reference to your hands (position where object goes)
    public bool canPickup = false; // flag to check if you can pick up
    public GameObject heldItem; // reference to the currently held item
    public bool hasItem = false; // flag to check if you have an item in hand
    public PlayerController playerInteract;  // Reference to the PlayerInteract script
    public Vector3 originalScale;

    void Update()
    {
        Debug.Log("has item" + hasItem);
        // If the player is looking at an interactable object and presses "e", pick it up
        if (playerInteract.canPickup && playerInteract.currentInteractableObject != null && !hasItem)
        {
            GameObject ObjectToPickUp = playerInteract.currentInteractableObject;  // Reference to the object to pick up
            Debug.Log("Can Pickup: " + ObjectToPickUp.name);

            if (Input.GetKeyDown("e"))
            {
                Debug.Log("eeeeeeeeee");
                // Pick up the object
                PickUpItem(ObjectToPickUp);
            }
        }
    }

    private void PickUpItem(GameObject ObjectToPickUp)
    {
        // Store the original scale
        originalScale = ObjectToPickUp.transform.localScale;

        // Disable physics and collider on the object
        ObjectToPickUp.GetComponent<Rigidbody>().isKinematic = true;
        ObjectToPickUp.GetComponent<Collider>().enabled = false;

        // Set the object as the currently held item
        heldItem = ObjectToPickUp;

        // Parent the object to the player's hand
        heldItem.transform.SetParent(myHands.transform, false);

        // Reset the object's position and rotation
        PickedupRotation interactableObject = heldItem.GetComponent<PickedupRotation>();
        if (interactableObject != null)
        {
            heldItem.transform.localPosition = interactableObject.positionOffset;
            heldItem.transform.localRotation = Quaternion.Euler(interactableObject.rotationOffset);
        }
        else
        {
            heldItem.transform.localPosition = new Vector3(0, 0, 0.55f);
            heldItem.transform.localRotation = Quaternion.identity;  // Default rotation
        }

        // Restore the original scale
        heldItem.transform.localScale = originalScale;

        hasItem = true;  // Mark that the player is holding an item
        Debug.Log("Picked up: " + heldItem.name);
    }
}