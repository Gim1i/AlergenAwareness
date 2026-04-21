using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private enum menuScreen { home, settingsMain, settingsControl, settingsVolume }
    private enum buttons { Continue, New, Settings, Exit, Control, Volume, Option1, Option2, Option3, Option4 } //Caps to avoid key words

    [SerializeField] private Transform homeUI;
    [SerializeField] private Transform settingsUI;

    private Dictionary<menuScreen, Dictionary<buttons, Button>> buttonSets = new Dictionary<menuScreen, Dictionary<buttons, Button>>();
    private menuScreen currentScreen = menuScreen.home;

    //
    // Setup
    //
    private void Awake()
    {
        VisualElement homeUIRoot = homeUI.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        VisualElement settingsUIRoot = settingsUI.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        VisualElement controlsUIRoot = settingsUI.GetChild(1).GetComponent<UIDocument>().rootVisualElement;
        VisualElement volumeUIRoot = settingsUI.GetChild(2).GetComponent<UIDocument>().rootVisualElement;

        buttonSets.Add(menuScreen.home, //Save menu buttons to the dictionary
            new Dictionary<buttons, Button>() {
                { buttons.Continue, homeUIRoot.Q<Button>("Continue") },
                { buttons.New, homeUIRoot.Q<Button>("New") },
                { buttons.Settings, homeUIRoot.Q<Button>("Settings") },
                { buttons.Exit, homeUIRoot.Q<Button>("Exit") }
            }
        );
        buttonSets.Add(menuScreen.settingsMain, //Save menu buttons to the dictionary
            new Dictionary<buttons, Button>() {
                { buttons.Control, homeUIRoot.Q<Button>("Controls") },
                { buttons.Volume, homeUIRoot.Q<Button>("Volume") }
            }
        );
        buttonSets.Add(menuScreen.settingsControl, //Save menu buttons to the dictionary
            new Dictionary<buttons, Button>() {
                { buttons.Option1, homeUIRoot.Q<Button>("Option_1") },
                { buttons.Option2, homeUIRoot.Q<Button>("Option_2") },
                { buttons.Option3, homeUIRoot.Q<Button>("Option_3") },
                { buttons.Option4, homeUIRoot.Q<Button>("Option_4") }
            }
        );
        buttonSets.Add(menuScreen.settingsVolume, //Save menu buttons to the dictionary
            new Dictionary<buttons, Button>() {
            
            }
        );
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("existingGame") != 0) { //If theres a game saved
            buttonSets[menuScreen.home][buttons.Continue].SetEnabled(true); //Enable the continue button
        } else {
            buttonSets[menuScreen.home][buttons.Continue].SetEnabled(false); //Disable if no saved game
        }

        EnableSectionButtons(menuScreen.home); //Enable home buttons on load
    }

    private void EnableSectionButtons(menuScreen section) //Turns on a sections buttons
    {
        switch (section)
        {
            case menuScreen.home:
                buttonSets[menuScreen.home][buttons.Continue].clicked += Continue;
                buttonSets[menuScreen.home][buttons.New].clicked += New;
                buttonSets[menuScreen.home][buttons.Settings].clicked += Settings;
                buttonSets[menuScreen.home][buttons.Exit].clicked += Exit;
                break;
            case menuScreen.settingsMain:
                buttonSets[menuScreen.settingsMain][buttons.Control].clicked += Continue;
                buttonSets[menuScreen.settingsMain][buttons.Volume].clicked += New;
                break;
            case menuScreen.settingsControl:
                buttonSets[menuScreen.home][buttons.Continue].clicked += Continue;
                buttonSets[menuScreen.home][buttons.New].clicked += New;
                break;
            case menuScreen.settingsVolume:
                break;
        }
    }
    private void DisableSectionButtons(menuScreen section) //Turns off a sections buttons
    {
        switch (section)
        {
            case menuScreen.home:
                buttonSets[menuScreen.home][buttons.Continue].clicked -= Continue;
                buttonSets[menuScreen.home][buttons.New].clicked -= New;
                buttonSets[menuScreen.home][buttons.Settings].clicked -= Settings;
                buttonSets[menuScreen.home][buttons.Exit].clicked -= Exit;
                break;
            case menuScreen.settingsMain:
                break;
        }
    }

    //
    // Functions run by buttons
    //
    public void Continue() { //Pressing the continue button
        SceneManager.LoadScene("Gameplay");
    }
    public void New() { //Pressing the new button
        SceneManager.LoadScene("Gameplay");
    }
    public void Settings() { //Pressing the settings button

    }
    public void Exit() { //Pressing the exit button
        Application.Quit();
    }

    public void ControlsNav() { //Pressing the exit button
        Application.Quit();
    }
    public void VolumeNav() { //Pressing the exit button
        Application.Quit();
    }
}
