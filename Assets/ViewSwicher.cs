using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera _firstPersonCamera;
    [SerializeField] private Camera _thirdPersonCamera;
    [SerializeField] private KeyCode _switchKey = KeyCode.V;

    [Header("Optional Player Reference")]
    [SerializeField] private GameObject _player; // For syncing position if needed
    [SerializeField] private bool _syncPlayerPosition = false;

    private void Start()
    {
        // Initialize cameras - enable one, disable the other
        _firstPersonCamera.gameObject.SetActive(true);
        _thirdPersonCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_switchKey))
        {
            if (_firstPersonCamera.gameObject.activeSelf)
            {
                SwitchToThirdPerson();
            }
            else
            {
                SwitchToFirstPerson();
            }
        }
    }

    private void SwitchToFirstPerson()
    {
        _thirdPersonCamera.gameObject.SetActive(false);
        _firstPersonCamera.gameObject.SetActive(true);
        
        if (_syncPlayerPosition && _player != null)
        {
            // If your third-person camera has an offset, you might need to adjust player position
            _player.transform.position = _thirdPersonCamera.transform.position;
        }
    }

    private void SwitchToThirdPerson()
    {
        _firstPersonCamera.gameObject.SetActive(false);
        _thirdPersonCamera.gameObject.SetActive(true);
        
        if (_syncPlayerPosition && _player != null)
        {
            // Position third person camera relative to player
            _thirdPersonCamera.transform.position = _player.transform.position;
        }
    }
}