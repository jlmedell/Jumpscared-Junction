using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] public GameObject generalEmpty;
    [SerializeField] public GameObject audioEmpty;
    [SerializeField] public GameObject controlsEmpty;
    [SerializeField] public GameObject accessiblityEmpty;
    [SerializeField] public GameObject mainMenuEmpty;
    [SerializeField] public GameObject settingsEmpty;

    [Header("Settings Buttons")]
    [SerializeField] private Button generalButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button accessiblyButton;
    [SerializeField] private Button returnButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction;

    private void Start()
    {
        generalButton.onClick.AddListener(openGeneralEmpty);
        audioButton.onClick.AddListener(openAudioEmpty);
        controlsButton.onClick.AddListener(openControlEmpty);
        accessiblyButton.onClick.AddListener(openAccessiblyEmpty);
        returnButton.onClick.AddListener(openMainMenuEmpty);
    }
    private void AutoSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        generalButton.Select();
        EventSystem.current.SetSelectedGameObject(generalButton.gameObject);
    }

    private void OnEnable()
    {
        if (submitAction != null)
        {
            submitAction.action.performed += OnSubmit;
            submitAction.action.Enable();
        }

        AutoSelect();
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

    public void openGeneralEmpty()
    {
        generalEmpty.SetActive(true);
        settingsEmpty.SetActive(false);
    }
    public void openAudioEmpty()
    {
        audioEmpty.SetActive(true);
        settingsEmpty.SetActive(false);
    }
    public void openControlEmpty()
    {
        controlsEmpty.SetActive(true);
        settingsEmpty.SetActive(false);
    }
    public void openAccessiblyEmpty()
    {
        accessiblityEmpty.SetActive(true);
        settingsEmpty.SetActive(false);
    }
    public void openMainMenuEmpty()
    {
        mainMenuEmpty.SetActive(true);
        settingsEmpty.SetActive(false);
    }
    private void OnDestroy()
    {
        generalButton.onClick.RemoveListener(openGeneralEmpty);
        audioButton.onClick.RemoveListener(openAudioEmpty);
        controlsButton.onClick.RemoveListener(openControlEmpty);
        accessiblyButton.onClick.RemoveListener(openAccessiblyEmpty);
        returnButton.onClick.RemoveListener(openMainMenuEmpty);
    }
}