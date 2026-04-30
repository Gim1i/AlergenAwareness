using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Modal_Managment : MonoBehaviour
{
    private enum playerStatLevel { none, low, medium, high }
    private enum modalVariant { happy, sad, angry, pain, tired, stress, bored, feelingSick, tinglingThroat, itchy, runnyNose, tightChest, hardToBreath, sick }
    private enum availableLevels { good_lowToHigh, bad_lowToHigh, low, mid, high }

    [SerializeField] private StateSprites[] statesArray;
    [SerializeField] private spriteBackgroundRanges[] backgroundRanges;
    [SerializeField] private VisualTreeAsset modalTemplate;
    [SerializeField] private float transitionSpeed;
    private VisualElement[] modalSlots = new VisualElement[8];
    private ModalInfo[] activeModals = new ModalInfo[8];
    private int filledModalSlots = 0;

    private void Awake() //Get UI elements neccessary
    {
        VisualElement gameDisplay = transform.GetChild(0).GetComponent<UIDocument>().rootVisualElement;
        Debug.Assert(gameDisplay != null, "Couldn't find the UIDoc's root");

        for (int i = 0; i < modalSlots.Length; i++) //Get all modal slots
        {
            modalSlots[i] = gameDisplay.Q<VisualElement>("Slot_" + (i+1));
            Debug.Assert(modalSlots[i] != null, "Couldn't find slot " + (i + 1));
        }
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
                        activeModals[j].AlterModalLevel(currentLevel, ref backgroundRanges);
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
        if (filledModalSlots >= 5) { return; }
        activeModals[filledModalSlots] = new ModalInfo(modalTemplate, ref statesArray, variant, isEmotion, level, ref backgroundRanges); //Create the modal
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
        private int levelRange; //Easy reference for changing the background sprite

        public ModalInfo(VisualTreeAsset template, ref StateSprites[] sprites, modalVariant variant, bool isEmotion, short level, ref spriteBackgroundRanges[] bgRanges) //Initialise modal
        {
            Debug.Log("Modal setup");
            visElmnt = template.Instantiate();
            visElmnt.AddToClassList("Modal"); //Add its proper class
            for (short i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].IsThisVariant(variant)) { //Locate the variant in the spites store
                    visElmnt.Q<VisualElement>("Modal_State").style.backgroundImage = new StyleBackground(sprites[i].GetStateSprite()); //Set the new sprite's state
                    break;
                }
            }

            for (short i = 0; i < bgRanges.Length; i++) //Look though all background ranges
            {
                if (bgRanges[i].IsVariantUsingRange(variant)) //If this variant is in this range
                {
                    levelRange = i;
                    break;
                }
            }
            visElmnt.Q<VisualElement>("Modal_Background").style.backgroundImage = new StyleBackground(bgRanges[levelRange].GetBackground(ShortToPlyrStat(level))); //Set the new sprite's background

            this.variant = variant;
            this.isEmotion = isEmotion;
            this.level = level;
            Debug.Log("Modal setup end");
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
        public void AlterModalLevel(short newLevel, ref spriteBackgroundRanges[] bgRanges) //Update the modal's background sprite
        {
            if (isEmotion) //Be sure this is an emotion (never run this with afflicts)
            {
                if (ShortToPlyrStat(level) != ShortToPlyrStat(newLevel)) //If there is enough of a change to go up or down a level
                {
                    visElmnt.Q<VisualElement>("Modal_Background").style.backgroundImage = new StyleBackground(bgRanges[levelRange].GetBackground(ShortToPlyrStat(newLevel))); //Set the new background
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
        [SerializeField] private Sprite sprite;
        [SerializeField] private bool isEmotion;
        [SerializeField] private availableLevels levelRange;

        // Get the sprite
        public Sprite GetStateSprite() { return sprite; }

        public availableLevels GetLevelRange() { return levelRange; }

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

    //
    // Stores modal backgrounds along side the level they represent. Designed to allow for multiple different sets of levels
    //
    [System.Serializable]
    class spriteBackgroundRanges
    {
        [SerializeField] private availableLevels levelRange;
        [SerializeField] private modalVariant[] modalsUsingThisRange;
        [SerializeField] private spriteBackgrounds[] backgrounds;

        // Look for the modal in this range
        public bool IsVariantUsingRange(modalVariant variant)
        {
            for (int i = 0; i < modalsUsingThisRange.Length; ++i)
            {
                if (modalsUsingThisRange[i] == variant) { return true; }
            }
            return false;
        }

        // Check if this is the range input
        public bool IsThisRangeLookedFor(availableLevels range)
        {
            if (levelRange == range) { return true; }
            return false;
        }

        // Get the background asociated with the level
        public Sprite GetBackground(playerStatLevel level)
        {
            for (int i = 0; i < backgrounds.Length; ++i)
            {
                if (backgrounds[i].GetLevel() == level) { return backgrounds[i].GetBackground(); }
            }
            return backgrounds[0].GetBackground();
        }
    }

    [System.Serializable]
    class spriteBackgrounds
    {
        [SerializeField] private Sprite background;
        [SerializeField] private playerStatLevel level;

        public Sprite GetBackground() { return background; }
        public playerStatLevel GetLevel() { return level; }
    }

    static private playerStatLevel ShortToPlyrStat(short shrt){ //Turns a level into a player stat for easy use. Used in many places in this script
        return (playerStatLevel)(shrt / 25);
    }
}