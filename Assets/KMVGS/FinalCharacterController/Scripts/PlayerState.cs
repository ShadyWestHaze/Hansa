using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace KMVGS.FinalCharacterController 
{
    public enum PlayerMovementState
    {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Sprinting = 3,
        Jumping = 4,
        Falling = 5,
        Strafing = 6,
    }

    public class PlayerState : MonoBehaviour
    {
        [field:SerializeField] public PlayerMovementState CurrentPlayerMovementState {get;private set;} = PlayerMovementState.Idling;

        public void SetPlayerMovementState(PlayerMovementState playerMovementState)
        {
            CurrentPlayerMovementState = playerMovementState;
        }
    }
}