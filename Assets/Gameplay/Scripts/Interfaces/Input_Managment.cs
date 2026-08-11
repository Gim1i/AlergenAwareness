using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Input_Managment : MonoBehaviour
{
    enum inputMap { Gameplay, PauseMenu }

    [SerializeField] private InputActionAsset inputActions;

    private Game_Process_Manager mainGameProcess;
    private Menu_Manager pauseMenuHandler;
    private Button[] optionButtons = new Button[4];
    private List<Button> modalButtons = new List<Button>();

    private Dictionary<inputMap, InputActionMap> inputMaps; // Stores all the action maps
    private Dictionary<inputMap, Dictionary<string, InputAction>> mappedButtons;
    private bool isGamePaused = false;

    #if DEBUG // Unity Debugger exclusive debugging controls
    private Dictionary<string, InputAction> debugControls;
    private Reaction_And_Event_Processing emotionController;
    #endif

    //
    // Handles al input managment
    //
    private void OnEnable()
    {
        inputMaps = new Dictionary<inputMap, InputActionMap>
        {
            { inputMap.Gameplay,  inputActions.FindActionMap("Gameplay")  },
            { inputMap.PauseMenu, inputActions.FindActionMap("PauseMenu") }
        };

        inputMaps[inputMap.Gameplay].Enable();

        #if DEBUG // Enable the debug controls
        inputActions.FindActionMap("Debug").Enable();
        #endif
    }

    private void Awake() // Store all button input managers
    {
        // Locate all the input actions in the game
        mappedButtons = new Dictionary<inputMap, Dictionary<string, InputAction>>
        {
            // Gameplay
            { inputMap.Gameplay, new Dictionary<string, InputAction> {
                { "next",     InputSystem.actions.FindAction("Next")     },
                { "option 1", InputSystem.actions.FindAction("Option_1") },
                { "option 2", InputSystem.actions.FindAction("Option_2") },
                { "option 3", InputSystem.actions.FindAction("Option_3") },
                { "option 4", InputSystem.actions.FindAction("Option_4") },
                { "pause",    InputSystem.actions.FindAction("Pause")    }
            }},

            // Pause Menu
            { inputMap.PauseMenu, new Dictionary<string, InputAction> {
                { "unpause", InputSystem.actions.FindAction("Unpause") }
            }}
        };

        #if DEBUG // Locate all debug inputs
        debugControls = new Dictionary<string, InputAction> {
            { "tired",          InputSystem.actions.FindAction("Tired")           },
            { "stress",         InputSystem.actions.FindAction("Stress")          },
            { "happy",          InputSystem.actions.FindAction("Happy")           },
            { "tinglingThroat", InputSystem.actions.FindAction("Tingling_Throat") },
            { "sick",           InputSystem.actions.FindAction("Sick")            },
            { "clearAll",       InputSystem.actions.FindAction("Clear_All")       }
        };

        emotionController = transform.GetComponent<Reaction_And_Event_Processing>();
        Debug.Assert(emotionController != null, "Emotion controller not present");
        #endif

        // Save the 4 option selecting buttons
        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        optionButtons[0] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_1");
        optionButtons[1] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_2");
        optionButtons[2] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_3");
        optionButtons[3] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_4");

        Debug.Assert(optionButtons[0] != null, "Option 1 button missing");
        Debug.Assert(optionButtons[1] != null, "Option 2 button missing");
        Debug.Assert(optionButtons[2] != null, "Option 3 button missing");
        Debug.Assert(optionButtons[3] != null, "Option 4 button missing");

        pauseMenuHandler = transform.GetComponent<Menu_Manager>();
        mainGameProcess = transform.GetComponent<Game_Process_Manager>();

        Debug.Assert(pauseMenuHandler != null, "Pause menu manager not present");
        Debug.Assert(mainGameProcess  != null, "Main game manager not present" );
    }

    private void Start() // Set input buttons
    {
        // Setup the 4 button's actions
        optionButtons[0].clicked += ClickedBnt1;
        optionButtons[1].clicked += ClickedBnt2;
        optionButtons[2].clicked += ClickedBnt3;
        optionButtons[3].clicked += ClickedBnt4;
    }

    private void Update() // All keyboard inputs (And tapping/clicking a screen)
    {
        // If any valid button to got to the next line of dialogue is pressed
        if (mappedButtons[inputMap.Gameplay]["next"].WasPressedThisFrame())
        {
            mainGameProcess.NextDialoguePressed();
        }

        // Pausing the game
        if (mappedButtons[inputMap.Gameplay]["pause"].WasPressedThisFrame()) { ToggleGamePause(); }

        // Unpausing the game
        else if (mappedButtons[inputMap.PauseMenu]["unpause"].WasPressedThisFrame()) { ToggleGamePause(); }

        // If any option is pressed
        if      (mappedButtons[inputMap.Gameplay]["option 1"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(0); }
        else if (mappedButtons[inputMap.Gameplay]["option 2"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(1); }
        else if (mappedButtons[inputMap.Gameplay]["option 3"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(2); }
        else if (mappedButtons[inputMap.Gameplay]["option 4"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(3); }

        #if DEBUG
        if      (debugControls["tired"         ].WasPressedThisFrame()) { emotionController.reactions.IncreaseEmotionForTest("tired"); }
        else if (debugControls["stress"        ].WasPressedThisFrame()) { emotionController.reactions.IncreaseEmotionForTest("stress"); }
        else if (debugControls["happy"         ].WasPressedThisFrame()) { emotionController.reactions.IncreaseEmotionForTest("happy"); }
        else if (debugControls["tinglingThroat"].WasPressedThisFrame()) { emotionController.reactions.SetAfflictForTest("tinglingThroat"); }
        else if (debugControls["sick"          ].WasPressedThisFrame()) { emotionController.reactions.SetAfflictForTest("sick"); }
        else if (debugControls["clearAll"      ].WasPressedThisFrame()) { emotionController.reactions.TestingClearAll(); }
        #endif
    }

    //
    // Pausing the game when the Pause Menu is up
    //
    private void ToggleGamePause()
    {
        pauseMenuHandler.ToggleEntireMenuVisability();
        if (isGamePaused)
        {
            // Re-enable the buttons
            optionButtons[0].clicked += ClickedBnt1;
            optionButtons[1].clicked += ClickedBnt2;
            optionButtons[2].clicked += ClickedBnt3;
            optionButtons[3].clicked += ClickedBnt4;

            // Turn on the game controls again
            inputMaps[inputMap.Gameplay].Enable();
            inputMaps[inputMap.PauseMenu].Disable();

            mainGameProcess.ChangeGameProcessManagerPauseState(false);
            isGamePaused = false;
        }
        else
        {
            // Disable the buttons
            optionButtons[0].clicked -= ClickedBnt1;
            optionButtons[1].clicked -= ClickedBnt2;
            optionButtons[2].clicked -= ClickedBnt3;
            optionButtons[3].clicked -= ClickedBnt4;

            // Turn off the game controls
            inputMaps[inputMap.Gameplay].Disable();
            inputMaps[inputMap.PauseMenu].Enable();

            mainGameProcess.ChangeGameProcessManagerPauseState(true);
            isGamePaused = true;
        }
    }

    public void ClickedBnt1() { mainGameProcess.OptionSelected(0); }
    public void ClickedBnt2() { mainGameProcess.OptionSelected(1); }
    public void ClickedBnt3() { mainGameProcess.OptionSelected(2); }
    public void ClickedBnt4() { mainGameProcess.OptionSelected(3); }

    //
    // Modal button stuff
    //
    public void UpdateModalButtons(Button[] newButtonArray) { // Updating the modal buttons list when the modals have changed
        for (int b = 0; b < modalButtons.Count; b++) { // Remove all the previous buttons
            modalButtons[b].clickable.clickedWithEventInfo -= ModalButtonClicked;
            modalButtons[b].clicked -= ModalButtonTest;
        }

        modalButtons = newButtonArray.ToList();
        for (int b = 0; b < modalButtons.Count; b++) { // Add all buttons back
            modalButtons[b].clickable.clickedWithEventInfo += ModalButtonClicked;
            modalButtons[b].clicked += ModalButtonTest;
        }
        Debug.Log("Updated modal buttons");
        Debug.Log(modalButtons.Count + " Buttons");
    }

    // Handles displaying the label when a modal button is clicked
    private void ModalButtonClicked(EventBase buttonClicked)
    {
        Debug.Log(buttonClicked);
        Debug.Log(buttonClicked.ToString());
    }
    public void ModalButtonTest()
    {
        Debug.Log("Worked");
    }
}