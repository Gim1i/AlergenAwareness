using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Modal_Managment : MonoBehaviour
{
    private enum playerStatLevel { none, low, medium, high }
    private enum modalVariant { happy, sad, angry, pain, tired, stress, bored, feelingSick, tinglingThroat, itchy, runnyNose, tightChest, hardToBreath, sick }

    [SerializeField] private StateSprites[] statesArray;
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
    // Refreshes current modals to reflect any stats changes
    //
    public void ApplyEmotionChanges() 
    {
        //
        // Check if any emotion has changed enough to need a new modal, their modal removed or their modal changed
        //
        for (int i = 0; i < Enum.GetNames(typeof(Reaction_And_Event_Processing.Reactions.emotionState)).Length; i++) //For every emotion
        {
            string emotStr = ((Reaction_And_Event_Processing.Reactions.emotionState)i).ToString();
            modalVariant modVarEquv = modalVariant.happy; //The modalVariant equivlant of its emotionState. Happy is the default state
            for (int m = 0; m < Enum.GetNames(typeof(modalVariant)).Length; m++)
            {
                if (emotStr == ((modalVariant)m).ToString()) {
                    modVarEquv = (modalVariant)m;
                    break;
                }
            }
            Debug.Assert(!(modVarEquv == modalVariant.happy && i != 0), "Modal variant not located (and not happy). Default value used");

            short currentLevel = (short)PlayerPrefs.GetInt(emotStr); //Get its current level
            bool hasActiveModal = false;
            for (int j = 0; j < filledModalSlots; j++)
            {
                if (activeModals[j].GetVariant() == modVarEquv) //If the emotion already has an active modal
                {
                    if (ShortToPlyrStat(currentLevel) == 0) { //Remove modal if level is too low
                        RemoveModal(modVarEquv);
                    }
                    else { //Else update the modal to match its new level
                        activeModals[j].AlterModalLevel(currentLevel, ref statesArray);
                    }
                    hasActiveModal = true;
                    break;
                }
            }

            if (!hasActiveModal) //If there is or was no active modal for the emotion
            {
                if (ShortToPlyrStat(currentLevel) > 0) { //Check wether it should get one or not
                    CreateNewModal(modVarEquv, true, currentLevel);
                }
            }
            
        }

        for (int i = 0; i < Enum.GetNames(typeof(Reaction_And_Event_Processing.Reactions.afflictState)).Length; i++) //For every afflict
        {
            string aflctStr = ((Reaction_And_Event_Processing.Reactions.afflictState)i).ToString();
            modalVariant modVarEquv = modalVariant.tinglingThroat; //The modalVariant equivlant of its afflictState. TinglingThroat is the default state
            for (int m = 0; m < Enum.GetNames(typeof(modalVariant)).Length; m++)
            {
                if (aflctStr == ((modalVariant)m).ToString()) {
                    modVarEquv = (modalVariant)m;
                    break;
                }
            }
            Debug.Assert(!(modVarEquv == modalVariant.tinglingThroat && i != 0), "Modal variant not located (and not TinglingThroat). Default value used");

            bool currentState = Convert.ToBoolean(PlayerPrefs.GetInt(aflctStr)); //Get its current state
            short activeModalLocation = -1;
            for (int j = 0; j < filledModalSlots; j++)
            {
                if (activeModals[j].GetVariant() == modVarEquv) { //If the afflict already has an active modal
                    activeModalLocation = (short)j; //Save its location
                    break;
                }
            }

            if (activeModalLocation == -1) //If theres no active modal for the afflict
            {
                if (currentState) { //Create one if it should have one
                    CreateNewModal(modVarEquv, false, 0);
                }
            } else //If there is an active modal for the afflict
            {
                if (!currentState) { //Remove modal if it shouldn't exist
                    RemoveModal(modVarEquv);
                }
            }
            
        }
    }

    //
    // Modal control functions
    //
    private void CreateNewModal(modalVariant variant, bool isEmotion, short level)
    {
        activeModals[filledModalSlots] = new ModalInfo(modalTemplate, ref statesArray, variant, isEmotion, level); //Create the modal
        modalSlots[filledModalSlots].Add(activeModals[filledModalSlots].GetModal()); //Put the modal into its proper slot (Visual Element)
        Debug.Log("Created " + variant + "-" + level + " modal");
        filledModalSlots++; //Move highest empty slot down one
    }

    private void RemoveModal(modalVariant variant)
    {
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].IsThisModalLookedFor(variant)) {
                modalSlots[i].Clear();
                activeModals[i] = null;
                Debug.Log("Removed " + variant + " modal");
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

    //
    // Data store for modals (as they no longer have gameobjet to store it in)
    //
    class ModalInfo
    {
        private TemplateContainer visElmnt;
        private bool isEmotion;
        private short level;
        private modalVariant variant;
        private short spriteArrayIndex; //The location of the variant in the spriteArray (don't gotta search it every time)

        public ModalInfo(VisualTreeAsset template, ref StateSprites[] sprites, modalVariant variant, bool isEmotion, short level) //Initialise modal
        {
            visElmnt = template.Instantiate();
            visElmnt.AddToClassList("Modal"); //Add its proper class
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].IsThisVariant(variant)) { //Locate the variant in the spites store
                    spriteArrayIndex = (short)i; //Save the loaction for easy use later
                    visElmnt.Q<VisualElement>("Modal").style.backgroundImage = new StyleBackground(sprites[spriteArrayIndex].GetSprite(level)); //Set the new sprite
                    break;
                }
            }

            this.variant = variant;
            this.isEmotion = isEmotion;
            this.level = level;
        }

        public bool IsThisModalLookedFor(modalVariant vari) //Check if the modal is the one being searched for
        {
            if (vari == variant) {
                return true;
            }
            return false;
        }

        //
        // Alter the current modal in some way
        //
        public void AlterModalLevel(short newLevel, ref StateSprites[] sprites) //Update the modal's display sprite
        {
            if (isEmotion) //Be sure this is an emotion (never run this with afflicts
            {
                if (ShortToPlyrStat(level) != ShortToPlyrStat(newLevel)) //If there is enough of a change to go up or down a level
                {
                    visElmnt.Q<VisualElement>("Modal").style.backgroundImage = new StyleBackground(sprites[spriteArrayIndex].GetSprite(newLevel)); //Set the new sprite
                }
                level = newLevel;
            }
            else { Debug.Assert(false, "AlterModalLevel run with an afflict. No change in modal"); }
        }

        // Return certain info
        public playerStatLevel GetLevelAsStat() { return ShortToPlyrStat(level); }
        public modalVariant GetVariant() { return variant; }
        public TemplateContainer GetModal() { return visElmnt; }
    }

    //
    // Stores all modal sprites and their info
    //
    [System.Serializable]
    class StateSprites
    {
        [SerializeField] private modalVariant variant;
        [SerializeField] private bool isEmotion;
        [SerializeField] private spriteVariant[] levels;

        // Get the class name for the given level
        public Sprite GetSprite(short? level)
        {
            if (!isEmotion) { return levels[0].GetClassName(); } //Skip search if afflict (only one level)

            if (level != null) { //Skip search if level missing
                for (int i = 0; i < levels.Length; i++)
                {
                    if (levels[i].GetLevel() == ShortToPlyrStat(level.Value)) { //Locate the proper level
                        return levels[i].GetClassName();
                    }
                }
            }
            else { Debug.Assert(false, "Level incorrectly missing from GetClassName. Default value returned"); }
            return levels[0].GetClassName();
        }

        // Check for variant
        public bool IsThisVariant(modalVariant vari) {
            if (vari == variant) { return true; }
            return false;
        }
        public bool IsThisVariantStrIn(string vari) {
            if (vari == variant.ToString()) { return true; }
            return false;
        }
    }

    [System.Serializable]
    class spriteVariant
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private playerStatLevel level;

        public Sprite GetClassName() { return sprite; }
        public playerStatLevel GetLevel() { return level; }
    }

    static private playerStatLevel ShortToPlyrStat(short shrt){ //Turns a level into a player stat for easy use. Used in many places in this script
        return (playerStatLevel)(shrt / 25);
    }
}