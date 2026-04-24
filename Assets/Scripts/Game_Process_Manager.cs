using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//Home Driving Delays event         -> more likely to late wake
//Afternoon Driving Delays event    -> cant use 2 afternoon options
//Morning Driving Delays event      -> first work skipped
public class Game_Process_Manager : MonoBehaviour
{
    private enum backgroundTime { day, afternoon, evening };
    private enum backgroundKind { bedroom, driving, officeJob, officeBreakRoom, coffeeShop, jenns, saladDeli, livingRoom, gym, resturant, pub};
    private enum option { unchosen, one, two, three, four, alergy }; //If used as bool "one" is true and "two" is false

    [SerializeField] private BackgroundSpriteInfo[] backgroundSheet;
    [SerializeField] private float textDisplayTime = 1f; //In seconds for easy alteration later

    private Dialogue_Manger dialogueSystem;
    private Reaction_And_Event_Processing reactAndEventProcessor;
    private Modal_Managment modalSystem;
    private VisualElement background;
    private Label textDisplay;
    private VisualElement optionsTemplate;
    private Button[] optionButtons = new Button[4];

    private (modalVariant variant, bool isEmotion, playerStatLevel level) testHappymodal = (modalVariant.happy, true, playerStatLevel.medium);

    private Dictionary<string, bool> savedEvents = new Dictionary<string, bool>() { //Any choice or event that might impact later options
        { "prepLunch", false },
        { "afternoonDriveDelay", false },
        { "skipFirstWork", false },
        { "skipHomeTravel", false },
        { "heavyDrinking", false },
        { "lateHomeArival", false }
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
    private UniTask currentDisplayTextTask;
    private bool isTextDisplaying = false;

    // Grab the UIDoc's various elements so they can be used later
    private void Awake() {
        //Get Main UI elements
        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        textDisplay = gameDisplay.Q<CustomUXML.UI.AspectRatioLabel>("TextBox");
        background = gameDisplay.Q<VisualElement>("Background");

        Debug.Assert(gameDisplay != null, "Couldn't find the UIDoc's root");
        Debug.Assert(textDisplay != null, "Couldn't find the text display");
        Debug.Assert(background != null, "Couldn't find the background");

        //Get option buttons
        optionsTemplate = gameDisplay.Q<TemplateContainer>("Option_Input_Template");
        optionButtons[0] = optionsTemplate.Q<Button>("Option_1");
        optionButtons[1] = optionsTemplate.Q<Button>("Option_2");
        optionButtons[2] = optionsTemplate.Q<Button>("Option_3");
        optionButtons[3] = optionsTemplate.Q<Button>("Option_4");

        Debug.Assert(optionsTemplate != null, "Couldn't find the options template");
        Debug.Assert(optionButtons[0] != null, "Couldn't find option 1 button");
        Debug.Assert(optionButtons[1] != null, "Couldn't find option 2 button");
        Debug.Assert(optionButtons[2] != null, "Couldn't find option 3 button");
        Debug.Assert(optionButtons[3] != null, "Couldn't find option 4 button");

        //Locate modal managment script
        modalSystem = transform.GetComponent<Modal_Managment>();

        Debug.Assert(optionButtons[3] != null, "Couldn't find option 4 button");
    }

    //
    // Input code activated by Input_Managment script
    //
    public void HPressed() //Temp Debug options
    {
        if (modalSystem.DoesModalExist(testHappymodal.variant, testHappymodal.level)) { //Check wether modal already exists
            modalSystem.RemoveModal(testHappymodal.variant, testHappymodal.level); //Remove it if it exists
        } else {
            modalSystem.CreateNewModal(testHappymodal.variant, testHappymodal.isEmotion, testHappymodal.level); //Add it if its missing
        }
    }

    public void OptionSelected(int option) { //Handles player input as one (Much easier)
        if (isChoiceActive) { //Ensure choice input is required
            Debug.Log("Option " + (option + 1) + " chosen");
            dialogueSystem.ChooseChoice(option);
            isChoiceActive = false;
            optionsTemplate.SetEnabled(false);
            optionsTemplate.style.display = DisplayStyle.None;
            NextDialoguePressed();
        }
    }

    public void NextDialoguePressed()
    {
        if (!isChoiceActive && !isTextDisplaying) { //Skip if choice is active or text is displaying
            char nextKind = dialogueSystem.NextDialogue();
            if (nextKind == 'D') //Sort next dialogue and check wether its a choice
            { //If dialogue
                (string dialogue, string[] tags) dialogue = dialogueSystem.GetDialogue();
                EvaliuateTags(dialogue.tags);
                currentDisplayTextTask = DisplayText(dialogue.dialogue, false); //Display text one character at a time
                if (dialogue.dialogue == "") { //If empty skip line (fixes start of section questions)
                    NextDialoguePressed();
                }
                return;
            }
            else if (nextKind == 'C')
            { //If choice
                textDisplay.text = "";
                string[] choices = dialogueSystem.GetChoices();
                optionsTemplate.SetEnabled(true);
                optionsTemplate.style.display = DisplayStyle.Flex;

                for (int i = 0; i < choices.Length; i++) { //Setup buttons that need to be active
                    optionButtons[i].style.display = DisplayStyle.Flex;
                    optionButtons[i].text = i + 1 + ". " + choices[i];
                    optionButtons[i].SetEnabled(true);
                }
                for (int i = choices.Length; i < optionButtons.Length; i++) { //Hide and clear buttons that dont need to be active
                    optionButtons[i].style.display = DisplayStyle.None;
                    optionButtons[i].text = "";
                    optionButtons[i].SetEnabled(false);
                }
                isChoiceActive = true;
                return;
            }
            else
            { //If end of Knot
                daysInfo.currentDaySection.NextSection();
                NextSectionSetup();
                return;
            }
        }
        else if (isTextDisplaying) { //If text is being displayed
            isTextDisplaying = false; //Stop displaying
            textDisplay.text = dialogueSystem.GetDialogue().dialogue;//Update text to show its completed form
            return;
        }
    }

    //
    // Initial setup
    //
    private void Start()
    {
        dialogueSystem = transform.GetComponent<Dialogue_Manger>();
        reactAndEventProcessor = transform.GetComponent<Reaction_And_Event_Processing>();

        EvaliuateChanceEvents();
        PlayerPrefs.SetInt("lateHomeArival", 0);
        PlayerPrefs.SetInt("heavyDrinking", 0);
        SetApproprateBackground(daysInfo.currentDaySection.section, "bedroom.day");
        NextSectionSetup();
    }

    private void EvaliuateChanceEvents() //Set all of todays random events
    {
        for (int i = 0; i < randomnessArray.daySections.Length; i++)
        { //For each day section
            int eventNumber = UnityEngine.Random.Range(1, 1001); //Generate a random number to be the event chosen
            if (randomnessArray.daySections[i].eventChances.Length != 0) { //If day section has no events skip event selection
                for (int f = 0; f < randomnessArray.daySections[i].eventChances.Length; f++)
                { //Calculate which event was selected
                    eventNumber -= randomnessArray.daySections[i].eventChances[f];
                    if (eventNumber <= 0) {
                        todaysChanceEvents[(daySection)i] = f+1; //Set the event ID
                        break;
                    }
                }
            } else {
                todaysChanceEvents[(daySection)i] = 0; //No event was rolled
            }
        }

        if (todaysChanceEvents[daySection.firstWork] == 3) { //Ensure both works have collegue down if selected
            todaysChanceEvents[daySection.secondWork] = 3;
        }

        int drinkingEventNumber = UnityEngine.Random.Range(1, 1001);
        if (drinkingEventNumber > randomnessArray.drinkingChances[1]) { //Check for which drinking event was chosen
            savedEvents["heavyDrinking"] = true; //Enable heavy drinking if rolled
        }

        #if DEBUG //Debug only code to output all events selected for the day
            string concatEventDebug = "";
            for (int ced = 0; ced < todaysChanceEvents.Count; ced++) {
                concatEventDebug += ((daySection)ced).ToString() + " event: " + todaysChanceEvents[(daySection)ced] + "\n";
            }
            concatEventDebug += "Heavy drinking: " + Convert.ToInt32(savedEvents["heavyDrinking"]);
            Debug.Log(concatEventDebug);
        #endif
    }

    //
    // Regular game flow
    //
    private void EvaliuateTags(string[] tags) //Evaliuate any/all tags and execute anything needed
    {
        if (tags.Length > 0) { //Skip if empty
            for (int h = 0; h < (tag.Length/2); h++) //For each tag pair
            {
                if (tags.Length == 1) { Debug.Assert(false, "TAG SERIOSLY BROKE " + tags[0]); break; }
                if (tags.Length == 0) { break; }

                string[] tagsToEval = tags.Take(2).ToArray(); //Seperate out 2 tags
                tags = tags.Skip(2).ToArray(); //And remove the 2 from the origonal array
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
                        string[] idSplit = tagsToEval[1].Split('.').ToArray(); //Split the main and sub id
                        int.TryParse(idSplit[0], out reactionIDs[0]); //Turn ids to int
                        int.TryParse(idSplit[1], out reactionIDs[1]);

                        if (reactionIDs[0] != -1 && reactionIDs[1] != -1) //Check if int cast worked
                        {
                            Debug.Log("Process reaction with ID: " + reactionIDs[0] + " & SubID: " + reactionIDs[1]);
                            reactAndEventProcessor.reactions.RollEventReaction((foodReactionSource)reactionIDs[0], reactionIDs[1]); //Run reaction chances
                        } else {
                            Debug.Assert(false, "React tag incorrectly set up");
                        }
                        break;
                    case "back":
                        SetApproprateBackground(daysInfo.currentDaySection.section, tagsToEval[1]);
                        break;
                    case "open":
                        //WIP. Will open the alergen table screen for different locations
                        Debug.Log("Open alergen table " + tagsToEval[1]);
                        break;
                    case "get":
                        Debug.Log("Get variable " + tagsToEval[1]);
                        switch (tagsToEval[1]) {
                            case "afternoonDriveDelay":
                                dialogueSystem.SetDialogueBool("afternoonDriveDelay", savedEvents["afternoonDriveDelay"]);
                                break;
                            case "heavyDrinking":
                                dialogueSystem.SetDialogueBool("heavyDrinking", savedEvents["heavyDrinking"]);
                                break;
                            case "prepLunch":
                                dialogueSystem.SetDialogueBool("prepLunch", savedEvents["prepLunch"]);
                                break;
                        }
                        break;
                    case "endday": //Executes the code to end the day. Does have a 2nd tag but its useless rn
                        EndDay();
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
                background.style.backgroundImage = new StyleBackground(backgroundSheet[i].getSprite()); //Set background
                Debug.Log("Set background to " + daysInfo.currentDaySection.section.ToString() + ", " + bgDetails);
                return; //and exit
            }
        }
        Debug.Assert(false, "Background texture asignment failed");
    }

    private void NextSectionSetup() //Does all the setup for going to the next section
    {
        int nextSection = (int)daysInfo.currentDaySection.section;
        if (nextSection == (int)daySection.firstWork && savedEvents["skipFirstWork"]) { //Skip section if skip section flag is set
            nextSection++;
            daysInfo.currentDaySection.NextSection();
        }
        else if (nextSection == (int)daySection.homeTravel && savedEvents["skipHomeTravel"]) {
            nextSection++;
            daysInfo.currentDaySection.NextSection();
        }

        Debug.Log("Up next: " + (daySection)nextSection);
        dialogueSystem.SetEvent(nextSection, todaysChanceEvents[(daySection)nextSection]); //Sets the next dialogue event
        string[] nextKnotTags = dialogueSystem.GetKnotTags("Sec" + nextSection); //Get next knot's tags
        EvaliuateTags(nextKnotTags); //Act on tags
        NextDialoguePressed();
    }

    private void EndDay() //Runs all of the end of day code along with closing the scene
    {
        PlayerPrefs.SetInt("heavyDrinking", Convert.ToInt32(savedEvents["heavyDrinking"]));
        PlayerPrefs.SetInt("lateHomeArival", Convert.ToInt32(savedEvents["lateHomeArival"]));
        SceneManager.LoadScene("Gameplay");
    }

    private async UniTask DisplayText(string textToDisplay, bool isChoice) //Takes the text to display to the user and updates it one character at a time
    {
        isTextDisplaying = true;
        textDisplay.text = "";
        int textLength = textToDisplay.Length;
        float textDisplaySpeed = textDisplayTime / textLength; //Calculate the speed based of the time
        for (int i = 0; i < textLength; i++) //For each character in the display text
        {
            if (!isTextDisplaying) { return; } //If something else tells it to stop exit
            textDisplay.text += textToDisplay[i]; //Add next character
            await UniTask.Delay((int)(textDisplaySpeed * 1000)); //Wait
        }
        isTextDisplaying = false;
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
        public backgroundKind getKind() { return kind; }
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

    //
    // The chances for all events to be rolled
    //
    private class randomnessArray
    {
        public static readonly (daySection section, int[] eventChances)[] daySections = //Chances are C/1000
        { //All day section random events and their chances (int[0] means no random events)
            new (daySection.dayStart,
                new[] {
                    75, //Early wake
                    22  //Late wake
                }
            ),
            new (daySection.workStartTravel,
                new[] {
                    45, //Car crash ahead
                    80, //Road closure
                    18  //Car doesn't start
                }
            ),
            new (daySection.firstWork,
                new[] {
                    30, //Homemade food
                    45, //Shop food
                    36, //Down colleague
                    5   //Work celebration
                }
            ),
            new (daySection.lunch, new int[0]),
            new (daySection.secondWork,
                new[] {
                    56, //Shop food
                    12  //Early end
                }
            ),
            new (daySection.workEndTravel,
                new[] {
                    50, //Car crash ahead
                    80, //Road closure
                    15  //Car doesn't start
                }
            ),
            new (daySection.afternoon, new int[0]),
            new (daySection.homeTravel,
                new[] {
                    55, //Car crash ahead
                    62  //Road closure
                }
            ),
            new (daySection.dayEnd, new int[0])
        };

        public static readonly int[] drinkingChances = new[] { //Chances are C/1000. This only exists because the party event is unique
            550, //Light drinking
            450  //Heavy drinking
        };
    }
}


