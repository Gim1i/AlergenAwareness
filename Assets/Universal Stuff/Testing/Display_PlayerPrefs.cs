using System;
using UnityEngine;

public class Display_PlayerPrefs : MonoBehaviour
{
    [NonSerialized] public bool existingGame;

    [NonSerialized] public bool lateHomeArival;
    [NonSerialized] public bool heavyDrinking;

    // Currently not implemented
    [NonSerialized] public int health;
    [NonSerialized] public bool itchy;

    // Emotions
    [NonSerialized] public int happy;
    [NonSerialized] public int sad;
    [NonSerialized] public int angry;
    [NonSerialized] public int pain;
    [NonSerialized] public int tired;
    [NonSerialized] public int stress;
    [NonSerialized] public int bored;
    [NonSerialized] public int feelingSick;

    // Afflicts
    [NonSerialized] public bool tinglingThroat;
    [NonSerialized] public bool runnyNose;
    [NonSerialized] public bool tightChest;
    [NonSerialized] public bool hardToBreath;
    [NonSerialized] public bool sick;

    void Start() { RefreshPlayerPrefs(); }
    void Update() { RefreshPlayerPrefs(); }

    // Refresh the player pref display
    public void RefreshPlayerPrefs()
    {
        // General
        existingGame = Convert.ToBoolean(PlayerPrefs.GetInt("existingGame"));
        lateHomeArival = Convert.ToBoolean(PlayerPrefs.GetInt("lateHomeArival"));
        heavyDrinking = Convert.ToBoolean(PlayerPrefs.GetInt("heavyDrinking"));
        health = PlayerPrefs.GetInt("health");

        // Emotions
        happy = PlayerPrefs.GetInt("happy");
        sad = PlayerPrefs.GetInt("sad");
        angry = PlayerPrefs.GetInt("angry");
        pain = PlayerPrefs.GetInt("pain");
        tired = PlayerPrefs.GetInt("tired");
        stress = PlayerPrefs.GetInt("stress");
        bored = PlayerPrefs.GetInt("bored");
        feelingSick = PlayerPrefs.GetInt("feelingSick");

        // Afflicts
        tinglingThroat = Convert.ToBoolean(PlayerPrefs.GetInt("tinglingThroat"));
        runnyNose = Convert.ToBoolean(PlayerPrefs.GetInt("runnyNose"));
        tightChest = Convert.ToBoolean(PlayerPrefs.GetInt("tightChest"));
        hardToBreath = Convert.ToBoolean(PlayerPrefs.GetInt("hardToBreath"));
        itchy = Convert.ToBoolean(PlayerPrefs.GetInt("itchy"));
        sick = Convert.ToBoolean(PlayerPrefs.GetInt("sick"));
    }
}
