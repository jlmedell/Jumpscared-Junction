using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] public GameObject mainMenuEmpty;
    [SerializeField] public GameObject creditEmpty;

    [Header("Menu Buttons")]
    [SerializeField] private Button returnButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction;

    private bool isCreditEmpty = true;
    private bool isMainMenuEmpty = false;

    private void Start()
    {
        returnButton.onClick.AddListener(openMainMenuEmpty);
        returnButton.Select();
    }

    private void OnEnable()
    {
        if (submitAction != null)
        {
            submitAction.action.performed += OnSubmit;
            submitAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (submitAction != null)
        {
            submitAction.action.performed -= OnSubmit;
            submitAction.action.Disable();
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (currentButton != null)
            {
                currentButton.onClick.Invoke();
            }
        }
    }

    public void openMainMenuEmpty()
    {
        mainMenuEmpty.SetActive(true);
        creditEmpty.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        returnButton.onClick.RemoveListener(openMainMenuEmpty);
    }
}