using UnityEngine;
using Ink.Runtime;

public class Dialogue_Manger : MonoBehaviour
{
    [SerializeField] private TextAsset inkTextAsset;
    private Story dialogueStoryStore;
    private (string dialogue, string[] tags) currentDialogue = ("", new[] { "" });
    private (string choice, string[] tags)[] currentChoices = new[] {("", new[] { "" })};

    void Start() {
        dialogueStoryStore = new Story(inkTextAsset.text); //Sets up the story variable
    }

    public char NextDialogue() //Handles all dialogue processing and conveys wether its a choice or not.   C is choice, D is dialogue, E is end
    {
        if (dialogueStoryStore.canContinue) //If theres dialogue next
        {
            currentDialogue.dialogue = dialogueStoryStore.Continue(); //Store the text
            currentDialogue.tags = dialogueStoryStore.currentTags.ToArray(); //and the tags
            currentChoices = new[] { ("", new[] { "" }) }; //Clear the choice store (redundancy)
            return 'D'; //And return false
        }
        else if (dialogueStoryStore.currentChoices.Count > 0) //If there is no dialogue and choice/s
        {
            Choice[] choicesTempStore = dialogueStoryStore.currentChoices.ToArray(); //Store choices in a new store
            currentChoices = new (string choice, string[] tags)[choicesTempStore.Length]; //Re-make the choices store so its the apropriate length
            for (int i = 0; i > choicesTempStore.Length; i--) { //Store each choice in an array (like dialogue)
                currentChoices[i].choice = choicesTempStore[i].text;
                currentChoices[i].tags = choicesTempStore[i].tags.ToArray();
            }
            currentDialogue = ("", new[] { "" }); //Clear the dialogue store (redundancy)
            return 'C'; //And return true
        }
        else //When Knot ends
        {
            currentDialogue = ("", new[] { "" }); //Clear stores for redundancy
            currentChoices = new[] { ("", new[] { "" }) };
            return 'E';
        }
    }

    public void SetEvent(daySection section, int eventID) //Sets the next event by stitch
    {
        dialogueStoryStore.ChoosePathString("Sec"+section+".Ev"+eventID);
    }

    public (string dialogue, string[] tags) GetDialogue(){ return currentDialogue; }
    public (string dialogue, string[] tags)[] GetChoices(){ return currentChoices; }
}
