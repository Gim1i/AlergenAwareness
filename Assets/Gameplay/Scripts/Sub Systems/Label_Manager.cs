using UnityEngine;
using UnityEngine.UIElements;

public class Label_Manager : MonoBehaviour
{
    [SerializeField] private string labelTemplateName;

    private VisualElement labelBaseElement;

    private void Awake()
    {
        Debug.Assert(labelTemplateName != null, "Label template name not assigned"); // Locates the Label's base element
        labelBaseElement = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement.Q<TemplateContainer>(labelTemplateName).Q<VisualElement>("Label");
    }

    //
    // Label related controls
    //
    public void CreateLabel(bool isStationary, string labelContent) // "Creates" (makes visable) a label
    {
        Label labelText = labelBaseElement.Q<Label>("Label_Text"); // Get the text element in the label
        labelText.text = labelContent; // Assign it the proper content

        Vector3 clickSpot = Input.mousePosition;
        Debug.Log(clickSpot.x + ", " + clickSpot.y);
        if (isStationary){
            labelBaseElement.style.marginLeft = clickSpot.x;
            labelBaseElement.style.marginTop = clickSpot.y;
        }
        else {
            
        }

        labelBaseElement.parent.visible = true;
    }
}
