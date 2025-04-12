using UnityEngine;
using UnityEngine.InputSystem;

namespace KMVGS.FinalCharacterController
{
    [RequireComponent(typeof(PlayerLocInput))]
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private Transform holdPosition;
        
        private PlayerLocInput input;
        private Camera playerCamera;
        private Interactable currentInteractable;
        private bool isHoldingItem = false;

        private void Awake()
        {
            input = GetComponent<PlayerLocInput>();
            playerCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            HandleInteractionInput();
            UpdateHeldItemPosition();
        }

        private void HandleInteractionInput()
        {
            // Check for new interaction
            if (input.InteractPressed)
            {
                if (!isHoldingItem)
                {
                    TryInteract();
                }
                else
                {
                    ReleaseHeldItem();
                }
            }

            // Check for dropping held item
            if (input.InteractReleased && isHoldingItem)
            {
                ReleaseHeldItem();
            }
        }

        private void TryInteract()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    PickUpItem(currentInteractable);
                }
            }
        }

        // Change from private to public
        public void PickUpItem(Interactable item)
        {
            if (item == null) return;
            
            item.OnPickup(holdPosition);
            isHoldingItem = true;
            currentInteractable = item;
        }

        private void ReleaseHeldItem()
        {
            if (currentInteractable == null) return;
            
            currentInteractable.OnDrop();
            currentInteractable = null;
            isHoldingItem = false;
        }

        private void UpdateHeldItemPosition()
        {
            if (isHoldingItem && currentInteractable != null)
            {
                currentInteractable.transform.position = holdPosition.position;
            }
        }

        // For future NPC interactions
        public bool IsHoldingItem() => isHoldingItem;
        public Interactable GetHeldItem() => currentInteractable;
    }
}