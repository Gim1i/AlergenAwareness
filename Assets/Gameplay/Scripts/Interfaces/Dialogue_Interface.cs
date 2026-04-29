using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class Dialogue_Manger : MonoBehaviour
{
    [SerializeField] private TextAsset inkTextAsset;
    private Story dialogueStoryStore;
    private (string dialogue, string[] tags) currentDialogue = ("", new[] { "" });
    private string[] currentChoices = new[] {""};

    void Awake() {
        dialogueStoryStore = new Story(inkTextAsset.text); //Sets up the story variable
    }

    public char NextDialogue() //Handles all dialogue processing and conveys wether its a choice or not.   C is choice, D is dialogue, E is end
    {
        if (dialogueStoryStore.canContinue) //If theres dialogue next
        {
            Debug.Log("Dialogue");
            currentDialogue.dialogue = dialogueStoryStore.Continue(); //Store the text
            currentDialogue.tags = dialogueStoryStore.currentTags.ToArray(); //and the tags
            currentChoices = new[] { "" }; //Clear the choice store (redundancy)
            return 'D'; //And return false
        }
        else if (dialogueStoryStore.currentChoices.Count > 0) //If there is no dialogue and choice/s
        {
            Debug.Log("Choice");
            Choice[] choicesTempStore = dialogueStoryStore.currentChoices.ToArray(); //Store choices in a new store
            currentChoices = new string[choicesTempStore.Length]; //Re-make the choices store so its the apropriate length
            for (int i = 0; i < choicesTempStore.Length; i++) { //Store each choice in an array (like dialogue)
                currentChoices[i] = choicesTempStore[i].text;
            }
            currentDialogue = ("", new[] { "" }); //Clear the dialogue store (redundancy)
            return 'C'; //And return true
        }
        else //When Knot ends
        {
            Debug.Log("End");
            currentDialogue = ("", new[] { "" }); //Clear stores for redundancy
            currentChoices = new[] { "" };
            return 'E';
        }
    }

    public void SetEvent(int section, int eventID) //Sets the next event by stitch
    {
        dialogueStoryStore.ChoosePathString("Sec"+section+".Ev"+eventID);
        Debug.Log("Event set to: Sec" + section + ".Ev" + eventID);
    }
    public string[] GetKnotTags(string knot)
    {
        List<string> nextKnotTags = dialogueStoryStore.TagsForContentAtPath(knot);
        Debug.Log(nextKnotTags.Count);
        if (nextKnotTags != null) {
            return nextKnotTags.ToArray();
        } else { return new string[0]; }
    }
    public (string dialogue, string[] tags) GetDialogue() { return currentDialogue; }
    public string[] GetChoices() { return currentChoices; }
    public void SetDialogueBool(string boolName, bool inp) { dialogueStoryStore.variablesState[boolName] = inp; }
    public void ChooseChoice(int index) { dialogueStoryStore.ChooseChoiceIndex(index); }
}
