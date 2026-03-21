using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game_Process_Manager : MonoBehaviour
{
    private enum backgroundTime { day, afternoon, evening };
    private enum backgroundSection { bedroom, driving, officeJob, officeBreakRoom, coffeeShop, jenns, saladDeli, livingRoom, gym, resturant, pub};
    [SerializeField] private Modal_Managment modalSystem;
    [SerializeField] private BackgroundSpriteInfo[] backgroundSheet;
    private modalInformation testHappymodal = new modalInformation(modalVariant.happy, true, playerStatLevel.medium);
    private modalInformation testSadmodal = new modalInformation(modalVariant.sad, true, playerStatLevel.medium);

    public void HPressed()
    {
        if (modalSystem.DoesModalExist(testHappymodal)) { //Check wether modal already exists
            modalSystem.RemoveModal(testHappymodal); //Remove it if it exists
        } else {
            modalSystem.CreateNewModal(testHappymodal); //Add it if its missing
        }
    }
    public void SPressed()
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

    //
    // Functions that handle regular game flow
    //
    private void Start()
    {
        int Event = EvaliuateChanceEvents(daysInfo.currentDaySection.section);
        if (Event >= 0) {
            string temp = randomnessArray.daySections[(int)daysInfo.currentDaySection.section].events[Event].rEvent;
        }
    }

    private int EvaliuateChanceEvents(daySection section) //Evaliuate whether a random event happens in a given day section
    {
        int eventNumber = Random.Range(1, 1000); //generate a random number to be the event chosen
        for (int i = 0; i < randomnessArray.daySections.Length;) { //Find the right day section
            if (randomnessArray.daySections[i].section == section) {
                if (randomnessArray.daySections[i].events.Length == 0) { return -1; } //Day section has no events
                for (int f = 0; f < randomnessArray.daySections[i].events.Length;) { //Calculate which event was selected
                    eventNumber -= randomnessArray.daySections[i].events[f].chance; {
                        return f; //Return the event ID
                    }
                }
                return -1; //No event was rolled
            }
        }
        Debug.Assert(false, "Event check couldn't find section (somehow)");
        return -1; 
    }

    //
    // All information about the current day. Resets on the next day due to the scene being re-made
    //
    private static class daysInfo
    {
        public static class currentDaySection
        {
            public static daySection section { get; private set; } = daySection.dayStart;
            static void NextSection()
            { //Move to next day section
                if (section != daySection.dayEnd) {
                    section = (daySection)((int)section + 1);
                } else {
                    section = daySection.dayStart;
                }
            }
        }

        public static class conditionalEvents //Which conditional events are active
        {
            private static bool isLunchPreped = false;
            private static bool isColleagueDown = false;
            private static bool isPostWorkDelay = false;

            public static bool Check(condition condition)
            { //Check the conditional bool's value
                switch (condition) {
                    case condition.prepedLunch:
                        return isLunchPreped;
                    case condition.downColleague:
                        return isColleagueDown;
                    case condition.postWorkDelay:
                        return isPostWorkDelay;
                    default:
                        Debug.Assert(false, "A condition is missing its bool variable");
                        return false;
                }
            }
            public static void Toggle(condition condition)
            { //Flip the value of a conditional bool
                switch (condition) {
                    case condition.prepedLunch:
                        isLunchPreped = !isLunchPreped;
                        return;
                    case condition.downColleague:
                        isColleagueDown = !isColleagueDown;
                        return;
                    case condition.postWorkDelay:
                        isPostWorkDelay = !isPostWorkDelay;
                        return;
                    default:
                        Debug.Assert(false, "A condition is missing its toggle");
                        return;
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
        [SerializeField] private backgroundSection section;
        [SerializeField] private backgroundTime time;

        public Sprite getSprite() { return sprite; }
        public backgroundSection getSection() { return section; }
        public backgroundTime getTime() { return time; }
    }
}


