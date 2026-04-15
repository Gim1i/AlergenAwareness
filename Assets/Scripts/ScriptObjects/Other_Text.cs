using UnityEngine;

[CreateAssetMenu(fileName = "Other_Text", menuName = "Scriptable Objects/Other_Text")]
public class Other_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four, alergy }; //Coppied from Game_Process_Manager to help with readability
    public readonly bool[] sectionConfiguration = new[] { true, false, false }; //Wether these sections have choices, sub sections or random elements
    public readonly (daySection section,
        (int eventId, (string text, int choiceID)[])[] text,
        (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] choices)[]
        sectionsCovered = new[] //Sections covered by this text store with their text, choices and sub sections after
    { 
        (daySection.dayStart, startText, startChoiceText),
        (daySection.dayEnd, endText, endChoiceText)
    }; 

    //
    // All other text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "*_*" = Narated actions
    //

    //
    //  Day start
    //
    static public (int eventId, (string text, int choiceID)[])[] startText { get; private set; } = new[] {
         //Regular Process
        (-1, new [] {                                 
            ("*Alarm beeping*", -1),
            ("...", -1),
            ("Uuurg, damn it", -1),
            ("*Gets up and dressed for work*", -1),
            ("Should I prepare lunch today?", 0),
            ("Time to head to work then", -1)
        }),
        //Early Wake
        (0, new [] {                                   
            ("...", -1),
            ("...", -1),
            ("Why don't I hear my alarm?", -1),
            ("*Jolts out of bed and checks the clock*", -1),
            ("Oh I woke up early *Sigh*", -1),
            ("*Gets dressed for work*", -1),
            ("Should I prepare lunch today?", 0),
            ("Time to head to work then", -1)
        }),
        //Late Wake
        (1, new [] {                                   
            ("...", -1),
            ("...", -1),
            ("Why don't I hear my alarm?", -1),
            ("*Jolts out of bed and checks the clock*", -1),
            ("Damn I'm late!", -1),
            ("*Dressed for work as fast as possible*", -1),
            ("Theres no time to prepare lunch today", -1),
            ("*Rushes out the door*", -1)
        })
    };

    // Lists the choices the player can make on day start, alongside all relevant information
    static public (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] startChoiceText { get; private set; } = new[] {
        //Lunch prep
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, -1, new string[0]),
            ( "Yes", (int)option.one, foodReactionChance.none, -1, new[] { "*Prepares lunch*" })
        },
    };

    //
    //  Day end
    //
    static public (int eventId, (string text, int choiceID)[])[] endText { get; private set; } = new[] {
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

    // Lists the choices the player can make on day start, alongside all relevant information
    static public (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] endChoiceText { get; private set; } = new[] {
        //Lunch prep
        new[] {
            ( "...", (int)option.two, foodReactionChance.none, -1, new string[0])
        }
    };
}
