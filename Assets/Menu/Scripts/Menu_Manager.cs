using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private enum menuScreen { home, settingsMain, settingsControl, settingsVolume, howToPlay }
    private enum buttons { Continue, New, Settings, Exit, Control, Volume, Option1, Option2, Option3, Option4, HowToPlay, Back, Apply } //Caps to avoid key words
    private enum homeButtons { Continue, New, Settings, Exit }  //Caps to avoid key words
    private enum settingsMainButtons { Control, Volume, HowToPlay, Back }
    private enum settingsControlButtons { Option1, Option2, Option3, Option4, Back }
    private enum settingsVolumeButtons { Back, Apply }
    private enum howToPlayButtons { Back }
    private enum sliders { master, music, ui }

    private Dictionary<homeButtons, Button> homeBtns;
    private Dictionary<settingsMainButtons, Button> settingsMainBtns;
    private Dictionary<settingsControlButtons, Button> settingsControlBtns;
    private Dictionary<settingsVolumeButtons, Button> settingsVolumeBtns;
    private Dictionary<howToPlayButtons, Button> howToPlayBtns;
    private Dictionary<sliders, Slider> allSliders;
    private Dictionary<menuScreen, TemplateContainer> pages;
    private menuScreen currentScreen = menuScreen.home;
    private Saved_Info_Manager savedInfoManager;

    //
    // Setup
    //
    private void Awake()
    {
        VisualElement UI = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;

        pages = new Dictionary<menuScreen, TemplateContainer>() //Save all page's gameobject and root visual element for later use
        {
            { menuScreen.home, UI.Q<TemplateContainer>("Home_Template") },
            { menuScreen.settingsMain, UI.Q<TemplateContainer>("SettingsMain_Template") },
            { menuScreen.settingsControl, UI.Q<TemplateContainer>("Controls_Template") },
            { menuScreen.settingsVolume, UI.Q<TemplateContainer>("Volume_Template") },
            { menuScreen.howToPlay, UI.Q<TemplateContainer>("HowToPlay_Template") }
        };

        homeBtns = new Dictionary<homeButtons, Button>()
        {
            { homeButtons.Continue, pages[menuScreen.home].Q<Button>("Continue") },
            { homeButtons.New, pages[menuScreen.home].Q<Button>("New") },
            { homeButtons.Settings, pages[menuScreen.home].Q<Button>("Settings") },
            { homeButtons.Exit, pages[menuScreen.home].Q<Button>("Exit") }
        };
        settingsMainBtns = new Dictionary<settingsMainButtons, Button>()
        {
            { settingsMainButtons.Control, pages[menuScreen.settingsMain].Q<Button>("Controls") },
            { settingsMainButtons.Volume, pages[menuScreen.settingsMain].Q<Button>("Volume") },
            { settingsMainButtons.HowToPlay, pages[menuScreen.settingsMain].Q<Button>("HowToPlay") },
            { settingsMainButtons.Back, pages[menuScreen.settingsMain].Q<Button>("Back") }
        };
        settingsControlBtns = new Dictionary<settingsControlButtons, Button>()
        {
            { settingsControlButtons.Option1, pages[menuScreen.settingsControl].Q<Button>("Option_1") },
            { settingsControlButtons.Option2, pages[menuScreen.settingsControl].Q<Button>("Option_2") },
            { settingsControlButtons.Option3, pages[menuScreen.settingsControl].Q<Button>("Option_3") },
            { settingsControlButtons.Option4, pages[menuScreen.settingsControl].Q<Button>("Option_4") },
            { settingsControlButtons.Back, pages[menuScreen.settingsControl].Q<Button>("Back") }
        };
        settingsVolumeBtns = new Dictionary<settingsVolumeButtons, Button>()
        {
            { settingsVolumeButtons.Back, pages[menuScreen.settingsVolume].Q<Button>("Back") },
            { settingsVolumeButtons.Apply, pages[menuScreen.settingsVolume].Q<Button>("Apply") }
        };
        howToPlayBtns = new Dictionary<howToPlayButtons, Button>()
        {
            { howToPlayButtons.Back, pages[menuScreen.howToPlay].Q<Button>("Back") }
        };

        allSliders = new Dictionary<sliders, Slider>() { //Save all sliders to their dictionary
            { sliders.master, pages[menuScreen.settingsVolume].Q<Slider>("Master_Slider") },
            { sliders.music, pages[menuScreen.settingsVolume].Q<Slider>("Music_Slider") },
            { sliders.ui, pages[menuScreen.settingsVolume].Q<Slider>("UI_Slider") }
        };

        savedInfoManager = GameObject.Find("PermaLoader").transform.GetComponent<Saved_Info_Manager>();
        Debug.Assert(savedInfoManager != null, "Couldn't find the saved info manager");
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("existingGame") != 0) { //If theres a game saved
            homeBtns[homeButtons.Continue].SetEnabled(true); //Enable the continue button
        } else {
            homeBtns[homeButtons.Continue].SetEnabled(false); //Disable if no saved game
        }

        //Set values for volume sliders
        allSliders[sliders.master].value = 100 * PlayerPrefs.GetFloat("masterVolume");
        allSliders[sliders.music].value = 100 * PlayerPrefs.GetFloat("musicVolume");
        allSliders[sliders.ui].value = 100 * PlayerPrefs.GetFloat("uiVolume");

        NavigateTo(menuScreen.home); //Set home as curren screen
    }

    //
    // Functions run by buttons
    //
    public void Continue() { //Pressing the continue button
        Debug.Log("Continuing game");
        SceneManager.LoadScene("Gameplay");
    }
    public void New() { //Pressing the new button
        Debug.Log("New game");
        savedInfoManager.ResetPrefs();
        PlayerPrefs.SetInt("existingGame", 1);
        SceneManager.LoadScene("Gameplay");
    }
    public void Exit() { //Pressing the exit button
        Application.Quit();
        #if DEBUG //End unity debuger if debuging
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void Apply() { //Pressing the exit button
        Debug.Log("Applying changes");

        PlayerPrefs.SetFloat("masterVolume", allSliders[sliders.master].value / 100);
        PlayerPrefs.SetFloat("musicVolume", allSliders[sliders.music].value / 100);
        PlayerPrefs.SetFloat("uiVolume", allSliders[sliders.ui].value / 100);

        NavigateTo(menuScreen.settingsMain);
    }

    //Menu navigation buttons
    public void Settings() => NavigateTo(menuScreen.settingsMain);
    public void ControlsNav() => NavigateTo(menuScreen.settingsControl);
    public void VolumeNav() => NavigateTo(menuScreen.settingsVolume);
    public void HowToPlayNav() => NavigateTo(menuScreen.howToPlay);
    public void BackToSettings() => NavigateTo(menuScreen.settingsMain);
    public void BackToHome() => NavigateTo(menuScreen.home);

    private void NavigateTo(menuScreen screenTo)
    {
        Debug.Log("To "+screenTo.ToString());
        pages[currentScreen].visible = false;
        pages[currentScreen].SetEnabled(false);
        pages[screenTo].visible = true;
        pages[screenTo].SetEnabled(true);

        DisableSectionControls(currentScreen);
        EnableSectionControls(screenTo);
        currentScreen = screenTo;
    }

    //
    //  Button enabling and disabling (I hate that this has to be done like this)
    //
    private void EnableSectionControls(menuScreen section) //Turns on a sections buttons
    {
        switch (section)
        {
            case menuScreen.home:
                homeBtns[homeButtons.Continue].clicked += Continue;
                homeBtns[homeButtons.New].clicked += New;
                homeBtns[homeButtons.Settings].clicked += Settings;
                homeBtns[homeButtons.Exit].clicked += Exit;
                Debug.Log("Enabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.settingsMain:
                settingsMainBtns[settingsMainButtons.Control].clicked += ControlsNav;
                settingsMainBtns[settingsMainButtons.Volume].clicked += VolumeNav;
                settingsMainBtns[settingsMainButtons.HowToPlay].clicked += HowToPlayNav;
                settingsMainBtns[settingsMainButtons.Back].clicked += BackToHome;
                Debug.Log("Enabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.settingsControl:
                settingsControlBtns[settingsControlButtons.Back].clicked += BackToSettings;
                Debug.Log("Enabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.settingsVolume:
                settingsVolumeBtns[settingsVolumeButtons.Back].clicked += BackToSettings;
                settingsVolumeBtns[settingsVolumeButtons.Apply].clicked += Apply;
                Debug.Log("Enabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.howToPlay:
                howToPlayBtns[howToPlayButtons.Back].clicked += BackToSettings;
                Debug.Log("Enabled " + section.ToString() + "'s buttons");
                return;
        }
    }
    private void DisableSectionControls(menuScreen section) //Turns off a sections buttons
    {
        switch (section)
        {
            case menuScreen.home:
                homeBtns[homeButtons.Continue].clicked -= Continue;
                homeBtns[homeButtons.New].clicked -= New;
                homeBtns[homeButtons.Settings].clicked -= Settings;
                homeBtns[homeButtons.Exit].clicked -= Exit;
                Debug.Log("Disabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.settingsMain:
                settingsMainBtns[settingsMainButtons.Control].clicked -= ControlsNav;
                settingsMainBtns[settingsMainButtons.Volume].clicked -= VolumeNav;
                settingsMainBtns[settingsMainButtons.HowToPlay].clicked -= HowToPlayNav;
                settingsMainBtns[settingsMainButtons.Back].clicked -= BackToHome;
                Debug.Log("Disabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.settingsControl:
                settingsControlBtns[settingsControlButtons.Back].clicked -= BackToSettings;
                Debug.Log("Disabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.settingsVolume:
                settingsVolumeBtns[settingsVolumeButtons.Back].clicked -= BackToSettings;
                settingsVolumeBtns[settingsVolumeButtons.Apply].clicked -= Apply;
                Debug.Log("Disabled " + section.ToString() + "'s buttons");
                return;
            case menuScreen.howToPlay:
                howToPlayBtns[howToPlayButtons.Back].clicked -= BackToSettings;
                Debug.Log("Disabled " + section.ToString() + "'s buttons");
                return;
        }
    }
}