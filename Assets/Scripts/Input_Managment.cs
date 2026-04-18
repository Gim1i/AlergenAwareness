using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Managment : MonoBehaviour
{
    [SerializeField] private Game_Process_Manager mainGameProcess;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction[] buttonsPressed = new InputAction[7];

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
    }

    private void Update()
    {
        if (buttonsPressed[4].WasPressedThisFrame()) { // If any valid button to got to the next line of dialogue is pressed
            mainGameProcess.NextDialoguePressed();
        }
        for (int i = 0; i < 4; i++) //Run though the 4 option inputs
        {
            if (buttonsPressed[i].WasPressedThisFrame()) { //If option pressed
                mainGameProcess.OptionSelected(i);
            }
        }
    }
}
