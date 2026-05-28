using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] public GameObject loadEmpty;
    [SerializeField] public GameObject settingsEmpty;
    [SerializeField] public GameObject creditsEmpty;
    [SerializeField] public GameObject mainMenuEmpty;

    [Header("Scene Settings")]
    [SerializeField] private string newGameScene = "Sprint 3 Demo";

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction;

    private void Start()
    {
        newGameButton.onClick.AddListener(loadNewGameScene);
        loadGameButton.onClick.AddListener(openLoadEmpty);
        settingsButton.onClick.AddListener(openSettingsEmpty);
        creditsButton.onClick.AddListener(openCreditsEmpty);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void AutoSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        newGameButton.Select();
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
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

    public void loadNewGameScene() => SceneManager.LoadScene(newGameScene);
    public void openLoadEmpty()
    {
        loadEmpty.SetActive(true);
        mainMenuEmpty.SetActive(false);
    }
    public void openSettingsEmpty()
    {
        settingsEmpty.SetActive(true);
        mainMenuEmpty.SetActive(false);
    }
    public void openCreditsEmpty()
    {
        creditsEmpty.SetActive(true);
        mainMenuEmpty.SetActive(false);
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
        newGameButton.onClick.RemoveListener(loadNewGameScene);
        loadGameButton.onClick.RemoveListener(openLoadEmpty);
        settingsButton.onClick.RemoveListener(openSettingsEmpty);
        creditsButton.onClick.RemoveListener(openCreditsEmpty);
        quitButton.onClick.RemoveListener(QuitGame);
    }
}