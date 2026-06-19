using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private enum screen { home, settingsMain, settingsControl, settingsVolume, howToPlay }
    private enum homeButtons { Continue, New, Settings, Exit }  //Caps to avoid key words
    private enum settingsNavButtons { Control, Volume, HowToPlay, Back, Reset }
    private enum controlsButtons { Option1, Option2, Option3, Option4, Back, ApplyControls }
    private enum volumeButtons { Back, ApplyVolume }
    private enum howToPlayButtons { Back }
    private enum sliders { master, music, ui, textSpeed }

    // Stores all buttons on the main menu
    private Dictionary<screen, (Dictionary<int, Button> Buttons, Dictionary<sliders, Slider> Sliders)> menuElements;
    private Dictionary<screen, TemplateContainer> pageTemplate; //Stores all page's TemplateContainer

    private screen currentScreen = screen.home;
    private Saved_Info_Manager savedInfoManager;

    //
    // Setup
    //
    private void Awake()
    {
        VisualElement UI = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;

        // Get and store each page's TemplateContainer
        pageTemplate = new Dictionary<screen, TemplateContainer>()
        {
            { screen.home,            UI.Q<TemplateContainer>("Home_Template")},
            { screen.settingsMain,    UI.Q<TemplateContainer>("SettingsMain_Template")},
            { screen.settingsControl, UI.Q<TemplateContainer>("Controls_Template")},
            { screen.settingsVolume,  UI.Q<TemplateContainer>("Volume_Template")},
            { screen.howToPlay,       UI.Q<TemplateContainer>("HowToPlay_Template")}
        };

        // Setup the storea and insert the buttons and sliders
        menuElements = new Dictionary<screen, (Dictionary<int, Button> Buttons, Dictionary<sliders, Slider> Sliders)>
        {
            // Home page
            { screen.home,
                (new Dictionary<int, Button>() {
                    { (int)homeButtons.Continue, pageTemplate[screen.home].Q<Button>("Continue") },
                    { (int)homeButtons.New,      pageTemplate[screen.home].Q<Button>("New")      },
                    { (int)homeButtons.Settings, pageTemplate[screen.home].Q<Button>("Settings") },
                    { (int)homeButtons.Exit,     pageTemplate[screen.home].Q<Button>("Exit")     }
                }, new Dictionary<sliders, Slider>())
            },

            // Main Settings page
            { screen.settingsMain,
                (new Dictionary<int, Button>() {
                    { (int)settingsNavButtons.Control,   pageTemplate[screen.settingsMain].Q<Button>("Controls")  },
                    { (int)settingsNavButtons.Volume,    pageTemplate[screen.settingsMain].Q<Button>("Volume")    },
                    { (int)settingsNavButtons.HowToPlay, pageTemplate[screen.settingsMain].Q<Button>("HowToPlay") },
                    { (int)settingsNavButtons.Back,      pageTemplate[screen.settingsMain].Q<Button>("Back")      },
                    { (int)settingsNavButtons.Reset,     pageTemplate[screen.settingsMain].Q<Button>("Reset")     }
                }, new Dictionary<sliders, Slider>())
            },

            // Settings controls page
            { screen.settingsControl,
                (new Dictionary<int, Button>() {
                    { (int)controlsButtons.Option1,       pageTemplate[screen.settingsControl].Q<Button>("Option_1")       },
                    { (int)controlsButtons.Option2,       pageTemplate[screen.settingsControl].Q<Button>("Option_2")       },
                    { (int)controlsButtons.Option3,       pageTemplate[screen.settingsControl].Q<Button>("Option_3")       },
                    { (int)controlsButtons.Option4,       pageTemplate[screen.settingsControl].Q<Button>("Option_4")       },
                    { (int)controlsButtons.Back,          pageTemplate[screen.settingsControl].Q<Button>("Back")           },
                    { (int)controlsButtons.ApplyControls, pageTemplate[screen.settingsControl].Q<Button>("Apply_Controls") }
                },
                new Dictionary<sliders, Slider>() {
                    { sliders.textSpeed, pageTemplate[screen.settingsControl].Q<Slider>("Text_Speed_Slider") }
                })
            },

            // Settings volume page
            { screen.settingsVolume,
                (new Dictionary<int, Button>() {
                    { (int)volumeButtons.Back,        pageTemplate[screen.settingsVolume].Q<Button>("Back")         },
                    { (int)volumeButtons.ApplyVolume, pageTemplate[screen.settingsVolume].Q<Button>("Apply_Volume") }
                },
                new Dictionary<sliders, Slider>() {
                    { sliders.master, pageTemplate[screen.settingsVolume].Q<Slider>("Master_Slider") },
                    { sliders.music,  pageTemplate[screen.settingsVolume].Q<Slider>("Music_Slider")  },
                    { sliders.ui,     pageTemplate[screen.settingsVolume].Q<Slider>("UI_Slider")     }
                })
            },

            // Settings how to play page
            { screen.howToPlay,
                (new Dictionary<int, Button>() {
                    { (int)howToPlayButtons.Back, pageTemplate[screen.howToPlay].Q<Button>("Back") }
                }, new Dictionary<sliders, Slider>())
            }
        };

        savedInfoManager = GameObject.Find("PermaLoader").transform.GetComponent<Saved_Info_Manager>();
        Debug.Assert(savedInfoManager != null, "Couldn't find the saved info manager");
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("existingGame") != 0) { //If theres a game saved
            menuElements[screen.home].Buttons[(int)homeButtons.Continue].SetEnabled(true); //Enable the continue button
        } else {
            menuElements[screen.home].Buttons[(int)homeButtons.Continue].SetEnabled(false); //Disable if no saved game
        }

        //Set values for volume sliders
        menuElements[screen.settingsVolume].Sliders[sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        menuElements[screen.settingsVolume].Sliders[sliders.music].value  = 100 * PlayerPrefs.GetFloat("musicVolume");
        menuElements[screen.settingsVolume].Sliders[sliders.ui].value     = 100 * PlayerPrefs.GetFloat("uiVolume");

        //Set text speed slider value
        menuElements[screen.settingsControl].Sliders[sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");

        EnableControls();
        NavigateTo(screen.home); //Set home as current screen
    }

    //
    // Main menu
    //
    public void Settings() => NavigateTo(screen.settingsMain); //To settings

    public void Continue() { //Pressing the continue button
        Debug.Log("Continuing game");
        SceneManager.LoadScene("Gameplay");
    }
    public void New() { //Pressing the new button
        Debug.Log("New game");
        savedInfoManager.ResetGamePrefs();
        PlayerPrefs.SetInt("existingGame", 1);
        SceneManager.LoadScene("Gameplay");
    }
    public void Exit() { //Pressing the exit button
        Application.Quit();
        #if DEBUG //End unity debuger if debuging
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    //
    // Settings main page
    //
    public void ControlsNav()  => NavigateTo(screen.settingsControl); //To controls page
    public void VolumeNav()    => NavigateTo(screen.settingsVolume); //To volume page
    public void HowToPlayNav() => NavigateTo(screen.howToPlay); //To how to play page
    public void BackToHome()   => NavigateTo(screen.home); //To main menu

    public void BackToSettings() => NavigateTo(screen.settingsMain); //To settings main page (from all sub-pages)

    public void ResetSettings() //Reset settings
    {
        Debug.Log("Resetting settings");
        savedInfoManager.ResetSettingsPrefs();

        // Update all sliders to their new values
        menuElements[screen.settingsVolume].Sliders[sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        menuElements[screen.settingsVolume].Sliders[sliders.music].value  = 100 * PlayerPrefs.GetFloat("musicVolume");
        menuElements[screen.settingsVolume].Sliders[sliders.ui].value     = 100 * PlayerPrefs.GetFloat("uiVolume");

        menuElements[screen.settingsControl].Sliders[sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");

        NavigateTo(screen.home); // To main menu (user feedback)
    }

    //
    // Controls settings page
    //
    public void ApplyControls() { //Pressing apply
        Debug.Log("Applying changes");
        PlayerPrefs.SetFloat("textSpeed", menuElements[screen.settingsControl].Sliders[sliders.textSpeed].value);

        NavigateTo(screen.settingsMain);
    }

    //
    // Volume settings page
    //
    public void ApplyVolume() { //Pressing apply
        Debug.Log("Applying changes");

        PlayerPrefs.SetFloat("masterVolume", menuElements[screen.settingsVolume].Sliders[sliders.master].value / 100);
        PlayerPrefs.SetFloat("musicVolume",  menuElements[screen.settingsVolume].Sliders[sliders.music].value  / 100);
        PlayerPrefs.SetFloat("uiVolume",     menuElements[screen.settingsVolume].Sliders[sliders.ui].value     / 100);

        NavigateTo(screen.settingsMain);
    }

    //
    // How to play page
    //

    //
    // Other functions
    //
    private void NavigateTo(screen screenTo)
    {
        Debug.Log("To "+screenTo.ToString());
        pageTemplate[currentScreen].visible = false;
        pageTemplate[currentScreen].SetEnabled(false);
        pageTemplate[screenTo].visible = true;
        pageTemplate[screenTo].SetEnabled(true);
        currentScreen = screenTo;
    }

    //
    //  Setup the buttons
    //
    private void EnableControls() //Turns on the buttons
    {
        // Main page
        menuElements[screen.home].Buttons[(int)homeButtons.Continue].clicked += Continue;
        menuElements[screen.home].Buttons[(int)homeButtons.New].clicked      += New;
        menuElements[screen.home].Buttons[(int)homeButtons.Settings].clicked += Settings;
        menuElements[screen.home].Buttons[(int)homeButtons.Exit].clicked     += Exit;

        // Main settings page
        menuElements[screen.settingsMain].Buttons[(int)settingsNavButtons.Control].clicked   += ControlsNav;
        menuElements[screen.settingsMain].Buttons[(int)settingsNavButtons.Volume].clicked    += VolumeNav;
        menuElements[screen.settingsMain].Buttons[(int)settingsNavButtons.HowToPlay].clicked += HowToPlayNav;
        menuElements[screen.settingsMain].Buttons[(int)settingsNavButtons.Back].clicked      += BackToHome;
        menuElements[screen.settingsMain].Buttons[(int)settingsNavButtons.Reset].clicked     += ResetSettings;

        // Controls page
        menuElements[screen.settingsControl].Buttons[(int)controlsButtons.Back].clicked          += BackToSettings;
        menuElements[screen.settingsControl].Buttons[(int)controlsButtons.ApplyControls].clicked += ApplyControls;

        //Volume page
        menuElements[screen.settingsVolume].Buttons[(int)volumeButtons.Back].clicked        += BackToSettings;
        menuElements[screen.settingsVolume].Buttons[(int)volumeButtons.ApplyVolume].clicked += ApplyVolume;

        // How to play page
        menuElements[screen.howToPlay].Buttons[(int)howToPlayButtons.Back].clicked += BackToSettings;
        Debug.Log("Enabled buttons");
    }
}