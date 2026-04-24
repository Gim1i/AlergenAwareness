using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Modal_Managment : MonoBehaviour
{
    [SerializeField] private ModalStates[] statesArray;
    [SerializeField] private VisualTreeAsset modalTemplate;
    [SerializeField] private float transitionSpeed;
    private VisualElement[] modalSlots = new VisualElement[5];
    private ModalInfo[] activeModals = new ModalInfo[5];
    private int filledModalSlots = 0;

    private void Awake() //Get UI elements neccessary
    {
        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        modalSlots[0] = gameDisplay.Q<VisualElement>("Slot_1");
        modalSlots[1] = gameDisplay.Q<VisualElement>("Slot_2");
        modalSlots[2] = gameDisplay.Q<VisualElement>("Slot_3");
        modalSlots[3] = gameDisplay.Q<VisualElement>("Slot_4");
        modalSlots[4] = gameDisplay.Q<VisualElement>("Slot_5");

        Debug.Assert(gameDisplay != null, "Couldn't find the UIDoc's root");
        Debug.Assert(modalSlots[0] != null, "Couldn't find slot 1");
        Debug.Assert(modalSlots[1] != null, "Couldn't find slot 2");
        Debug.Assert(modalSlots[2] != null, "Couldn't find slot 3");
        Debug.Assert(modalSlots[3] != null, "Couldn't find slot 4");
        Debug.Assert(modalSlots[4] != null, "Couldn't find slot 5");
    }

    //
    // Modal control functions
    //
    public void CreateNewModal(modalVariant variant, bool isEmotion, playerStatLevel level)
    {
        for (int i = 0; i < statesArray.Length; i++) //Look though the registered states
        {
            if (statesArray[i].IsThisStateLookedFor(variant, level)) //Find the state that includes the sprite (state might not have one)
            {
                activeModals[i] = new ModalInfo(modalTemplate, statesArray[i].GetClassName(), variant, isEmotion, level); //Create the modal
                modalSlots[filledModalSlots].Add(activeModals[i].GetModal()); //Put the modal into its proper slot (Visual Element)
                Debug.Log("Created " + variant + "-" + level + " modal");
                filledModalSlots++; //Move highest empty slot down one
                return;
            }
        }
    }

    public bool DoesModalExist(modalVariant variant, playerStatLevel level)
    {
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].IsThisModalLookedFor(variant, level)) {
                Debug.Log("Modal exists");
                return true; //return true if exists
            }
        }
        Debug.Log("Modal doesn't exist");
        return false; //return false if it can't be found
    }

    public void RemoveModal(modalVariant variant, playerStatLevel level)
    {
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].IsThisModalLookedFor(variant, level)) {
                modalSlots[i].Clear();
                activeModals[i] = null;
                Debug.Log("Removed " + variant + "-" + level + " modal");
                filledModalSlots--;

                if (i != filledModalSlots) { //Moving Modals up check
                    for (int h = i+1; h < filledModalSlots; h++) { //Move modals up
                        modalSlots[h - 1].Add(activeModals[h].GetModal());
                    }
                }
                return;
            }
        }
    }


    public void ApplyEmotionChanges() //Refreshes current modals to reflect any new stats
    {
        /*(emotionState.happy, 70),
        (emotionState.sad, 0),
        (emotionState.angry, 0),
        (emotionState.pain, 0),
        (emotionState.tired, 0),
        (emotionState.stress, 0),
        (emotionState.bored, 0),
        (emotionState.itchiness, 0),
        (emotionState.feelingSick, 0)*/

        //Check if any emotion has changed enough to need a new modal
        for (int i = 0; i < 9; i++) //For every emotion
        {
            short currentLevel = (short)PlayerPrefs.GetInt(((emotionState)i).ToString()); //Get its current level
            playerStatLevel crntStatLvl = (playerStatLevel)Mathf.Floor(currentLevel / 25); //Convert it to a playerStatLevel
            for (int j = 0; j < filledModalSlots; j++)
            {
                if (activeModals[j].GetVariant().ToString() == ((emotionState)i).ToString()) //If the emotion already has an active modal
                {
                    if (activeModals[j].GetLevel() == crntStatLvl) { break; } //Go to next emotion if no change is needed
                    activeModals[j].AlterModalLevel(GetStatesClassName(activeModals[j].GetVariant(), activeModals[j].GetLevel()), crntStatLvl);
                }
            }
            /*if (currentLevel > activeModals[j].level * 25) {
            
            }*/
        }

        /*(afflictState.tinglingThroat, false),
        (afflictState.runnyNose, false),
        (afflictState.tightChest, false),
        (afflictState.hardToBreath, false),
        (afflictState.sick, false)*/
    }

    /*public static void AlterEmotionLevel(emotionState emotion, int scale)
    { //Alter an emotion up or down by scale
        for (int i = 0; i < emotions.Length; i++)
        { //Find the emotion
            if (emotions[i].emotion == emotion)
            {
                int newLevel = ((int)emotions[i].level + scale); //Make enum int and add scale
                if (newLevel > 3) { newLevel = 3; } //Constrict number to an acceptable range
                else if (newLevel < 0) { newLevel = 0; }
                emotions[i].level = (playerStatLevel)newLevel; //Save as new level
                return;
            }
        }
    }*/

    //
    // Other functions
    //
    private string GetStatesClassName(modalVariant variant, playerStatLevel level)
    {
        for (int i = 0; i < statesArray.Length; i++) //Look though the registered states
        {
            if (statesArray[i].IsThisStateLookedFor(variant, level)) { //Find the state that includes the sprite (state might not have one)
                return statesArray[i].GetClassName();
            }
        }
        return "Happy-Mid";
    }

    //
    // Data store for modals (as they no longer have gameobjet to store it in)
    //
    class ModalInfo
    {
        private TemplateContainer visElmnt;
        private bool isEmotion;
        private playerStatLevel level;
        private modalVariant variant;

        public ModalInfo(VisualTreeAsset template, string className, modalVariant variant, bool isEmotion, playerStatLevel level) //Initialise modal
        {
            visElmnt = template.Instantiate();
            visElmnt.AddToClassList("Modal"); //Add its proper class
            visElmnt.Q<VisualElement>("Modal_Status").AddToClassList(className); //Add the proper status class (adds sprite to display)

            this.variant = variant;
            this.isEmotion = isEmotion;
            this.level = level;
        }

        public bool IsThisModalLookedFor(modalVariant vari, playerStatLevel lvl) //Check if the modal is the one being searched for
        {
            if (vari == variant && lvl == level) {
                return true;
            }
            return false;
        }

        public void AlterModalLevel(string className, playerStatLevel newLevel) //Update the modal's display sprite
        {
            visElmnt.Q<VisualElement>("Modal_Status").ClearClassList(); //Remove the existing classes
            visElmnt.Q<VisualElement>("Modal_Status").AddToClassList(className); //Add the new status class

            this.level = newLevel;
        }

        public playerStatLevel GetLevel() { return level; }
        public modalVariant GetVariant() { return variant; }
        public TemplateContainer GetModal() { return visElmnt; }
    }

    //
    // Stores all modal sprites and their info
    //
    [System.Serializable]
    class ModalStates
    {
        [SerializeField] private string className;
        [SerializeField] private modalVariant variant;
        [SerializeField] private bool isEmotion;
        [SerializeField] private playerStatLevel level;

        public string GetClassName() {
            return className;
        }

        public bool IsThisStateLookedFor(modalVariant vari, playerStatLevel lvl) //Check if the state is the one being searched for
        {
            if (vari == variant && lvl == level) {
                return true;
            }
            return false;
        }
    }
}