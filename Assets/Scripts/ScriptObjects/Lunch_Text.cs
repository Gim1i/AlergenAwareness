using UnityEngine;

[CreateAssetMenu(fileName = "Lunch_Text", menuName = "Scriptable Objects/Lunch_Text")]
public class Lunch_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four }; //Coppied from Game_Process_Manager to help with readability
    public readonly bool[] sectionConfiguration = new[] { true, true }; //Wether these sections have choices or sub sections
    public readonly (daySection section,
        (int eventId, (string text, int choice)[])[] text,
        (string text, int associatedOption, foodReactionChance reactionCheck, bool hasSubSection, string[] uniqueLinesAfter)[][] choices,
        (int linkedFrom, (string text, int choice)[])[] subSections)[]
        sectionsCovered = new[] //Sections covered by this text store with their text, choices and sub sections after
    {
        (daySection.lunch, text, choiceText, subSectionText)
    };

    //
    // All other text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "-_"  = Naration
    //    "*_*" = Narated actions
    //

    //
    //  Lunch choices
    //
    static public readonly (int eventId, (string text, int choice)[])[] text = new[] {
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

    // Lists the choices the player can make, alongside all relevant information
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, bool hasSubSection, string[] uniqueLinesAfter)[][] choiceText = new[] {
        //Initial lunch choice
        new[] {                                             
            ( "Local coffee shop",           (int)option.two,   foodReactionChance.coffeeShop, true, new string[0]),
            ( "Jenns",                       (int)option.three, foodReactionChance.jenns,      true, new string[0]),
            ( "Salad deli bar",              (int)option.four,  foodReactionChance.saladDeli,  true, new string[0]),
            ( "Have prepared Lunch instead", (int)option.one,   foodReactionChance.none,       false, new [] {
                    "...",
                    "...",
                    "*Under breath* Needs more mayo",
                    "..."
                })
        },
        //Coffee shop choice
        new[] {                                             
            ( "Local coffee shop",           (int)option.two,   foodReactionChance.coffeeShop, true, new string[0])
        }
    };
    // All sub sections
    static public readonly (int linkedFrom, (string text, int choice)[])[] subSectionText = new[] {
        //Local coffee shop
        ((int)option.two, new [] {                           
            ("*Door bell dings*", -1),
            ("Barista: What can I do for you today?", -1),
            ("What should I choose?", 0),
        }),
        //Jenns
        ((int)option.three, new [] {                         
            ("...", -1)
        }),
        //Salad deli
        ((int)option.four, new [] {                          
            ("...", -1)
        })
    };
}
