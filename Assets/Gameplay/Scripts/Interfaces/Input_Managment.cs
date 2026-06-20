using System.Collections.Generic;
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

    private Dictionary<inputMap, InputActionMap> inputMaps; // Stores all the action maps
    private Dictionary<inputMap, Dictionary<string, InputAction>> mappedButtons;
    private bool isGamePaused = false;

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
    }

    private void Awake() // Store all button input managers
    {
        // Locate all the input actions in the game
        mappedButtons = new Dictionary<inputMap, Dictionary<string, InputAction>>
        {
            // Gameplay
            { inputMap.Gameplay, new Dictionary<string, InputAction> {
                { "option 1", InputSystem.actions.FindAction("Option_1") },
                { "option 2", InputSystem.actions.FindAction("Option_2") },
                { "option 3", InputSystem.actions.FindAction("Option_3") },
                { "option 4", InputSystem.actions.FindAction("Option_4") },
                { "next",     InputSystem.actions.FindAction("Next")     },
                { "pause",    InputSystem.actions.FindAction("Pause")    }
            }},

            // Pause Menu
            { inputMap.PauseMenu, new Dictionary<string, InputAction> {
                { "unpause", InputSystem.actions.FindAction("Unpause") }
            }}
        };

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
        Debug.Assert(pauseMenuHandler != null, "Pause menu manager not present");
    }

    private void Start() // Set input buttons
    {
        // Setup the 4 button's actions
        optionButtons[0].clicked += ClickedBnt1;
        optionButtons[1].clicked += ClickedBnt2;
        optionButtons[2].clicked += ClickedBnt3;
        optionButtons[3].clicked += ClickedBnt4;

        mainGameProcess = transform.GetComponent<Game_Process_Manager>();
        pauseMenuHandler = transform.GetComponent<Menu_Manager>();
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
        if (mappedButtons[inputMap.PauseMenu]["unpause"].WasPressedThisFrame()) { ToggleGamePause(); }

        // If any option is pressed
        if (mappedButtons[inputMap.Gameplay]["option 1"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(0); }
        if (mappedButtons[inputMap.Gameplay]["option 2"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(1); }
        if (mappedButtons[inputMap.Gameplay]["option 3"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(2); }
        if (mappedButtons[inputMap.Gameplay]["option 4"].WasPressedThisFrame()) { mainGameProcess.OptionSelected(3); }
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
}