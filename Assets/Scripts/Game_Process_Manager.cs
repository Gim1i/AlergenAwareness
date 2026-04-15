using System.Collections.Generic;
using UnityEngine;

public class Game_Process_Manager : MonoBehaviour
{
    private enum backgroundTime { day, afternoon, evening };
    private enum backgroundKind { bedroom, driving, officeJob, officeBreakRoom, coffeeShop, jenns, saladDeli, livingRoom, gym, resturant, pub};
    private enum option { unchosen, one, two, three, four, alergy }; //If used as bool "one" is true and "two" is false

    [SerializeField] private Modal_Managment modalSystem;
    [SerializeField] private BackgroundSpriteInfo[] backgroundSheet;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Dialogue_Manger dialogueSystem;

    [SerializeField] private ScriptableObject[] displayTexts = new ScriptableObject[5];

    private modalInformation testHappymodal = new modalInformation(modalVariant.happy, true, playerStatLevel.medium);
    private modalInformation testSadmodal = new modalInformation(modalVariant.sad, true, playerStatLevel.medium);

    private Dictionary<string, bool> savedEvents = new Dictionary<string, bool>() { //Any choice or event that might impact later options
        { "prepedLunch", false },
        { "afternoonDriveDelay", false }
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

    public void Option1Pressed()
    {

    }
    public void Option2Pressed()
    {

    }
    public void Option3Pressed()
    {

    }
    public void Option4Pressed()
    {

    }

    public void NextDialoguePressed()
    {
        if (!dialogueSystem.NextDialogue()) //Sort next dialogue and check wether its a choice
        { //If dialogue
            (string choice, string[] tags) test = dialogueSystem.GetDialogue();
        }
        else
        { //If choice
            (string choice, string[] tags)[] tests = dialogueSystem.GetChoices();
        }
    }

    //  ||
    // Functions that handle regular game flow
    //
    private void Start()
    {
        for (int i = 0; i > 9; i++) { //Gets all events for the day
            todaysChanceEvents[(daySection)i] = EvaliuateChanceEvents((daySection)i);
        }
        //NextSectionProcessing(daysInfo.currentDaySection.section);
    }

    //private void NextSectionProcessing(daySection section) //Uses other functions to run though all processing needed at the start of a section change
    //{
    //    int eventChosen = EvaliuateChanceEvents(section); //Select this sections event (if any)
    //    if (eventChosen == 3) { //Check if any on driving section if event 3 is rolled
    //        switch (section)
    //        {
    //            case daySection.workEndTravel: //Afternoon Driving Delays event chosen
    //                todaysChoicesAndEvents[4] = option.one;
    //                return;
    //            case daySection.homeTravel: //Home Driving Delays event chosen
    //                playerStats.EveningDriveDelayed();
    //                return;
    //            case daySection.workStartTravel: //Morning Driving Delays event chosen
    //                todaysChoicesAndEvents[1] = option.one;
    //                return;
    //        }
    //    }
    //    if (section == daySection.firstWork && eventChosen == 3) { //If colleague down event
    //        todaysChoicesAndEvents[2] = option.one; //Record in "todaysChoicesAndEvents"
    //    }

    //    SetApproprateBackground(daysInfo.currentDaySection.section, todaysChoicesAndEvents);
    //}

    private int EvaliuateChanceEvents(daySection section) //Evaliuate whether a random event happens in a given day section
    {
        int eventNumber = Random.Range(1, 1000); //generate a random number to be the event chosen
        for (int i = 0; i < randomnessArray.daySections.Length;) { //Find the right day section
            if (randomnessArray.daySections[i].section == section) {
                if (randomnessArray.daySections[i].eventChances.Length == 0) { return -1; } //If day section has no events skip event selection
                for (int f = 0; f < randomnessArray.daySections[i].eventChances.Length;) { //Calculate which event was selected
                    eventNumber -= randomnessArray.daySections[i].eventChances[f];
                    if (eventNumber <= 0) {
                        return f; //Return the event ID
                    }
                }
                return -1; //No event was rolled
            }
        }
        Debug.Assert(false, "Event check couldn't find section (somehow)");
        return -1; 
    }

    private void SetApproprateBackground(daySection section, option[] choices) //Get the proper background sprite
    {
        for (int i = 0; i < backgroundSheet.Length; i++) { //Locate the correct sprite for the input section
            if (backgroundSheet[i].getSection() == section) {
                switch (section) //Determine if the section is a multi-choice section
                {
                    case daySection.lunch:
                        backgroundKind lChoice;
                        switch (choices[3]) //Get the correct background kind for the choice chosen
                        {
                            case option.one:
                                lChoice = backgroundKind.officeBreakRoom;
                                break;
                            case option.two:
                                lChoice = backgroundKind.coffeeShop;
                                break;
                            case option.three:
                                lChoice = backgroundKind.jenns;
                                break;
                            case option.four:
                                lChoice = backgroundKind.saladDeli;
                                break;
                            default:
                                Debug.Assert(false, "Lunch background kind dermine failed");
                                lChoice = backgroundKind.jenns;
                                break;
                        }
                        if (backgroundSheet[i].getKind() == lChoice) { //Determine if the background is the same as the lunch choice chosen
                            background.sprite = backgroundSheet[i].getSprite();
                            return;
                        }
                        break; //If not the same continue searching
                    case daySection.afternoon:
                        backgroundKind aChoice;
                        switch (choices[5]) //Get the correct background kind for the choice chosen
                        {
                            case option.one:
                                aChoice = backgroundKind.livingRoom;
                                break;
                            case option.two:
                                aChoice = backgroundKind.gym;
                                break;
                            case option.three:
                                aChoice = backgroundKind.resturant;
                                break;
                            case option.four:
                                aChoice = backgroundKind.pub;
                                break;
                            default:
                                Debug.Assert(false, "Afternoon activity background kind dermine failed");
                                aChoice = backgroundKind.livingRoom;
                                break;
                        }
                        if (backgroundSheet[i].getKind() == aChoice) { //Determine if the background is the same as the lunch choice chosen
                            background.sprite = backgroundSheet[i].getSprite();
                            return;
                        }
                        break; //If not the same continue searching
                    default: //If not a multi-choice section just set background
                        background.sprite = backgroundSheet[i].getSprite();
                        return;
                }
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
        public backgroundKind getKind() { return kind; }
        public backgroundTime getTime() { return time; }
        public daySection getSection() { return section; }
    }
}


