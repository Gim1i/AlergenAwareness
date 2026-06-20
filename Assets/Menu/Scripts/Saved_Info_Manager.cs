using System;
using UnityEngine;

public class Saved_Info_Manager : MonoBehaviour
{
    private enum emotionState { happy, sad, angry, pain, tired, stress, bored, feelingSick } // Possible emotion modals
    private enum afflictState { tinglingThroat, itchy, runnyNose, tightChest, hardToBreath, sick } // Possible afflict modals

    [SerializeField] private static Saved_Info_Manager instance;

    (string prefKey, int defVal)[] gamePrefKeyList = new[] { // Bools are stored here too (0 is false, 1 is true)
        ("lateHomeArival", 0), // Bool
        ("heavyDrinking", 0),  // Bool
        ("existingGame", 0),   // Bool
        ("health", 100)
    };
    (string prefKey, float defVal)[] settingsPrefKeyList = new[] {
        ("masterVolume", 1f),
        ("musicVolume", 1f),
        ("uiVolume", 1f),
        ("textSpeed", 1f)
    };
    (emotionState prefKey, int defVal)[] emotionPrefKeyList = new[] {
        (emotionState.happy, 70),
        (emotionState.sad, 0),
        (emotionState.angry, 0),
        (emotionState.pain, 0),
        (emotionState.tired, 0),
        (emotionState.stress, 0),
        (emotionState.bored, 0),
        (emotionState.feelingSick, 0)
    };
    (afflictState prefKey, bool defVal)[] afflictsPrefKeyList = new[] {
        (afflictState.tinglingThroat, false),
        (afflictState.runnyNose, false),
        (afflictState.tightChest, false),
        (afflictState.hardToBreath, false),
        (afflictState.itchy, false),
        (afflictState.sick, false)
    };

    void Awake() // When game loads check if all player settings are set
    {
        if (instance != null) { Destroy(gameObject); return; } //Deletes any duplicates that are made
        DontDestroyOnLoad(gameObject); //Always exists so only runs once
        instance = this;

        // Game player pref keys
        for (int i = 0; i < gamePrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(gamePrefKeyList[i].prefKey)) { //Check if these are set
                PlayerPrefs.SetInt(gamePrefKeyList[i].prefKey, gamePrefKeyList[i].defVal); //If they arn't set them
            }
        }

        // Settings player pref keys
        for (int i = 0; i < settingsPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(settingsPrefKeyList[i].prefKey)) { //Check if these are set
                PlayerPrefs.SetFloat(settingsPrefKeyList[i].prefKey, settingsPrefKeyList[i].defVal); //If they arn't set them
            }
        }

        // Emotion modal player pref keys
        for (int i = 0; i < emotionPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(emotionPrefKeyList[i].prefKey.ToString())) { //Check if these are set
                PlayerPrefs.SetInt(emotionPrefKeyList[i].prefKey.ToString(), (int)emotionPrefKeyList[i].defVal); //If they arn't set them
            }
        }

        // Afflict modal player pref keys
        for (int i = 0; i < afflictsPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(afflictsPrefKeyList[i].prefKey.ToString())) { //Check if these are set
                PlayerPrefs.SetInt(afflictsPrefKeyList[i].prefKey.ToString(), Convert.ToInt32(afflictsPrefKeyList[i].defVal)); //If they arn't set them
            }
        }
    }

    //
    // Reseting player prefs
    //
    public void ResetGamePrefs() // Reset player prefs related to gameplay
    {
        // Game player pref keys
        for (int i = 0; i < gamePrefKeyList.Length; i++)
        {
            PlayerPrefs.SetInt(gamePrefKeyList[i].prefKey, gamePrefKeyList[i].defVal);
        }

        // Emotion modal player pref keys
        for (int i = 0; i < emotionPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetInt(emotionPrefKeyList[i].prefKey.ToString(), (int)emotionPrefKeyList[i].defVal);
        }

        // Afflict modal player pref keys
        for (int i = 0; i < afflictsPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetInt(afflictsPrefKeyList[i].prefKey.ToString(), Convert.ToInt32(afflictsPrefKeyList[i].defVal));
        }
    }

    public void ResetSettingsPrefs() // Reset player prefs related to settings
    {
        //Settings player pref keys
        for (int i = 0; i < settingsPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetFloat(settingsPrefKeyList[i].prefKey, settingsPrefKeyList[i].defVal);
        }
    }
}
