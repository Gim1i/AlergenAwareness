using UnityEngine;

public class Saved_Info_Manager : MonoBehaviour
{
    [SerializeField] static Saved_Info_Manager instance;

    (string prefKey, int defVal)[] intPrefKeyList = new[] { //Bools are stored here too (0 is false, 1 is true)
        ("lateHomeArival", 0),
        ("heavyDrinking", 0),
        ("existingGame", 0)
    };
    (string prefKey, float defVal)[] floatPrefKeyList = new[] {
        ("masterVolume", 1f),
        ("musicVolume", 1f),
        ("uiVolume", 1f),
        ("textSpeed", 0.07f)
    };
    (emotionState prefKey, playerStatLevel defVal)[] emotionPrefKeyList = new[] {
        (emotionState.happy, playerStatLevel.none),
        (emotionState.sad, playerStatLevel.none),
        (emotionState.angry, playerStatLevel.none),
        (emotionState.pain, playerStatLevel.none),
        (emotionState.tired, playerStatLevel.none),
        (emotionState.stress, playerStatLevel.none),
        (emotionState.bored, playerStatLevel.none),
        (emotionState.itchiness, playerStatLevel.none),
        (emotionState.feelingSick, playerStatLevel.none)
    };
    (afflictState prefKey, playerStatLevel defVal)[] afflictsPrefKeyList = new[] {
        (afflictState.tinglingThroat, playerStatLevel.none),
        (afflictState.runnyNose, playerStatLevel.none),
        (afflictState.tightChest, playerStatLevel.none),
        (afflictState.shortBreath, playerStatLevel.none),
        (afflictState.sick, playerStatLevel.none)
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
                PlayerPrefs.SetInt(afflictsPrefKeyList[i].prefKey.ToString(), (int)afflictsPrefKeyList[i].defVal); //If they arn't set them
            }
        }
    }
}
