using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] public GameObject mainMenuEmpty;
    [SerializeField] public GameObject loadEmpty;

    [Header("Menu Buttons")]
    [SerializeField] private Button returnButton;
    [SerializeField] private Button slotOneButton;
    [SerializeField] private Button slotTwoButton;
    [SerializeField] private Button slotThreeButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference submitAction;

    private void Start()
    {
        returnButton.onClick.AddListener(openMainMenuEmpty);
        slotOneButton.onClick.AddListener(loadSlotOne);
        slotTwoButton.onClick.AddListener(loadSlotTwo);
        slotThreeButton.onClick.AddListener(loadSlotThree);
    }
    private void AutoSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        slotOneButton.Select();
        EventSystem.current.SetSelectedGameObject(slotOneButton.gameObject);
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

    public void openMainMenuEmpty()
    {
        mainMenuEmpty.SetActive(true);
        loadEmpty.SetActive(false);
    }

    public void loadSlotOne()
    {
        // TODO
    }

    public void loadSlotTwo()
    {
        // TODO
    }

    public void loadSlotThree()
    {
        // TODO
    }

    private void OnDestroy()
    {
        returnButton.onClick.RemoveListener(openMainMenuEmpty);
        slotOneButton.onClick.RemoveListener(loadSlotOne);
        slotTwoButton.onClick.RemoveListener(loadSlotTwo);
        slotThreeButton.onClick.RemoveListener(loadSlotThree);
    }
}