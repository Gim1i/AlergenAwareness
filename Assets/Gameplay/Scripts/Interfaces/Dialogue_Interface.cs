using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class Dialogue_Manger : MonoBehaviour
{
    private enum storyElement { dialogue, choice, end }

    [SerializeField] private TextAsset inkTextAsset;
    private Reaction_And_Event_Processing emotionAndEventManager;
    private Game_Process_Manager gameProcessManager;
    private Story dialogueStoryStore;

    private int currentEvent = 0;

    private Dictionary<string, bool> savedEvents = new Dictionary<string, bool>() { //Any choice or event that might impact later options
        { "prepLunch", false },
        { "afternoonDriveDelay", false },
        { "skipFirstWork", false },
        { "skipHomeTravel", false },
        { "heavyDrinking", false },
        { "lateHomeArival", false }
    };

    void Awake() {
        dialogueStoryStore = new Story(inkTextAsset.text); //Sets up the story variable

        //Set the reaction and event manager for later use
        emotionAndEventManager = transform.GetComponent<Reaction_And_Event_Processing>();
        gameProcessManager = transform.GetComponent<Game_Process_Manager>();
    }

    //
    // Dialogue
    //
    public (int storyElement, string[] text) NextDialogue() //Handles all dialogue processing and conveys which story element its on
    {
        if (dialogueStoryStore.canContinue) //If theres dialogue next
        {
            Debug.Log("Dialogue");
            string[] dialogue = new string[1] { dialogueStoryStore.Continue() }; //Move to the next line and save the text in it
            EvaliuateTags(dialogueStoryStore.currentTags.ToArray()); //Evaliuate any tags
            return ((int)storyElement.dialogue, dialogue);
        }
        else if (dialogueStoryStore.currentChoices.Count > 0) //If there is no dialogue and choice/s
        {
            Debug.Log("Choice");
            Choice[] choicesTempStore = dialogueStoryStore.currentChoices.ToArray(); //Temporarilly store the choices
            string[] choices = new string[choicesTempStore.Length]; //Create an array of the proper length
            for (int i = 0; i < choicesTempStore.Length; i++)
            {
                choices[i] = choicesTempStore[i].text; //Move the text from each choice to a new array
            }
            return ((int)storyElement.choice, choices);
        }
        else //When Knot ends
        {
            Debug.Log("End");
            return ((int)storyElement.end, new string[0]);
        }
    }

    //
    // Tags
    //
    private void EvaliuateTags(string[] tags) //Evaliuate any/all tags and execute anything needed
    {
        string[] tagsToDo = tags.ToArray();
        if (tags.Length > 0) { //Skip if empty
            for (int h = 0; h < (tags.Length/2); h++) //For each tag pair
            {
                if (tagsToDo.Length == 1) { Debug.Assert(false, "TAG SERIOSLY BROKE " + tagsToDo[0]); break; }
                if (tagsToDo.Length == 0) { break; }

                string[] tagsToEval = tagsToDo.Take(2).ToArray(); //Seperate out 2 tags
                tagsToDo = tagsToDo.Skip(2).ToArray(); //And remove the 2 from the origonal array
                tagsToEval[0] = tagsToEval[0].Trim();
                tagsToEval[1] = tagsToEval[1].Trim();
                Debug.Log(tagsToEval[0] + ":" + tagsToEval[1]);
                switch (tagsToEval[0].ToLower()) //Execute appropriate action
                {
                    case "save": //Save information
                        savedEvents[tagsToEval[1]] = true;
                        Debug.Log(tagsToEval[1]+" set");
                        break;
                    case "react":
                        int[] reactionIDs = new[] { -1, -1 };
                        string[] idSplit = tagsToEval[1].Split('.'); //Split the main and sub id
                        int.TryParse(idSplit[0], out reactionIDs[0]); //Turn ids to int
                        int.TryParse(idSplit[1], out reactionIDs[1]);

                        if (reactionIDs[0] != -1 && reactionIDs[1] != -1) //Check if int cast worked
                        {
                            Debug.Log("Process reaction with ID: " + reactionIDs[0] + " & SubID: " + reactionIDs[1]);
                            emotionAndEventManager.reactions.RollEventReaction((Reaction_And_Event_Processing.Reactions.foodReactionSource)reactionIDs[0], reactionIDs[1]);
                        } else {
                            Debug.Assert(false, "React tag incorrectly set up");
                        }
                        break;
                    case "back":
                        gameProcessManager.SetApproprateBackground(tagsToEval[1]);
                        break;
                    case "open":
                        //WIP. Will open the alergen table screen for different locations
                        Debug.Log("Open alergen table " + tagsToEval[1]);
                        break;
                    case "get":
                        Debug.Log("Get variable " + tagsToEval[1]);
                        switch (tagsToEval[1]) {
                            case "afternoonDriveDelay":
                                dialogueStoryStore.variablesState["afternoonDriveDelay"] = savedEvents["afternoonDriveDelay"];
                                break;
                            case "heavyDrinking":
                                dialogueStoryStore.variablesState["heavyDrinking"] = savedEvents["heavyDrinking"];
                                break;
                            case "prepLunch":
                                dialogueStoryStore.variablesState["prepLunch"] = savedEvents["prepLunch"];
                                break;
                        }
                        break;
                    case "endday": //Executes the code to end the day. Does have a 2nd tag but its useless rn
                        PlayerPrefs.SetInt("heavyDrinking", Convert.ToInt32(savedEvents["heavyDrinking"]));
                        PlayerPrefs.SetInt("lateHomeArival", Convert.ToInt32(savedEvents["lateHomeArival"]));
                        SceneManager.LoadScene("Gameplay");
                        break;
                    case "prefchange": //Changes a PlayerPref by the value specified
                        emotionAndEventManager.emotions.UpdatePlayerPref(tagsToEval[1]);
                        break;
                    case "event": //Event related stuff
                        string[] eventStuffSplit = tagsToEval[1].Split('.');
                        switch (eventStuffSplit[0]) {
                            case "set": //Set the next event to the proper place
                                SetEvent(eventStuffSplit[1], currentEvent);
                                break;
                        }
                        break;
                    default:
                        Debug.Assert(false, "Unable to identify tag of type " + tagsToEval[0]);
                        break;
                }
            }
        }
	}

    //
    // Events
    //
    public void SetEvent(string section, int eventID) //Sets the next event by stitch
    {
        dialogueStoryStore.ChoosePathString(section+".Ev"+eventID);
        Debug.Log("Event set to: " + section + ".Ev" + eventID);
    }
    public void ChooseChoice(int index) { dialogueStoryStore.ChooseChoiceIndex(index); }
    public void SetSavedEvent(string eventID, bool value) { savedEvents[eventID] = value; }

    //
    // Other
    //
    public void SetupSection(int sectionID, int eventID) //Does any dialogue processing for going to the next section
    {
        List<string> nextKnotTags = dialogueStoryStore.TagsForContentAtPath("Sec" + sectionID);
        currentEvent = eventID;
        if (nextKnotTags != null) //Evaluate all tags for this section
        {
            EvaliuateTags(nextKnotTags.ToArray());
        }
    }

    public bool GetSavedEvent(string eventName) { return savedEvents[eventName]; }
}
