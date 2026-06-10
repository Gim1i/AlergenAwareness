using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//Home Driving Delays event         -> more likely to late wake
public class Game_Process_Manager : MonoBehaviour
{
    public enum daySection { dayStart, workStartTravel, firstWork, lunch, secondWork, workEndTravel, afternoon, homeTravel, dayEnd }

    private enum backgroundTime { day, afternoon, evening };
    private enum backgroundKind { bedroom, driving, officeJob, officeBreakRoom, coffeeShop, jenns, saladDeli, livingRoom, gym, resturaunt, pub};
    private enum option { unchosen, one, two, three, four, alergy }; //If used as bool "one" is true and "two" is false

    [SerializeField] private BackgroundSpriteSet[] backgroundSet;
    [SerializeField] private float textDisplayTime = 1f; //In seconds for easy alteration later

    private Dialogue_Manger dialogueSystem;
    private Reaction_And_Event_Processing emotionAndEventProcessor;
    private Modal_Managment modalSystem;
    private VisualElement background;
    private Label textDisplay;
    private VisualElement optionsTemplate;
    private Button[] optionButtons = new Button[4];

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
    private string currentDialogueText = "";

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
        if (isChoiceActive) { return; } //Skip if choice is active

		if (!isChoiceActive && !isTextDisplaying) { //If text isn't displaying
            (int storyElement, string[] text) nextdialogue = dialogueSystem.NextDialogue();
            if (nextdialogue.storyElement == 0) //Sort next dialogue and check wether its a choice
            { //If dialogue
                currentDialogueText = nextdialogue.text[0];
                currentDisplayTextTask = DisplayText(nextdialogue.text[0], false); //Display text one character at a time
                if (nextdialogue.text[0] == "") { //If empty skip line (fixes start of section questions)
                    NextDialoguePressed();
                }
                return;
            }
            else if (nextdialogue.storyElement == 1)
            { //If choice
                textDisplay.text = "";
                optionsTemplate.SetEnabled(true);
                optionsTemplate.style.display = DisplayStyle.Flex;

                for (int i = 0; i < nextdialogue.text.Length; i++) { //Setup buttons that need to be active
                    optionButtons[i].style.display = DisplayStyle.Flex;
                    optionButtons[i].text = i + 1 + ". " + nextdialogue.text[i];
                    optionButtons[i].SetEnabled(true);
                }
                for (int i = nextdialogue.text.Length; i < optionButtons.Length; i++) { //Hide and clear buttons that dont need to be active
                    optionButtons[i].style.display = DisplayStyle.None;
                    optionButtons[i].text = "";
                    optionButtons[i].SetEnabled(false);
                }
                isChoiceActive = true;
                return;
            }
            else
            { //If end of Knot
                NextSection();
                return;
            }
        }
        else if (isTextDisplaying) { //If text is being displayed
            isTextDisplaying = false; //Stop displaying
            textDisplay.text = currentDialogueText;//Update text to show its completed form
            return;
        }
    }

    //
    // Initial setup
    //
    private void Start()
    {
        dialogueSystem = transform.GetComponent<Dialogue_Manger>();
        emotionAndEventProcessor = transform.GetComponent<Reaction_And_Event_Processing>();

        var events = emotionAndEventProcessor.events.EvaliuateChanceEvents(); //Roll for any random event
        todaysChanceEvents = events.chanceEvents;
        dialogueSystem.SetSavedEvent("heavyDrinking", events.isHeavyDrinking);

        PlayerPrefs.SetInt("lateHomeArival", 0);
        PlayerPrefs.SetInt("heavyDrinking", 0);
        SetApproprateBackground("bedroom.day");
        emotionAndEventProcessor.reactions.RefreshModals();

        //Part of the NextSection function to set up for the first section
        Debug.Log("Up next: " + daysInfo.currentDaySection.section);
        dialogueSystem.SetupSection((int)daysInfo.currentDaySection.section, todaysChanceEvents[daysInfo.currentDaySection.section]);
        NextDialoguePressed();
    }

    //
    // Regular game flow
    //
    public void SetApproprateBackground(string bgDetails) //Get the proper background sprite
    {
        string[] splitDetails = bgDetails.Split('.');
        string bgKind = splitDetails[0];
        string bgTime = splitDetails[1];

        for (int i = 0; i < backgroundSet.Length; i++) { //Locate the correct sprite for the input section
            if (backgroundSet[i].isKind(bgKind) ) //If the kind in tag
            {
                background.style.backgroundImage = new StyleBackground(backgroundSet[i].LocateTime(bgTime)); //Set background
                Debug.Log("Set background to " + bgDetails);
                return; //and exit
            }
        }
        Debug.Assert(false, "Background texture asignment failed");
    }
    
    private void NextSection() //Does all the setup for going to the next section
    {
        daysInfo.currentDaySection.NextSection();
        if (daysInfo.currentDaySection.section == daySection.firstWork && dialogueSystem.GetSavedEvent("skipFirstWork")) { //Skip section if skip section flag is set
            daysInfo.currentDaySection.NextSection();
        }
        else if (daysInfo.currentDaySection.section == daySection.homeTravel && dialogueSystem.GetSavedEvent("skipHomeTravel")) {
            daysInfo.currentDaySection.NextSection();
        }

        Debug.Log("Up next: " + daysInfo.currentDaySection.section);
        dialogueSystem.SetupSection((int)daysInfo.currentDaySection.section, todaysChanceEvents[daysInfo.currentDaySection.section]);
        NextDialoguePressed();
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
    private class BackgroundSpriteSet
    {
        [SerializeField] private BackgroundSprite[] sprites;
        [SerializeField] private backgroundKind kind;

        public backgroundKind getKind() { return kind; }
        public int SpritesLength() { return sprites.Length; }

        // Checks if background kind is same as one searched for
        public bool isKind(string inp) {
            if (inp == kind.ToString())
            {
                return true;
            }
            else { return false; }
        }
        // Find specified time from sprite list
        public Sprite LocateTime(string inp) {
            for (short i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].isTime(inp)) {
                    return sprites[i].getSprite();
                }
            }
            Debug.Assert(false, "No background time requested among this kind. (" + kind.ToString() + "-" + inp + ")");
            return sprites[0].getSprite();
        }
    }

    [System.Serializable]
    private class BackgroundSprite
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private backgroundTime time;

        public Sprite getSprite() { return sprite; }

        // Checks if background is same time as one searched for
        public bool isTime(string inp) {
            if (inp == time.ToString())
            {
                return true;
            }
            else { return false; }
        }
    }
}