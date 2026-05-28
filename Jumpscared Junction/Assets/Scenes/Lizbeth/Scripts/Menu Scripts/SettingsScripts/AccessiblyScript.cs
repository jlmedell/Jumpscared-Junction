using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AccessiblyScript : MonoBehaviour
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

    [Header("General Buttons")]
    [SerializeField] private Button onButton;
    [SerializeField] private Button offButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction;

    private Button selectedLanguageButton;
    private ColorBlock originalEnglishColors;
    private ColorBlock originalSpanishColors;
    private ColorBlock originalLanguageColors;

    private void Start()
    {
        originalEnglishColors = onButton.colors;
        originalSpanishColors = offButton.colors;

        generalButton.onClick.AddListener(openGeneralEmpty);
        audioButton.onClick.AddListener(openAudioEmpty);
        controlsButton.onClick.AddListener(openControlEmpty);
        accessiblyButton.onClick.AddListener(openAccessiblyEmpty);
        returnButton.onClick.AddListener(openMainMenuEmpty);

        onButton.onClick.AddListener(() => SelectONOFF(onButton));
        offButton.onClick.AddListener(() => SelectONOFF(offButton));

        if (onButton != null)
        {
            ColorBlock englishColors = onButton.colors;
            englishColors.normalColor = englishColors.pressedColor;
            onButton.colors = englishColors;
            selectedLanguageButton = onButton;
        }

        if (generalButton != null)
        {
            EventSystem.current.SetSelectedGameObject(generalButton.gameObject);
        }
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

    // So only one button can be pressed between the two at a time
    private void SelectONOFF(Button pressedButton)
    {
        if (pressedButton == selectedLanguageButton)
            return;

        ResetButtonColor(onButton, originalEnglishColors);
        ResetButtonColor(offButton, originalSpanishColors);

        ColorBlock pressedColors = pressedButton.colors;
        pressedColors.normalColor = pressedColors.pressedColor;
        pressedButton.colors = pressedColors;

        selectedLanguageButton = pressedButton;
    }

    private void ResetButtonColor(Button button, ColorBlock originalColors)
    {
        button.colors = originalColors;
    }

    public void openGeneralEmpty()
    {
        generalEmpty.SetActive(true);
        accessiblityEmpty.SetActive(false);
    }
    public void openAudioEmpty()
    {
        audioEmpty.SetActive(true);
        accessiblityEmpty.SetActive(false);
    }
    public void openControlEmpty()
    {
        controlsEmpty.SetActive(true);
        accessiblityEmpty.SetActive(false);
    }
    public void openAccessiblyEmpty()
    {
        //accessiblityEmpty.SetActive(true);
        //accessiblityEmpty.SetActive(false);
    }
    public void openMainMenuEmpty()
    {
        mainMenuEmpty.SetActive(true);
        accessiblityEmpty.SetActive(false);
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