using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Managment : MonoBehaviour
{
    [SerializeField] private Game_Process_Manager mainGameProcess;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction[] buttonsPressed = new InputAction[6];

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
        buttonsPressed[4] = InputSystem.actions.FindAction("S");
        buttonsPressed[5] = InputSystem.actions.FindAction("H");
    }

    private void Update()
    {
        if (buttonsPressed[5].WasPressedThisFrame()) { //If H is pressed
            mainGameProcess.HPressed();
        }
        if (buttonsPressed[4].WasPressedThisFrame()) { //If S is pressed
            mainGameProcess.SPressed();
        }
    }
}
