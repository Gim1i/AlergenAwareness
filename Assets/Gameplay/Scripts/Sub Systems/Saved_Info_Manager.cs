using System;
using UnityEngine;

public class Saved_Info_Manager : MonoBehaviour
{
    private enum emotionState { happy, sad, angry, pain, tired, stress, bored, itchiness, feelingSick } //Possible emotion modals
    private enum afflictState { tinglingThroat, runnyNose, tightChest, hardToBreath, sick } //Possible afflict modals

    [SerializeField] private static Saved_Info_Manager instance;

    (string prefKey, int defVal)[] intPrefKeyList = new[] { //Bools are stored here too (0 is false, 1 is true)
        ("lateHomeArival", 0), //Bool
        ("heavyDrinking", 0),  //Bool
        ("existingGame", 0),   //Bool
        ("health", 0)
    };
    (string prefKey, float defVal)[] floatPrefKeyList = new[] {
        ("masterVolume", 1f),
        ("musicVolume", 1f),
        ("uiVolume", 1f),
        ("textSpeed", 0.07f)
    };
    (emotionState prefKey, int defVal)[] emotionPrefKeyList = new[] {
        (emotionState.happy, 70),
        (emotionState.sad, 0),
        (emotionState.angry, 0),
        (emotionState.pain, 0),
        (emotionState.tired, 0),
        (emotionState.stress, 0),
        (emotionState.bored, 0),
        (emotionState.itchiness, 0),
        (emotionState.feelingSick, 0)
    };
    (afflictState prefKey, bool defVal)[] afflictsPrefKeyList = new[] {
        (afflictState.tinglingThroat, false),
        (afflictState.runnyNose, false),
        (afflictState.tightChest, false),
        (afflictState.hardToBreath, false),
        (afflictState.sick, false)
    };

    void Awake() //When game loads check if all player settings are set
    {
        if (instance != null) { Destroy(gameObject); return; } //Deletes any duplicates that are made
        DontDestroyOnLoad(gameObject); //Always exists so only runs once
        instance = this;

        //Integer player pref keys
        for (int i = 0; i < intPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(intPrefKeyList[i].prefKey)) { //Check if these are set
                PlayerPrefs.SetInt(intPrefKeyList[i].prefKey, intPrefKeyList[i].defVal); //If they arn't set them
            }
        }

        //Float player pref keys
        for (int i = 0; i < floatPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(floatPrefKeyList[i].prefKey)) { //Check if these are set
                PlayerPrefs.SetFloat(floatPrefKeyList[i].prefKey, floatPrefKeyList[i].defVal); //If they arn't set them
            }
        }

        //Emotion modal player pref keys
        for (int i = 0; i < emotionPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(emotionPrefKeyList[i].prefKey.ToString())) { //Check if these are set
                PlayerPrefs.SetInt(emotionPrefKeyList[i].prefKey.ToString(), (int)emotionPrefKeyList[i].defVal); //If they arn't set them
            }
        }

        //Afflict modal player pref keys
        for (int i = 0; i < afflictsPrefKeyList.Length; i++) {
            if (!PlayerPrefs.HasKey(afflictsPrefKeyList[i].prefKey.ToString())) { //Check if these are set
                PlayerPrefs.SetInt(afflictsPrefKeyList[i].prefKey.ToString(), Convert.ToInt32(afflictsPrefKeyList[i].defVal)); //If they arn't set them
            }
        }
    }

    //
    // Resets all player prefs
    //
    public void ResetPrefs()
    {
        //Integer player pref keys
        for (int i = 0; i < intPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetInt(intPrefKeyList[i].prefKey, intPrefKeyList[i].defVal);
        }

        //Float player pref keys
        for (int i = 0; i < floatPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetFloat(floatPrefKeyList[i].prefKey, floatPrefKeyList[i].defVal);
        }

        //Emotion modal player pref keys
        for (int i = 0; i < emotionPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetInt(emotionPrefKeyList[i].prefKey.ToString(), (int)emotionPrefKeyList[i].defVal);
        }

        //Afflict modal player pref keys
        for (int i = 0; i < afflictsPrefKeyList.Length; i++)
        {
            PlayerPrefs.SetInt(afflictsPrefKeyList[i].prefKey.ToString(), Convert.ToInt32(afflictsPrefKeyList[i].defVal));
        }
    }
}
