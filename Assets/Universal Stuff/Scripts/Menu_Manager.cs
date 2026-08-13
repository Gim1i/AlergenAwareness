using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu_Manager : MonoBehaviour
{
    enum buttons { Continue, New, settings, exit, toMainMenu }

    [SerializeField] private bool isMainMenu = true;
    [SerializeField] private string menuUIName; // Stores the menu template's name
    [SerializeField] private string menuBackgroundName; // Stores the menu template's background element name

    private Dictionary<buttons, Button> menuButtons; // Stores all buttons

    private VisualElement menuTemplate;
    private VisualElement backgroundElement;
    private bool visable;
    private bool isSettingsOpen = false;
    private Settings_Handler settingsHandler;

    //
    // Setup
    //
    private void Awake()
    {
        Debug.Assert(menuUIName != null, "Menu's UI document not assigned");
        menuTemplate = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement.Q<TemplateContainer>(menuUIName);

        if (menuBackgroundName == null) {
            Debug.Log("Background not set so skipping background controls");
        } else {
            backgroundElement = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(menuBackgroundName);
        }

        // Store all the buttons
        if (isMainMenu)
        {
            menuButtons = new Dictionary<buttons, Button>
            {
                { buttons.Continue, menuTemplate.Q<Button>("Continue") },
                { buttons.New,      menuTemplate.Q<Button>("New")      },
                { buttons.settings, menuTemplate.Q<Button>("Settings") },
                { buttons.exit,     menuTemplate.Q<Button>("Exit")     }
            };
        }
        else
        { // Skip Continue and New buttons if not main menu
            menuButtons = new Dictionary<buttons, Button>
            {
                { buttons.settings,   menuTemplate.Q<Button>("Settings") },
                { buttons.toMainMenu, menuTemplate.Q<Button>("Exit")     }
            };
        }

        // Get the settings handler
        settingsHandler = transform.GetComponent<Settings_Handler>();
        Debug.Assert(settingsHandler != null, "Settings handler not present");
    }

    private void Start()
    {
        visable = isMainMenu;

        if (isMainMenu) { // Skip if not main menu (no continue button)
            if (PlayerPrefs.GetInt("existingGame") != 0) // If theres a game saved
            {
                menuButtons[buttons.Continue].SetEnabled(true); // Enable the continue button
            }
            else
            {
                menuButtons[buttons.Continue].SetEnabled(false); // Disable if no saved game
            }
        }

        // Setup the menu buttons
        if (isMainMenu)
        {
            menuButtons[buttons.Continue].clicked += Continue;
            menuButtons[buttons.New].clicked += New;
            menuButtons[buttons.settings].clicked += Settings;
            menuButtons[buttons.exit].clicked += Exit;
        }
        else
        {
            menuButtons[buttons.settings].clicked += Settings;
            menuButtons[buttons.toMainMenu].clicked += ToMainMenu;
        }
    }

    //
    // Universal
    //
    public void ToggleMainMenuVisability() // Toggles the visability of the main page ONLY
    {
        if (isSettingsOpen)
        {
            menuTemplate.visible = true; // Hide this page
            menuTemplate.SetEnabled(true);
            isSettingsOpen = false;
        }
        else
        {
            menuTemplate.visible = false; // Show this page
            menuTemplate.SetEnabled(false);
            isSettingsOpen = true;
        }
    }
    private void Settings() // Opens the settings (though Settings_Handler)
    {
        ToggleMainMenuVisability(); // Hide this page
        settingsHandler.OpenSettings();
    }

    //
    // Main menu
    //
    private void Continue() // Pressing the continue button
    {
        Debug.Log("Continuing game");
        SceneManager.LoadScene("Gameplay");
    }
    private void New() // Pressing the new button
    {
        Debug.Log("New game");
        settingsHandler.ResetGamePrefs();
        PlayerPrefs.SetInt("existingGame", 1);
        SceneManager.LoadScene("Gameplay");
    }
    private void Exit() // Pressing the exit button
    {
        Application.Quit();
        #if DEBUG // End unity debuger if debuging
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    //
    // Pause menu
    //
    private void ToMainMenu() // Pressing exit button on the pause menu
    {
        Debug.Log("Exiting game");
        SceneManager.LoadScene("Menu");
    }

    // Hide/Show the entire menu (only used by pause menu)
    public void ToggleEntireMenuVisability()
    {
        if (visable) // Closing the pause menu
        {
            Debug.Log("Closing pause menu");
            // A couple extra steps if closed from any settings menu
            if (isSettingsOpen)
            {
                settingsHandler.ForceCloseSettings();
                isSettingsOpen = false;
            }

            menuTemplate.visible = false; // Hide the main page
            menuTemplate.SetEnabled(false);
            menuTemplate.parent.visible = false; // Disable the menu template
            menuTemplate.parent.SetEnabled(false);
            visable = false;
            if (backgroundElement != null) { backgroundElement.visible = false; } // Hide background
        }
        else // Open the pause menu
        {
            Debug.Log("Opening pause menu");
            menuTemplate.visible = true; // Show the main page
            menuTemplate.SetEnabled(true);
            menuTemplate.parent.visible = true; // Enable the menu template
            menuTemplate.parent.SetEnabled(true);
            visable = true;
            if (backgroundElement != null) { backgroundElement.visible = true; } // Show background
        }
    }
}