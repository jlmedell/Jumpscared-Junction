using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitlePage : MonoBehaviour
{

    /*
        1. The action reference
        2. Subscribe to event
        3. Enable the action
    */

    [SerializeField] private string nextScene = "Main Menu";
    [SerializeField] private InputActionReference anyButtonContinue; // 1

    private void OnEnable()
    {
        anyButtonContinue.action.performed += OnAnyButtonPerformed; // 2
        anyButtonContinue.action.Enable(); // 3
    }

    private void OnDisable()
    {
        anyButtonContinue.action.performed -= OnAnyButtonPerformed;
        anyButtonContinue.action.Disable();
    }

    private void OnAnyButtonPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(nextScene);
    }
}