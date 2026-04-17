using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Game_Process_Manager : MonoBehaviour
{
    private enum backgroundTime { day, afternoon, evening };
    private enum backgroundKind { bedroom, driving, officeJob, officeBreakRoom, coffeeShop, jenns, saladDeli, livingRoom, gym, resturant, pub};
    private enum option { unchosen, one, two, three, four, alergy }; //If used as bool "one" is true and "two" is false

    [SerializeField] private Modal_Managment modalSystem;
    [SerializeField] private BackgroundSpriteInfo[] backgroundSheet;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Dialogue_Manger dialogueSystem;
    [SerializeField] private TextMeshPro textDisplay;

    private modalInformation testHappymodal = new modalInformation(modalVariant.happy, true, playerStatLevel.medium);
    private modalInformation testSadmodal = new modalInformation(modalVariant.sad, true, playerStatLevel.medium);

    private Dictionary<string, bool> savedEvents = new Dictionary<string, bool>() { //Any choice or event that might impact later options
        { "prepLunch", false },
        { "afternoonDriveDelay", false },
        { "skipFirstWork", false },
        { "skipHomeTravel", false },
        { "heavyDrinking", false }
    };
    private Dictionary<daySection, int> todaysChanceEvents = new Dictionary<daySection, int>() { //All the current day's events (by section)
        { daySection.dayStart, 0 },
        { daySection.workStartTravel, 0 },
        { daySection.firstWork, 0 },
        { daySection.lunch, 0 },
        { daySection.secondWork, 0 },
        { daySection.workEndTravel, 0 },
        { daySection.afternoon, 0 },
        { daySection.homeTravel, 0 },
        { daySection.dayEnd, 0 }
    };

    private bool isChoiceActive = false;

    //
    // Input code activated by Input_Managment script
    //
    public void HPressed() //Temp Debug options
    {
        if (modalSystem.DoesModalExist(testHappymodal)) { //Check wether modal already exists
            modalSystem.RemoveModal(testHappymodal); //Remove it if it exists
        } else {
            modalSystem.CreateNewModal(testHappymodal); //Add it if its missing
        }
    }
    public void SPressed() //Temp Debug options
    {
        if (modalSystem.DoesModalExist(testSadmodal)) { //Check wether modal already exists
            modalSystem.RemoveModal(testSadmodal); //Remove it if it exists
        } else {
            modalSystem.CreateNewModal(testSadmodal); //Add it if its missing
        }
    }

    public void Option1Pressed(){ //All player option code
        if (isChoiceActive){
            OptionSelected(1);
        }
    }
    public void Option2Pressed(){
        if (isChoiceActive){
            OptionSelected(2);
        }
    }
    public void Option3Pressed(){
        if (isChoiceActive){
            OptionSelected(3);
        }
    }
    public void Option4Pressed(){
        if (isChoiceActive){
            OptionSelected(4);
        }
    }
    private void OptionSelected(int option) {
    
    }

    public void NextDialoguePressed()
    {
        if (!isChoiceActive) { //Skip if choice is active
            char nextKind = dialogueSystem.NextDialogue();
            if (nextKind == 'D') //Sort next dialogue and check wether its a choice
            { //If dialogue
                (string dialogue, string[] tags) dialogue = dialogueSystem.GetDialogue();
                EvaliuateTags(dialogue.tags);
                textDisplay.text = dialogue.dialogue;
            }
            else if (nextKind == 'C')
            { //If choice
                string[] choices = dialogueSystem.GetChoices();
                string choiceDisplayConcat = "";
                for (int i = 0; i < choices.Length; i++) //Turn the set of choices into text
                {
                    choiceDisplayConcat += i + 1 + ". " + choices[i] + "\n";
                }
                textDisplay.text = choiceDisplayConcat;
            }
            else
            { //If end of Knot
                daysInfo.currentDaySection.NextSection();
                string[] nextKnotTags = dialogueSystem.GetKnotTags("Sec" + daysInfo.currentDaySection.section);
                EvaliuateTags(nextKnotTags);
            }
        }
    }

    //
    // Functions that handle regular game flow
    //
    private void Start()
    {
        EvaliuateChanceEvents();
        playerStats.ResetDriveDelayBool();
        SetApproprateBackground(daysInfo.currentDaySection.section, "bedroom.day");
        dialogueSystem.SetEvent((int)daySection.dayStart, todaysChanceEvents[daysInfo.currentDaySection.section]);
        NextDialoguePressed();
    }
    //Home Driving Delays event         -> more likely to late wake
    //Afternoon Driving Delays event    -> cant use 2 afternoon options
    //Morning Driving Delays event      -> first work skipped

    private void EvaliuateChanceEvents() //Set all of todays random events
    {
        for (int i = 0; i < randomnessArray.daySections.Length; i++)
        { //For each day section
            int eventNumber = Random.Range(1, 1000); //Generate a random number to be the event chosen
            if (randomnessArray.daySections[i].eventChances.Length != 0) { //If day section has no events skip event selection
                for (int f = 0; f < randomnessArray.daySections[i].eventChances.Length; f++)
                { //Calculate which event was selected
                    eventNumber -= randomnessArray.daySections[i].eventChances[f];
                    if (eventNumber <= 0) {
                        todaysChanceEvents[(daySection)i] = f; //Set the event ID 
                        Debug.Log(((daySection)i).ToString() + " event: " + f);
                    }
                }
            }
            todaysChanceEvents[(daySection)i] = 0; //No event was rolled
        }

        if (todaysChanceEvents[daySection.firstWork] == 3) { //Ensure both works have collegue down if selected
            todaysChanceEvents[daySection.secondWork] = 3;
        }
    }

    private void EvaliuateTags(string[] tags) //Evaliuate any/all tags and execute anything needed
    {
        if (tags.Length > 0) { //Skip if empty
            for (int h = 0; h < (tag.Length/2); h++) //For each tag pair
            {
                string[] tagsToEval = tags.Take(2).ToArray(); //Seperate out 2 tags
                tags = tags.Skip(2).ToArray(); //And remove the 2 from the origonal array
                switch (tagsToEval[0].ToLower()) //Execute appropriate action
                {
                    case "save": //Save information
                        savedEvents[tagsToEval[1]] = true;
                        break;
                    case "savehigher": //Save information to universal code
                        switch (tagsToEval[1]) {
                            case "lateHomeArival":
                                playerStats.EveningDriveDelayed();
                                break;
                        }
                        break;
                    case "react":
                        int[] reactionIDs = new[] { -1, -1 };
                        string[] idSplit = tagsToEval[1].Split('.').ToArray(); //Split the main and sub id
                        int.TryParse(idSplit[0], out reactionIDs[0]); //Turn ids to int
                        int.TryParse(idSplit[1], out reactionIDs[1]);

                        if (reactionIDs[0] != -1 && reactionIDs[1] != -1) //Check if int cast worked
                        {
                            //UTILISE REACTION PROCESSING BASED ON ID
                        }
                        else
                        {
                            Debug.Assert(false, "React tag incorrectly set up");
                        }
                        break;
                    case "back":
                        SetApproprateBackground(daysInfo.currentDaySection.section, tagsToEval[1]);
                        break;
                    case "open":
                        //WIP. Will open the alergen table screen for different locations
                        Debug.Log(tagsToEval[1]+" alergen table is supposed to open here");
                        break;
                    case "get":
                        switch (tagsToEval[1]) {
                            case "afternoonDriveDelay":
                                dialogueSystem.SetDialogueBool("afternoonDriveDelay", savedEvents["afternoonDriveDelay"]);
                                break;
                            case "heavyDrinking":
                                dialogueSystem.SetDialogueBool("heavyDrinking", savedEvents["heavyDrinking"]);
                                break;
                        }
                        break;

                }
            }
        }
    }

    private void SetApproprateBackground(daySection section, string bgDetails) //Get the proper background sprite
    {
        string[] splitDetails = bgDetails.Split('.');
        string bgKind = splitDetails[0];
        string bgTime = splitDetails[1];

        for (int i = 0; i < backgroundSheet.Length; i++) { //Locate the correct sprite for the input section
            if (backgroundSheet[i].isSection(section) && backgroundSheet[i].isKind(bgKind) && backgroundSheet[i].isTime(bgTime)) //If the background in tag
            {
                background.sprite = backgroundSheet[i].getSprite(); //Set background
                return; //and exit
            }
        }
        Debug.Assert(false, "Background texture asignment failed");
    }

    //
    // All information about the current day. Resets on the next day due to the scene being re-made
    //
    private static class daysInfo
    {
        public static class currentDaySection
        {
            public static daySection section { get; private set; } = daySection.dayStart;
            public static void NextSection() { //Move to next day section
                if (section != daySection.dayEnd) {
                    section = (daySection)((int)section + 1);
                } else {
                    section = daySection.dayStart;
                }
            }
        }
    }

    //
    // This class helps simplify the process of saving and using the background textures
    //
    [System.Serializable]
    private class BackgroundSpriteInfo
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private backgroundKind kind;
        [SerializeField] private backgroundTime time;
        [SerializeField] private daySection section;

        public Sprite getSprite() { return sprite; }

        // 3 bellow check if background is same as one searched for
        public bool isKind(string inp) {
            if (inp == kind.ToString()) {
                return true;
            }
            else { return false;  }
        }
        public bool isTime(string inp) {
            if (inp == time.ToString()) {
                return true;
            }
            else { return false; }
        }
        public bool isSection(daySection inp) {
            if (inp == section) {
                return true;
            }
            else { return false; }
        }
    }
}


