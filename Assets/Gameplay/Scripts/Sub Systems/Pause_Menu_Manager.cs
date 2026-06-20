using UnityEngine;
using UnityEngine.UIElements;

public class Pause_Menu_Manager : MonoBehaviour
{
    /*// Stores all buttons on the pause menu
    private Dictionary<screen, (Dictionary<int, Button> Buttons, Dictionary<sliders, Slider> Sliders)> menuElements;
    private Dictionary<screen, TemplateContainer> pageTemplate; //Stores all page's TemplateContainer

    private void Awake()
    {
        // Save the 4 option selecting buttons
        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        //optionButtons[0] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_1");
        //optionButtons[1] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_2");
        //optionButtons[2] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_3");
        //optionButtons[3] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_4");

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
    }*/
    // Enable the pause menu (from Input_Managment)
    public void EnableMenu()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
