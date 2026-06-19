using UnityEngine;
using UnityEngine.UIElements;

public class Pause_Menu_Manager : MonoBehaviour
{


    private void Awake()
    {
        // Save the 4 option selecting buttons
        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        //optionButtons[0] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_1");
        //optionButtons[1] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_2");
        //optionButtons[2] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_3");
        //optionButtons[3] = gameDisplay.Q<CustomUXML.UI.AspectRatioButton>("Option_4");
    }
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
