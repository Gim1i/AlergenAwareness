using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class Settings_Handler : MonoBehaviour
{
    [SerializeField] private UIDocument settingsUI; // Stores the menu's template

    private Dictionary<ID, Button> menuButtons; // Stores all buttons
    private Dictionary<ID, Slider> menuSliders; // Stores all sliders
    private Dictionary<ID, TemplateContainer> pageTemplates; // Stores all page's TemplateContainer

    private Saved_Info_Manager savedInfoManager;
    private Menu_Manager menuManager;
    private ID currentScreen = Page.closed;

    //
    // Setup
    //
    private void Awake()
    {
        Debug.Assert(settingsUI != null, "Settings UI Document not set");
        VisualElement UI = settingsUI.rootVisualElement;

        // Get and link each page's TemplateContainer to its internal Page ID
        pageTemplates = new Dictionary<ID, TemplateContainer>
        {
            { Page.settingsNavPage, UI.Q<TemplateContainer>("SettingsNav_Template") },
            { Page.controlsPage,    UI.Q<TemplateContainer>("Controls_Template")     },
            { Page.volumePage,      UI.Q<TemplateContainer>("Volume_Template")       },
            { Page.howToPlayPage,   UI.Q<TemplateContainer>("HowToPlay_Template")    },
        };

        // Assign all buttons to their respective internal IDs
        menuButtons = new Dictionary<ID, Button>
        {
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

        // Try to locate the Saved_Info_Manager
        savedInfoManager = GameObject.Find("PermaLoader").transform.GetComponent<Saved_Info_Manager>();
        Debug.Assert(savedInfoManager != null, "Couldn't find the saved info manager");
    }

    private void Start()
    {
        // Set values for volume sliders
        menuSliders[Page.volume.sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        menuSliders[Page.volume.sliders.music].value  = 100 * PlayerPrefs.GetFloat("musicVolume");
        menuSliders[Page.volume.sliders.ui].value     = 100 * PlayerPrefs.GetFloat("uiVolume");

        // Set text speed slider value
        menuSliders[Page.controls.sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");
    }

    //
    // Settings main page
    //
    public void ControlsNav()  => NavigateTo(Page.controlsPage);   // To controls page
    public void VolumeNav()    => NavigateTo(Page.volumePage);       // To volume page
    public void HowToPlayNav() => NavigateTo(Page.howToPlayPage); // To how to play page
    public void BackToHome()   => NavigateTo(Page.closed);          // To main menu

    public void BackToSettings() => NavigateTo(Page.settingsNavPage); // To settings main page (from all sub-pages)

    public void ResetSettings() // Reset settings
    {
        Debug.Log("Resetting settings");
        savedInfoManager.ResetSettingsPrefs();

        // Update all sliders to their new values
        menuSliders[Page.volume.sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        menuSliders[Page.volume.sliders.music].value  = 100 * PlayerPrefs.GetFloat("musicVolume");
        menuSliders[Page.volume.sliders.ui].value     = 100 * PlayerPrefs.GetFloat("uiVolume");

        menuSliders[Page.controls.sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");

        NavigateTo(Page.closed); // To main menu (user feedback)
    }

    //
    // Controls settings page
    //
    public void ApplyControls() // Pressing apply
    {
        Debug.Log("Applying changes");
        PlayerPrefs.SetFloat("textSpeed", menuSliders[Page.controls.sliders.textSpeed].value);

        NavigateTo(Page.settingsNavPage);
    }

    //
    // Volume settings page
    //
    public void ApplyVolume() // Pressing apply
    {
        Debug.Log("Applying changes");

        PlayerPrefs.SetFloat("masterVolume", menuSliders[Page.volume.sliders.master].value / 100);
        PlayerPrefs.SetFloat("musicVolume",  menuSliders[Page.volume.sliders.music].value  / 100);
        PlayerPrefs.SetFloat("uiVolume",     menuSliders[Page.volume.sliders.ui].value     / 100);

        NavigateTo(Page.settingsNavPage);
    }

    //
    // How to play page
    //

    //
    // Other functions
    //
    private void NavigateTo(ID screenTo)
    {
        // Exiting the menu if needed
        if (screenTo == Page.closed)
        {
            pageTemplates[currentScreen].visible = false;
            pageTemplates[currentScreen].SetEnabled(false);
            currentScreen = screenTo;
            Debug.Log("Closing settings");
            menuManager.ToggleMainMenuVisability();
            DisableControls();
        } else
        {
            Debug.Log("To " + screenTo);
            pageTemplates[currentScreen].visible = false;
            pageTemplates[currentScreen].SetEnabled(false);
            pageTemplates[screenTo].visible = true;
            pageTemplates[screenTo].SetEnabled(true);
            currentScreen = screenTo;
        }

        // Reset the sliders (as any value input wasnt saved)
        if (screenTo == Page.controlsPage) {
            menuSliders[Page.controls.sliders.textSpeed].value = PlayerPrefs.GetFloat("textSpeed");
        }

        if (screenTo == Page.volumePage) {
            menuSliders[Page.volume.sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
            menuSliders[Page.volume.sliders.music].value  = 100 * PlayerPrefs.GetFloat("musicVolume");
            menuSliders[Page.volume.sliders.ui].value     = 100 * PlayerPrefs.GetFloat("uiVolume");
        }
    }

    // Open the settings from another script
    public void OpenSettings()
    {
        Debug.Log("Opening settings");

        // Get the Menu_Manager
        menuManager = transform.GetComponent<Menu_Manager>();
        Debug.Assert(menuManager != null, "Menu manager not present");

        currentScreen = Page.settingsNavPage;
        EnableControls();
        pageTemplates[currentScreen].visible = true;
        pageTemplates[currentScreen].SetEnabled(true);
    }

    // Resets game related player prefs (means I dont gotta store Saved_Info_Manager in the Menu_Manager)
    public void ResetGamePrefs() { savedInfoManager.ResetGamePrefs(); }

    // Force exit settings
    public void ForceCloseSettings()
    {
        pageTemplates[currentScreen].visible = false;
        pageTemplates[currentScreen].SetEnabled(false);
        currentScreen = Page.closed;
        Debug.Log("Force closing settings");
        DisableControls();
    }

    //
    //  Setup the buttons
    //
    private void EnableControls() //Turns on the buttons
    {
        // Main settings page
        menuButtons[Page.settingsNav.buttons.control].clicked   += ControlsNav;
        menuButtons[Page.settingsNav.buttons.volume].clicked    += VolumeNav;
        menuButtons[Page.settingsNav.buttons.howToPlay].clicked += HowToPlayNav;
        menuButtons[Page.settingsNav.buttons.back].clicked      += BackToHome;
        menuButtons[Page.settingsNav.buttons.reset].clicked     += ResetSettings;

        // Controls page
        menuButtons[Page.controls.buttons.back].clicked          += BackToSettings;
        menuButtons[Page.controls.buttons.applyControls].clicked += ApplyControls;

        //Volume page
        menuButtons[Page.volume.buttons.back].clicked        += BackToSettings;
        menuButtons[Page.volume.buttons.applyVolume].clicked += ApplyVolume;

        // How to play page
        menuButtons[Page.howToPlay.buttons.back].clicked += BackToSettings;
        Debug.Log("Enabled buttons");
    }

    private void DisableControls() //Turns on the buttons
    {
        // Main settings page
        menuButtons[Page.settingsNav.buttons.control].clicked   -= ControlsNav;
        menuButtons[Page.settingsNav.buttons.volume].clicked    -= VolumeNav;
        menuButtons[Page.settingsNav.buttons.howToPlay].clicked -= HowToPlayNav;
        menuButtons[Page.settingsNav.buttons.back].clicked      -= BackToHome;
        menuButtons[Page.settingsNav.buttons.reset].clicked     -= ResetSettings;

        // Controls page
        menuButtons[Page.controls.buttons.back].clicked          -= BackToSettings;
        menuButtons[Page.controls.buttons.applyControls].clicked -= ApplyControls;

        //Volume page
        menuButtons[Page.volume.buttons.back].clicked        -= BackToSettings;
        menuButtons[Page.volume.buttons.applyVolume].clicked -= ApplyVolume;

        // How to play page
        menuButtons[Page.howToPlay.buttons.back].clicked -= BackToSettings;
        Debug.Log("Disabled buttons");
    }

    //
    // Custom "enum" to categorise pages, buttons and sliders in a better format
    // Stores: the page its on, wether its a button or a slider and its unique ID
    //
    private static class Page
    {
        public static readonly ID closed = 1;
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
                public static readonly ID control   = button[0];
                public static readonly ID volume    = button[1];
                public static readonly ID howToPlay = button[2];
                public static readonly ID back      = button[3];
                public static readonly ID reset     = button[4];
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
                public static readonly ID back    = button[4];
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
                public static readonly ID back        = button[0];
                public static readonly ID applyVolume = button[1];
            }

            // Sliders
            public static readonly ID slider = volumePage[1];
            public static class sliders
            {
                public static readonly ID master = slider[0];
                public static readonly ID music  = slider[1];
                public static readonly ID ui     = slider[2];
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
