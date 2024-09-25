using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // Cursor Lock variables
    public bool isCursorLocked = true;
    public bool isNewScene;

    // POV Camera variables
    public float sensitivity = 1f;
    public Transform player;  // Reference to the player's body (for rotating)
    private float pitch = 0f;

    // Player Interaction variables
    public LayerMask interactableLayer;
    public Transform playerCamera;
    public float interactRange = 1.5f;
    private Ray rayOrigin;
    public GameObject currentInteractableObject;
    public GameObject currentPutdownObject;
    public bool canPickup = false;
    public bool canPutdown = false;

    public bool canOpenShop = false;
    public SceneManager sceneManager;

    public Pickup hasItem;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI putdownText;
    public TextMeshProUGUI shopText;

    private void Start()
    {
        if (pickupText != null)
        {
            pickupText.enabled = false;
        }
        if (putdownText != null)
        {
            putdownText.enabled = false;
        }
        if (shopText != null)
        {
            shopText.enabled = false;
        }
    }
    void Update()
    {
        // Handle First Person Camera Movement
        HandleCameraMovement();

        // Handle Player Interaction
        HandlePlayerInteraction();
    }

    // Handle camera rotation based on mouse movement
    void HandleCameraMovement()
    {
        if (isCursorLocked)
        {
            float lookX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float lookY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            player.Rotate(Vector3.up * lookX);

            pitch -= lookY;
            pitch = Mathf.Clamp(pitch, -45f, 60f);
            playerCamera.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }

    // Handle player interaction with objects
    void HandlePlayerInteraction()
    {
        // Cast a ray from the center of the screen
        rayOrigin = playerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of screen
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, out hit, interactRange, interactableLayer))
        {
            // Handle interaction with Pickup objects
            if (hit.collider.CompareTag("Pickup") && !hasItem.hasItem)
            {
                canPickup = true;
                currentInteractableObject = hit.collider.gameObject;
                pickupText.enabled = true;
                putdownText.enabled = false;
            }
            else
            {
                canPickup = false;
                currentInteractableObject = null;
                pickupText.enabled = false;
            }

            // Handle interaction with PutDown objects
            if (hit.collider.CompareTag("PutDown") && hasItem.hasItem)
            {
                canPutdown = true;
                currentPutdownObject = hit.collider.gameObject;
                putdownText.enabled = true;
                pickupText.enabled = false;
            }
            else
            {
                canPutdown = false;
                currentPutdownObject = null;
                putdownText.enabled = false;
            }
            if (hit.collider.CompareTag("Shop"))
            {
                canOpenShop = true;
                shopText.enabled = true;
                if(Input.GetKeyDown("e"))
                {
                      SceneManager.LoadScene("Shop");
                }


            }
            else
            {
                canOpenShop = false;

            }
            Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * interactRange, Color.blue);
        }
        else
        {
            canPickup = false;
            canPutdown = false;
            currentInteractableObject = null;
            currentPutdownObject = null;
            pickupText.enabled = false;
            putdownText.enabled = false;
            shopText.enabled = false;
            Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * interactRange, Color.green);
        }
    }
}
