using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph.Internal;
using System;

namespace KMVGS.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
    #region Class Variables
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;
        [Header("3rd-Person Camera")]
        public bool isThirdPerson = false; // Toggle between 1st/3rd-person
        public float thirdPersonDistance = 5f; // Distance behind player
        public float thirdPersonHeight = 2f; // Height offset
        public float followSmoothness = 5f; // Smoothing speed
        
        [Header("Basic Movement")]
        public float runAcceleration = 0.25f;
        public float runSpeed = 4f;
        public float drag = 0.1f;
        public float movingThreshold = 0.01f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerLocInput _playerLocInput;
        private PlayerState _playerState;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
      #endregion
    
    #region Startup
    private void Awake()
    {
      _playerLocInput = GetComponent<PlayerLocInput>();
      _playerState = GetComponent<PlayerState>();
      _playerLocInput.Player.SwitchCamera.performed += ctx => ToggleCameraMode();
    }
    #endregion
    
    #region Update Logic
    private void Update()
    {
      UpdateMovementState();
      HandleLateralMovement();
    }
    #endregion
    
    #region Late Update Logic
private void LateUpdate()
{
    // Handle camera rotation (unchanged)
    _cameraRotation.x += lookSenseH * _playerLocInput.LookInput.x;
    _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocInput.LookInput.y, -lookLimitV, lookLimitV);
    _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocInput.LookInput.x;
    transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
    
    // Apply rotation to camera
    _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

    // 3rd-Person Position Logic
    if (isThirdPerson)
    {
        Vector3 desiredPosition = transform.position - _playerCamera.transform.forward * thirdPersonDistance;
        desiredPosition.y += thirdPersonHeight;

        // Optional: Wall collision avoidance
        if (Physics.SphereCast(transform.position, 0.3f, (desiredPosition - transform.position).normalized, out RaycastHit hit, thirdPersonDistance))
        {
            desiredPosition = hit.point + hit.normal * 0.3f; // Adjust to avoid clipping
        }

        // Smooth follow
        _playerCamera.transform.position = Vector3.Lerp(_playerCamera.transform.position, desiredPosition, followSmoothness * Time.deltaTime);
    }
    else // 1st-Person: Camera stays at player's head
    {
        _playerCamera.transform.localPosition = Vector3.zero; // Reset to default (adjust if needed)
    }
}    
#endregion
    
    #region State Checks
    private void UpdateMovementState()
    {
      bool isMovementInput = _playerLocInput.MovementInput == Vector2.zero;
      bool isMovingLaterally = IsMovingLaterally();

      PlayerMovementState lateralState = isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
      _playerState.SetPlayerMovementState(lateralState);
    }
    #endregion
    private void HandleLateralMovement()
    {
      Vector3 cameraFowardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
      Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
      Vector3 movementDirection = cameraRightXZ * _playerLocInput.MovementInput.x + cameraFowardXZ * _playerLocInput.MovementInput.y;
      Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
      Vector3 newVelocity = _characterController.velocity + movementDelta;

      Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
      newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
      newVelocity = Vector3.ClampMagnitude(newVelocity,runSpeed);

      _characterController.Move(newVelocity * Time.deltaTime);
    }
        private void OnEnable()
    {
        _playerLocInput.SwitchCamera.performed += ToggleCameraMode;
    }

    private void OnDisable()
    {
        _playerLocInput.SwitchCamera.performed -= ToggleCameraMode;
    }

    private void ToggleCameraMode(InputAction.CallbackContext context)
    {
        isThirdPerson = !isThirdPerson;
    }

    private bool IsMovingLaterally()
    {
      Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.y);
      return lateralVelocity.magnitude > movingThreshold;
    }
  }   
}