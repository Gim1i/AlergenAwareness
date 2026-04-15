using UnityEngine;

[CreateAssetMenu(fileName = "All_Driving_Text", menuName = "Scriptable Objects/All_Driving_Text")]
public class All_Driving_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four, alergy }; //Coppied from Game_Process_Manager to help with readability
    public readonly bool[] sectionConfiguration = new[] { false, false, false }; //Wether these sections have choices, sub sections or random elements
    public readonly (daySection section,
        (int eventId, (string text, int choiceID)[])[] text)[]
        sectionsCovered = new[] //Sections covered by this text store with their text, choices and sub sections after
    {
        (daySection.workStartTravel, firstWorkDriveText),
        (daySection.workEndTravel, secondWorkDriveText),
        (daySection.homeTravel, homeDriveText)
    };

    //
    // All other text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "*_*" = Narated actions
    //

    //
    //  Drive to first work
    //
    static public (int eventId, (string text, int choiceID)[])[] firstWorkDriveText { get; private set; } = new[] {
        //Regular Process
        (-1, new [] {                                   
            ("", -1)
        }),
        //Car crash ahead
        (0, new [] {                                    
            ("...", -1),
            ("...", -1),
            ("Why am I queueing for so long?", -1),
            ("If this keeps up I might not make it to work on time", -1),
            ("...", -1),
            ("Finaly moving again", -1),
            ("*Drives by a car crash*", -1),
            ("Ahh that makes sense", -1)
        }),
        //Road closure
        (1, new [] {                                   
            ("...", -1),
            ("That road is closed now?", -1),
            ("I guess I'm going to have to switch route this week", -1),
            ("Hopefully I can still make it to work on time", -1)
        }),
        //Car doesn't start
        (2, new [] {                                    
            ("*Tries to start car*", -1),
            ("*Tries again*", -1),
            ("Damn thing wont start", -1),
            ("*Sigh* I've got to call Tyler", -1),
            ("*Ring Ring Rin-*", -1),
            ("Hi Tyler, I'm probably not going to be able to be in this morning as my car isn't starting", -1),
            ("Tyler: What?! Thats not good", -1),
            ("Tyler: Do you think you will be able to make get here by lunch time?", -1),
            ("Yea I can be there by lunch. Just have to give the car to the garage", -1),
            ("Tyler: Good to hear! I'll see you after the lunch break then", -1),
            ("*Click*", -1),
            ("Luckly the issue was quite minor so the car will be fixed by this evening", -1),
        })
    };

    //
    //  Drive from second work
    //
    static public (int eventId, (string text, int choiceID)[])[] secondWorkDriveText { get; private set; } = new[] {
        //Regular Process
        (-1, new [] {
            ("", -1)
        }),
        //Car crash ahead
        (0, new [] {
            ("...", -1),
            ("...", -1),
            ("Why am I queueing for so long?", -1),
            ("If this keeps up I might not have much time this evening", -1),
            ("...", -1),
            ("Finaly moving again", -1),
            ("*Drives by a car crash*", -1),
            ("Oh, that explains the delay", -1)
        }),
        //Road closure
        (1, new [] {
            ("...", -1),
            ("That road is closed now?", -1),
            ("I guess I'm going to have to switch route this week", -1)
        }),
        //Car doesn't start
        (2, new [] {
            ("*Tries to start car*", -1),
            ("*Tries again*", -1),
            ("Damn thing wont start", -1),
            ("*Sigh* I guess I'm not doing anything tonight", -1),
            ("Hopefully I can still make it into work tommorow", -1),
            ("...", -1),
            ("Luckly the issue was quite minor so the car will be fixed by tommorow morning", -1),
        })
    };

    //
    //  Drive home
    //
    static public (int eventId, (string text, int choiceID)[])[] homeDriveText { get; private set; } = new[] {
        (-1, new [] {                                  //Regular Process
            ("...", -1),
        }),
        (0, new [] {                                   //
            ("...", -1),
        }),
        (1, new [] {                                   //
            ("...", -1),
        })
    };
}
