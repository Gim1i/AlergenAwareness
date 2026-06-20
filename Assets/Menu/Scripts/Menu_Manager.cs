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
    private Dictionary<ID, Button> menuButtons;
    // Stores all sliders on the main menu
    private Dictionary<ID, Slider> menuSliders;
    private Dictionary<ID, TemplateContainer> pageTemplates; // Stores all page's TemplateContainer

    private screen currentScreen = screen.home;
    private Saved_Info_Manager savedInfoManager;

    //
    // Setup
    //
    private void Awake()
    {
        VisualElement UI = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;

        // Get and link each page's TemplateContainer to its internal Page ID
        pageTemplates = new Dictionary<ID, TemplateContainer>
        {
            { Page.homePage,        UI.Q<TemplateContainer>("Home_Template")         },
            { Page.settingsNavPage, UI.Q<TemplateContainer>("SettingsMain_Template") },
            { Page.controlsPage,    UI.Q<TemplateContainer>("Controls_Template")     },
            { Page.volumePage,      UI.Q<TemplateContainer>("Volume_Template")       },
            { Page.howToPlayPage,   UI.Q<TemplateContainer>("HowToPlay_Template")    },
        };

        // Assign all buttons to their respective internal IDs
        menuButtons = new Dictionary<ID, Button>
        {
            // Home buttons
            { Page.home.buttons.Continue, pageTemplates[Page.homePage].Q<Button>("Continue") },
            { Page.home.buttons.New,      pageTemplates[Page.homePage].Q<Button>("New")      },
            { Page.home.buttons.settings, pageTemplates[Page.homePage].Q<Button>("Settings") },
            { Page.home.buttons.exit,     pageTemplates[Page.homePage].Q<Button>("Exit")     },

            // Settings Nav buttons
            { Page.settingsNav.buttons.control,   pageTemplates[Page.settingsNavPage].Q<Button>("Controls")  },
            { Page.settingsNav.buttons.volume,    pageTemplates[Page.settingsNavPage].Q<Button>("Volume")    },
            { Page.settingsNav.buttons.howToPlay, pageTemplates[Page.settingsNavPage].Q<Button>("HowToPlay") },
            { Page.settingsNav.buttons.back,      pageTemplates[Page.settingsNavPage].Q<Button>("Back")      },
            { Page.settingsNav.buttons.reset,     pageTemplates[Page.settingsNavPage].Q<Button>("Reset")     },

            // Controls buttons
            { Page.controls.buttons.option1,       pageTemplates[Page.controlsPage].Q<Button>("Option_1")       },
            { Page.controls.buttons.option2,       pageTemplates[Page.controlsPage].Q<Button>("Option_2")       },
            { Page.controls.buttons.option3,       pageTemplates[Page.controlsPage].Q<Button>("Option_3")       },
            { Page.controls.buttons.option4,       pageTemplates[Page.controlsPage].Q<Button>("Option_4")       },
            { Page.controls.buttons.back,          pageTemplates[Page.controlsPage].Q<Button>("Back")           },
            { Page.controls.buttons.applyControls, pageTemplates[Page.controlsPage].Q<Button>("Apply_Controls") },

            // Volume buttons
            { Page.volume.buttons.back,        pageTemplates[Page.volumePage].Q<Button>("Back")         },
            { Page.volume.buttons.applyVolume, pageTemplates[Page.volumePage].Q<Button>("Apply_Volume") },

            // How To Play buttons
            { Page.howToPlay.buttons.back, pageTemplates[Page.howToPlayPage].Q<Button>("Back") }
        };

        // Assign all sliders to their respective internal IDs
        menuSliders = new Dictionary<ID, Slider>
        {
            // Controls sliders
            { Page.controls.sliders.textSpeed, pageTemplates[Page.controlsPage].Q<Slider>("Text_Speed_Slider") },

            // Volume sliders
            { Page.volume.sliders.master, pageTemplates[Page.volumePage].Q<Slider>("Master_Slider") },
            { Page.volume.sliders.music,  pageTemplates[Page.volumePage].Q<Slider>("Music_Slider")  },
            { Page.volume.sliders.ui,     pageTemplates[Page.volumePage].Q<Slider>("UI_Slider")     }
        };

        savedInfoManager = GameObject.Find("PermaLoader").transform.GetComponent<Saved_Info_Manager>();
        Debug.Assert(savedInfoManager != null, "Couldn't find the saved info manager");
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("existingGame") != 0) { // If theres a game saved
            menuButtons[Page.home.buttons.Continue].SetEnabled(true); // Enable the continue button
        } else {
            menuButtons[Page.home.buttons.Continue].SetEnabled(false); //Disable if no saved game
        }

        // Set values for volume sliders
        menuSliders[Page.volume.sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        menuSliders[Page.volume.sliders.music].value  = 100 * PlayerPrefs.GetFloat("musicVolume");
        menuSliders[Page.volume.sliders.ui].value     = 100 * PlayerPrefs.GetFloat("uiVolume");

        // Set text speed slider value
        menuSliders[Page.controls.sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");

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
        menuSliders[Page.volume.sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        menuSliders[Page.volume.sliders.music].value = 100 * PlayerPrefs.GetFloat("musicVolume");
        menuSliders[Page.volume.sliders.ui].value = 100 * PlayerPrefs.GetFloat("uiVolume");

        menuSliders[Page.controls.sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");

        NavigateTo(screen.home); // To main menu (user feedback)
    }

    //
    // Controls settings page
    //
    public void ApplyControls() { //Pressing apply
        Debug.Log("Applying changes");
        PlayerPrefs.SetFloat("textSpeed", menuSliders[Page.controls.sliders.textSpeed].value);

        NavigateTo(screen.settingsMain);
    }

    //
    // Volume settings page
    //
    public void ApplyVolume() { //Pressing apply
        Debug.Log("Applying changes");

        PlayerPrefs.SetFloat("masterVolume", menuSliders[Page.volume.sliders.master].value / 100);
        PlayerPrefs.SetFloat("musicVolume",  menuSliders[Page.volume.sliders.music].value  / 100);
        PlayerPrefs.SetFloat("uiVolume",     menuSliders[Page.volume.sliders.ui].value     / 100);

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
        pageTemplates[currentScreen].visible = false;
        pageTemplates[currentScreen].SetEnabled(false);
        pageTemplates[screenTo].visible = true;
        pageTemplates[screenTo].SetEnabled(true);
        currentScreen = screenTo;
    }

    //
    //  Setup the buttons
    //
    private void EnableControls() //Turns on the buttons
    {
        // Main page
        menuElements[screen.home].Buttons[(short)homeButtons.Continue].clicked += Continue;
        menuElements[screen.home].Buttons[(short)homeButtons.New].clicked      += New;
        menuElements[screen.home].Buttons[(short)homeButtons.Settings].clicked += Settings;
        menuElements[screen.home].Buttons[(short)homeButtons.Exit].clicked     += Exit;

        // Main settings page
        menuElements[screen.settingsMain].Buttons[(short)settingsNavButtons.Control].clicked   += ControlsNav;
        menuElements[screen.settingsMain].Buttons[(short)settingsNavButtons.Volume].clicked    += VolumeNav;
        menuElements[screen.settingsMain].Buttons[(short)settingsNavButtons.HowToPlay].clicked += HowToPlayNav;
        menuElements[screen.settingsMain].Buttons[(short)settingsNavButtons.Back].clicked      += BackToHome;
        menuElements[screen.settingsMain].Buttons[(short)settingsNavButtons.Reset].clicked     += ResetSettings;

        // Controls page
        menuElements[screen.settingsControl].Buttons[(short)controlsButtons.Back].clicked          += BackToSettings;
        menuElements[screen.settingsControl].Buttons[(short)controlsButtons.ApplyControls].clicked += ApplyControls;

        //Volume page
        menuElements[screen.settingsVolume].Buttons[(short)volumeButtons.Back].clicked        += BackToSettings;
        menuElements[screen.settingsVolume].Buttons[(short)volumeButtons.ApplyVolume].clicked += ApplyVolume;

        // How to play page
        menuElements[screen.howToPlay].Buttons[(short)howToPlayButtons.Back].clicked += BackToSettings;
        Debug.Log("Enabled buttons");
    }
    
    //
    // Custom "enum" to categorise pages, buttons and sliders in a better format
    // Stores: the page its on, wether its a button or a slider and its unique ID
    //
    private static class Page
    {
        //
        // Home
        //
        public static readonly ID homePage = 1;
        public static class home
        {
            // Buttons
            public static readonly ID button = homePage[0];
            public static class buttons
            {
                public static readonly ID Continue = button[0];
                public static readonly ID New = button[1];
                public static readonly ID settings = button[2];
                public static readonly ID exit = button[3];
            }
        }

        //
        // Settings Nav
        //
        public static readonly ID settingsNavPage = 2;
        public static class settingsNav
        {
            // Buttons
            public static readonly ID button = settingsNavPage[0];
            public static class buttons
            {
                public static readonly ID control = button[0];
                public static readonly ID volume = button[1];
                public static readonly ID howToPlay = button[2];
                public static readonly ID back = button[3];
                public static readonly ID reset = button[4];
            }
        }

        //
        // Controls
        //
        public static readonly ID controlsPage = 3;
        public static class controls
        {
            // Buttons
            public static readonly ID button = controlsPage[0];
            public static class buttons
            {
                public static readonly ID option1 = button[0];
                public static readonly ID option2 = button[1];
                public static readonly ID option3 = button[2];
                public static readonly ID option4 = button[3];
                public static readonly ID back = button[4];
                public static readonly ID applyControls = button[5];
            }

            // Sliders
            public static readonly ID slider = controlsPage[1];
            public static class sliders
            {
                public static readonly ID textSpeed = slider[0];
            }
        }

        //
        // Volume
        //
        public static readonly ID volumePage = 4;
        public static class volume
        {
            // Buttons
            public static readonly ID button = volumePage[0];
            public static class buttons
            {
                public static readonly ID back = button[0];
                public static readonly ID applyVolume = button[1];
            }

            // Sliders
            public static readonly ID slider = controlsPage[1];
            public static class sliders
            {
                public static readonly ID master = slider[0];
                public static readonly ID music = slider[1];
                public static readonly ID ui = slider[2];
            }
        }

        //
        // How To Play
        //
        public static readonly ID howToPlayPage = 5;
        public static class howToPlay
        {
            // Buttons
            public static readonly ID button = howToPlayPage[0];
            public static class buttons
            {
                public static readonly ID back = button[0];
            }
        }
    }
}