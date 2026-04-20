using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private enum menuScreen { home, settings }
    private enum homebuttons { Continue, New, Settings, Exit } //Caps to avoid key words

    [SerializeField] private Transform homeUI;
    [SerializeField] private Transform settingsUI;

    private Dictionary<menuScreen, Dictionary<homebuttons, Button>> buttonSets = new Dictionary<menuScreen, Dictionary<homebuttons, Button>>();
    private menuScreen currentScreen = menuScreen.home;

    //
    // Setup
    //
    private void Awake()
    {
        VisualElement uiDocRoot = homeUI.GetChild(0).GetComponent<UIDocument>().rootVisualElement;

        buttonSets.Add(menuScreen.home, //Save menu buttons to the dictionary
            new Dictionary<homebuttons, Button>() {
                { homebuttons.Continue, uiDocRoot.Q<Button>("Continue") },
                { homebuttons.New, uiDocRoot.Q<Button>("New") },
                { homebuttons.Settings, uiDocRoot.Q<Button>("Settings") },
                { homebuttons.Exit, uiDocRoot.Q<Button>("Exit") }
            }
        );
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("existingGame") != 0) { //If theres a game saved
            buttonSets[menuScreen.home][homebuttons.Continue].SetEnabled(true); //Enable the continue button
        } else {
            buttonSets[menuScreen.home][homebuttons.Continue].SetEnabled(false); //Disable if no saved game
        }

        EnableSectionButtons(menuScreen.home); //Enable home buttons on load
    }

    private void EnableSectionButtons(menuScreen section) //Turns on a sections buttons
    {
        switch (section)
        {
            case menuScreen.home:
                buttonSets[menuScreen.home][homebuttons.Continue].clicked += Continue;
                buttonSets[menuScreen.home][homebuttons.New].clicked += New;
                buttonSets[menuScreen.home][homebuttons.Settings].clicked += Settings;
                buttonSets[menuScreen.home][homebuttons.Exit].clicked += Exit;
                break;
            case menuScreen.settings:
                break;
        }
    }
    private void DisableSectionButtons(menuScreen section) //Turns off a sections buttons
    {
        switch (section)
        {
            case menuScreen.home:
                buttonSets[menuScreen.home][homebuttons.Continue].clicked -= Continue;
                buttonSets[menuScreen.home][homebuttons.New].clicked -= New;
                buttonSets[menuScreen.home][homebuttons.Settings].clicked -= Settings;
                buttonSets[menuScreen.home][homebuttons.Exit].clicked -= Exit;
                break;
            case menuScreen.settings:
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
}
