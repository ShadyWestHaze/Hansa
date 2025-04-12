using UnityEngine;

namespace KMVGS.FinalCharacterController
{
    public class Interactable : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float throwForce = 5f;
        [SerializeField] private float rotationSpeedWhenHeld = 30f;
        [SerializeField] private bool canBePickedUp = true;
        
        private Rigidbody rb;
        private Collider col;
        private bool isBeingHeld = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
        }

        private void Update()
        {
            if (isBeingHeld)
            {
                // Optional: Add slight rotation when held
                transform.Rotate(Vector3.up, rotationSpeedWhenHeld * Time.deltaTime);
            }
        }

        public void OnPickup(Transform holdPosition)
        {
            if (!canBePickedUp) return;
            
            transform.SetParent(holdPosition);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            if (col != null)
            {
                col.enabled = false;
            }
            
            isBeingHeld = true;
        }

        public void OnDrop()
        {
            transform.SetParent(null);
            
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }
            
            if (col != null)
            {
                col.enabled = true;
            }
            
            isBeingHeld = false;
        }

        // For future NPC interactions
        public virtual void OnInteract(PlayerInteractor interactor)
        {
            // Base interaction logic
            if (canBePickedUp)
            {
                if (!isBeingHeld)
                {
                    interactor.PickUpItem(this);
                }
            }
        }

        public bool CanBePickedUp() => canBePickedUp;
        public void SetPickupable(bool state) => canBePickedUp = state;
    }
}