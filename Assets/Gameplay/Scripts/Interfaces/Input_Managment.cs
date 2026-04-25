using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Input_Managment : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private Game_Process_Manager mainGameProcess;
    private InputAction[] buttonsPressed = new InputAction[7];
    private Button[] optionButtons = new Button[4];

    #if DEBUG
    private Reaction_And_Event_Processing reactions;
    private InputAction[] testingReactions = new InputAction[3];
    #endif
    //
    // Handles al input managment
    //
    private void OnEnable()
    {
        inputActions.FindActionMap("Controls").Enable();
    }

    private void Awake() //Store all button input managers
    {
        buttonsPressed[0] = InputSystem.actions.FindAction("Option_1");
        buttonsPressed[1] = InputSystem.actions.FindAction("Option_2");
        buttonsPressed[2] = InputSystem.actions.FindAction("Option_3");
        buttonsPressed[3] = InputSystem.actions.FindAction("Option_4");
        buttonsPressed[4] = InputSystem.actions.FindAction("Next");

        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        optionButtons[0] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_1");
        optionButtons[1] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_2");
        optionButtons[2] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_3");
        optionButtons[3] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_4");

        Debug.Assert(optionButtons[0] != null, "Option 1 button missing");
        Debug.Assert(optionButtons[1] != null, "Option 2 button missing");
        Debug.Assert(optionButtons[2] != null, "Option 3 button missing");
        Debug.Assert(optionButtons[3] != null, "Option 4 button missing");

        testingReactions[0] = InputSystem.actions.FindAction("J");
        testingReactions[1] = InputSystem.actions.FindAction("K");
        testingReactions[2] = InputSystem.actions.FindAction("L");
    }

    private void Start() //Set input buttons up
    {
        optionButtons[0].clicked += ClickedBnt1;
        optionButtons[1].clicked += ClickedBnt2;
        optionButtons[2].clicked += ClickedBnt3;
        optionButtons[3].clicked += ClickedBnt4;

        mainGameProcess = transform.GetComponent<Game_Process_Manager>();

        #if DEBUG
        reactions = transform.GetComponent<Reaction_And_Event_Processing>();
        #endif
    }

private void Update()
    {
        if (buttonsPressed[4].WasPressedThisFrame()) { //If any valid button to got to the next line of dialogue is pressed
            mainGameProcess.NextDialoguePressed();
        }
        for (int i = 0; i < 4; i++) //Run though the 4 option inputs
        {
            if (buttonsPressed[i].WasPressedThisFrame()) { //If option pressed
                mainGameProcess.OptionSelected(i);
            }
        }

        #if DEBUG
        for (int i = 0; i < 3; i++) { //Run though the 3 test inputs
            if (testingReactions[i].WasPressedThisFrame()) { //If input pressed
                reactions.reactions.TestReactions(i);
            }
        }
        #endif
    }

    public void ClickedBnt1() { mainGameProcess.OptionSelected(0); }
    public void ClickedBnt2() { mainGameProcess.OptionSelected(1); }
    public void ClickedBnt3() { mainGameProcess.OptionSelected(2); }
    public void ClickedBnt4() { mainGameProcess.OptionSelected(3); }
}
