using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KMVGS.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocInput : MonoBehaviour, PlayerControls.IPlayerLocMapsActions
    {
        public PlayerControls PlayerControls {get;private set;}

        public Vector2 MovementInput {get;private set;}

        public Vector2 LookInput {get;private set;}
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocMaps.Enable();
            PlayerControls.PlayerLocMaps.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerLocMaps.Disable();
            PlayerControls.PlayerLocMaps.RemoveCallbacks(this);
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
    }
}

