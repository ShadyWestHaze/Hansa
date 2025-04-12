using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
namespace KMVGS.FinalCharacterController
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float locomotionBlendSpeed = 0.02f;

        private PlayerLocInput _playerLocInput;

        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");

        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocInput = GetComponent<PlayerLocInput>();
        }
    private void Update()
    {
      UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        Vector2 inputTarget = _playerLocInput.MovementInput;
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);
        _animator.SetFloat(inputXHash,inputTarget.x);
        _animator.SetFloat(inputYHash,inputTarget.y);
    }

  }
}
