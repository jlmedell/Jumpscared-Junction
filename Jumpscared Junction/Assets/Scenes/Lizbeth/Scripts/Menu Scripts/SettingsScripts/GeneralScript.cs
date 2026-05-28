using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;               // NEW: needed for coroutine
using System.Collections.Generic;

using UnityEngine.Localization;          // NEW: Localization Locale type
using UnityEngine.Localization.Settings; // NEW: LocalizationSettings.SelectedLocale

using UnityEngine.Serialization;

public class GeneralScript : MonoBehaviour
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
    [SerializeField] private Button englishButton;
    [SerializeField] private Button spanishButton;

    // NEW: This button is your Japanese button in the Inspector.
    // If you want, you can rename this variable to japaneseButton later.
    [SerializeField] private Button japaneseButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction;

    // NEW: Save the chosen language so it applies across scenes/menus.
    private const string PREF_LOCALE = "selected_locale_code";

    private Button selectedLanguageButton;
    private ColorBlock originalEnglishColors;
    private ColorBlock originalSpanishColors;
    private ColorBlock originalJapaneseColors;

    private void Start()
    {
        // NEW: Null safety in case any button isn't assigned yet.
        if (englishButton != null) originalEnglishColors = englishButton.colors;
        if (spanishButton != null) originalSpanishColors = spanishButton.colors;
        if (japaneseButton != null) originalJapaneseColors = japaneseButton.colors;

        if (generalButton != null) generalButton.onClick.AddListener(openGeneralEmpty);
        if (audioButton != null) audioButton.onClick.AddListener(openAudioEmpty);
        if (controlsButton != null) controlsButton.onClick.AddListener(openControlEmpty);
        if (accessiblyButton != null) accessiblyButton.onClick.AddListener(openAccessiblyEmpty);
        if (returnButton != null) returnButton.onClick.AddListener(openMainMenuEmpty);

        // NEW: These now pass locale codes so clicking a language changes the whole UI language.
        // en = English, es = Spanish, ja = Japanese
        if (englishButton != null) englishButton.onClick.AddListener(() => SelectLanguage(englishButton, "en"));
        if (spanishButton != null) spanishButton.onClick.AddListener(() => SelectLanguage(spanishButton, "es"));
        if (japaneseButton != null) japaneseButton.onClick.AddListener(() => SelectLanguage(japaneseButton, "ja-JP"));

        // NEW: Load saved language (defaults to English if none saved yet).
        string savedLocale = PlayerPrefs.GetString(PREF_LOCALE, "en");

        // NEW: Apply the saved locale + highlight the correct button on startup.
        // We do highlight + locale set here so every scene starts in the correct language.
        if (savedLocale == "es" && spanishButton != null)
            SelectLanguage(spanishButton, "es", applyLocale: true);
        else if (savedLocale == "ja-JP" && japaneseButton != null)
            SelectLanguage(japaneseButton, "ja-JP", applyLocale: true);
        else if (englishButton != null)
            SelectLanguage(englishButton, "en", applyLocale: true);

        if (generalButton != null && EventSystem.current != null)
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
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (currentButton != null)
            {
                currentButton.onClick.Invoke();
            }
        }
    }

    // So only one button can be pressed between the three at a time
    // NEW: Also sets the active Unity Localization Locale so ALL menus update.
    private void SelectLanguage(Button pressedButton, string localeCode, bool applyLocale = true)
    {
        if (pressedButton == selectedLanguageButton && applyLocale == false)
            return;

        // NEW: Reset highlights before selecting the new one.
        if (englishButton != null) ResetButtonColor(englishButton, originalEnglishColors);
        if (spanishButton != null) ResetButtonColor(spanishButton, originalSpanishColors);
        if (japaneseButton != null) ResetButtonColor(japaneseButton, originalJapaneseColors);

        // NEW: Set "pressed" look by copying pressedColor into normalColor.
        ColorBlock pressedColors = pressedButton.colors;
        pressedColors.normalColor = pressedColors.pressedColor;
        pressedButton.colors = pressedColors;

        selectedLanguageButton = pressedButton;

        // NEW: Actually change the game's language (global locale).
        // This updates every Localize String Event in all scenes.
        if (applyLocale)
        {
            PlayerPrefs.SetString(PREF_LOCALE, localeCode);
            PlayerPrefs.Save();
            StartCoroutine(SetLocale(localeCode));
        }
    }

    private void ResetButtonColor(Button button, ColorBlock originalColors)
    {
        button.colors = originalColors;
    }

    // NEW: LocalizationSettings initializes async, so we wait before selecting a locale.
    private IEnumerator SetLocale(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        Locale target = null;

        // NEW: Find the Locale asset with the matching code (en, es, ja).
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale != null && locale.Identifier.Code == localeCode)
            {
                target = locale;
                break;
            }
        }

        if (target != null)
            LocalizationSettings.SelectedLocale = target;
        else
            Debug.LogWarning("Locale not found: " + localeCode);
    }

    public void openGeneralEmpty()
    {
        // generalEmpty.SetActive(true);
        // generalEmpty.SetActive(false);
    }
    public void openAudioEmpty()
    {
        audioEmpty.SetActive(true);
        generalEmpty.SetActive(false);
    }
    public void openControlEmpty()
    {
        controlsEmpty.SetActive(true);
        generalEmpty.SetActive(false);
    }
    public void openAccessiblyEmpty()
    {
        accessiblityEmpty.SetActive(true);
        generalEmpty.SetActive(false);
    }
    public void openMainMenuEmpty()
    {
        mainMenuEmpty.SetActive(true);
        generalEmpty.SetActive(false);
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