using UnityEngine;

[CreateAssetMenu(fileName = "All_Work_Text", menuName = "Scriptable Objects/All_Work_Text")]
public class All_Work_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four }; //Coppied from Game_Process_Manager to help with readability
    public readonly bool[] sectionConfiguration = new[] { true, false }; //Wether these sections have choices or sub sections
    public readonly (daySection section,
        (int eventId, (string text, int choice)[])[] text,
        (string text, int associatedOption, foodReactionChance reactionCheck, bool hasSubSection, string[] uniqueLinesAfter)[][] choices)[]
        sectionsCovered = new[] //Sections covered by this text store with their text, choices and sub sections after
    {
        (daySection.firstWork, firstWorkText, firstWorkChoiceText),
        (daySection.secondWork, secondWorkText, secondWorkChoiceText)
    };

    //
    // All other text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "-_"  = Naration
    //    "*_*" = Narated actions
    //

    //
    //  First work
    //
    static public readonly (int eventId, (string text, int choice)[])[] firstWorkText = new[] {
        //Regular Process
        (-1, new [] {                                   
            ("Hi Tyler. Anything important I should know?", -1),
            ("Tyler: Nope, just a regular day!", -1),
            ("Good to hear! I'll get to working then", -1),
            ("*Tyler give a thumbs up*", -1)
        }),
        //Homemade food
        (0, new [] {                                    
            ("Hi Tyler. Whats happening over there?", -1),
            ("Tyler: Someone brought in some home-made food", -1),
            ("Tyler: It's really good, you should have some!", -1),
            ("Should I have some?", 1)
        }),
        //Shop food
        (1, new [] {                                    
            ("Hi Tyler. Whats happening over there?", -1),
            ("Tyler: Someone brought some food on their way to work", -1),
            ("Tyler: It's free so I recomend grabing some!", -1),
            ("Should I have some?", 2)
        }),
        //Down colleague
        (2, new [] {                                    
            ("Hi Tyler. Anything important I should know?", -1),
            ("Tyler: Someone called in sick earlier so we're going to be short-staffed today", -1),
            ("You know wether they'll be able to be here tommorow?", -1),
            ("Tyler: Luckly they said they should be here tommorow", -1),
            ("Good to hear! Give them a get well soon from me next time you speak to them", -1),
            ("Tyler: Will do!", -1),
            ("*Walk's of to start work*", -1)
        }),
        //Work celebration
        (3, new [] {                                   
            ("Hi Tyler. Why is the office is looking fun today?", -1),
            ("Tyler: Upper managment organised a party to celebrate us completing the project before the deadline", -1),
            ("Tyler: Theres free food and drinks if you want any", -1),
            ("Should I eat the food?", 3),
            ("Good to hear! Give them a get well soon from me next time you speak to them", -1),
            ("Tyler: Will do!", -1),
            ("*Walk's of to start work*", -1)
        })
    };

    // Lists the choices the player can make on day start, alongside all relevant information
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, bool hasSubSection, string[] uniqueLinesAfter)[][] firstWorkChoiceText = new[] {
        //Brought in home-made food
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, false,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.homemadeFood, false,
                new[] {
                    "Will do!"
                })
            },
        //Brought in shop food
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, false,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.broughtInShopFood, false,
                new[] {
                    "Will do!"
                })
            },
        //Work party
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, false,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.workCelebration, false,
                new[] {
                    "Will do! which do you recomend?",
                    "The pepperoni was quite good when I tried it, though margherita is always a solid choice"
                })
            },

    };

    //
    //  Second work
    //
    static public readonly (int eventId, (string text, int choice)[])[] secondWorkText = new[] {
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
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, bool hasSubSection, string[] uniqueLinesAfter)[][] secondWorkChoiceText = new[] {
        //Lunch prep
        new[] {
            ( "...", (int)option.two, foodReactionChance.none, false, new string[0])
        }
    };
}
