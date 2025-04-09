using UnityEngine;

public class ViewSwitcher : MonoBehaviour
{
    [Header("Controller Prefabs")]
    public GameObject fpsController;
    public GameObject tpsController;

    [Header("Switch Key")]
    public KeyCode switchKey = KeyCode.V;

    private bool isThirdPerson = false;

    void Start()
    {
        // Initialize with FPS view
        fpsController.SetActive(true);
        tpsController.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isThirdPerson = !isThirdPerson; // Toggle state
            
            fpsController.SetActive(!isThirdPerson);
            tpsController.SetActive(isThirdPerson);
        }
    }
}