using UnityEngine;

[CreateAssetMenu(fileName = "Afternoon_Text", menuName = "Scriptable Objects/Afternoon_Text")]
public class Afternoon_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four, alergy }; //Coppied from Game_Process_Manager to help with readability
    public readonly bool[] sectionConfiguration = new[] { true, true, true }; //Wether these sections have choices, sub sections or random elements
    public readonly (daySection section,
        (int eventId, (string text, int choiceID)[])[] text,
        (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] choices,
        (string text, int choiceID)[][] subSections)[]
        sectionsCovered = new[] //Sections covered by this text store with their text, choices and sub sections after
    {
        (daySection.lunch, text, choiceText, subSectionText)
    };

    //
    // All other text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "*_*" = Narated actions
    //

    //
    //  Lunch choices
    //
    static public readonly (int eventId, (string text, int choiceID)[])[] text = new[] {
        //Regular Process
        (-1, new [] {                                  
            ("What should I do this afternoon/evening?", 0)
        }),
        //If "Car doesn't start" event is rolled for driving home
        (1, new [] {                                  
            ("What should I do with my remaining afternoon/evening?", 1)
        })
    };

    // Lists the choices the player can make, alongside all relevant information
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] choiceText = new[] {
        //Initial afternoon activity choice
        new[] {
            ( "Relax at home",          (int)option.one,   foodReactionChance.none, 0,  new string[0]),
            ( "Go to the gym",          (int)option.two,   foodReactionChance.none, 1,  new string[0]),
            ( "Go to a resturaunt",     (int)option.three, foodReactionChance.none, 2,  new string[0]),
            ( "Go to a party",          (int)option.four,  foodReactionChance.none, 3,  new string[0])
        },
        new[] {
            ( "Relax at home",          (int)option.one,   foodReactionChance.none, 0,  new string[0]),
            ( "Go to the gym",          (int)option.two,   foodReactionChance.none, 1,  new string[0])
        },
        //Home dinner choices
        new[] {
            ( "Cook", (int)option.one, foodReactionChance.none, -1, new[] {
                    "..."
                }),
            ( "Order a takeaway", (int)option.two, foodReactionChance.none, -1, new[] {
                    "..."
                })
        },
        new[] {
            ( "Pizza", (int)option.one, foodReactionChance.pizza, -1, new[] {
                    "..."
                }),
            ( "Chinese", (int)option.two, foodReactionChance.chinese, -1, new[] {
                    "..."
                })
        },
        //Resturaunt choices
        new[] {
            ( "Ask for table", (int)option.one, foodReactionChance.none, -1, new[] {
                    "..."
                }),
            ( "Ask about allergies", (int)option.alergy, foodReactionChance.none, -1, new[] {
                    "..."
                })
        },
        new[] {
            ( "Deli bar", (int)option.one, foodReactionChance.resturaunt, -1, new[] {
                    "..."
                }),
            ( "Lasagne with chips", (int)option.two, foodReactionChance.resturaunt, -1, new[] {
                    "..."
                }),
            ( "Mac & Cheese", (int)option.three, foodReactionChance.resturaunt, -1, new[] {
                    "..."
                })
        },
        //Party choice
        new[] {
            ( "Have snacks", (int)option.one, foodReactionChance.none, -1, new[] {
                    "..."
                }),
            ( "Ask about allergies", (int)option.alergy, foodReactionChance.none, -1, new[] {
                    "..."
                })
        }
    };

    // All sub sections
    static public readonly (string text, int choiceID)[][] subSectionText = new[] {
        //Relax at home
        (new [] {
            ("I don't really fancy going out tonight", -1),
            (randomHomeRelaxComment[0], -1),
            (randomHomeRelaxComment[1], -1),
            (randomHomeRelaxComment[2], -1),
            (randomHomeRelaxComment[3], -1),
            ("What should I for dinner?", 2)
        }),
        //Gym
        (new [] {
            ("I should head to the gym, I've been slacking on excercie lately", -1),
            (randomGymComment[0], -1),
            (randomGymComment[1], -1),
            (randomGymComment[2], -1),
            ("What should I for dinner?", -1)
        }),
        //Resturant
        (new [] {
            ("...", -1)
        }),
        //Party
        (new [] {
            ("...", -1)
        })
    };

    //
    // All random elements for these sections
    //
    public void RegenerateRandomElements() //Get new a new random selection for this day's version of this section
    {
        randomHomeRelaxComment = homeRelaxComment[Random.Range(0, homeRelaxComment.Length)];
        randomGymComment = gymComment[Random.Range(0, gymComment.Length)];
    }

    //Staying home set of (4) lines
    static private string[][] homeRelaxComment = new[] {
        new[] {
            "Actualy didn't a new show come out on fetnlix? I should watch that tonight",
            "...",
            "...",
            "This new show is fairly good but I'm starting to want dinner"
        },
        new[] {
            "...",
            "...",
            "...",
            "..."
        },
        new[] {
            "...",
            "...",
            "...",
            "..."
        }
    };
    static private string[] randomHomeRelaxComment = homeRelaxComment[0];

    //Gym set of (4) lines
    /*static private string[][] gymComment = new[] {
        new[] {
            "Hmm, I should go for a new personal record",
            "...",
            "Damn didn't make it. Maybe next time"
        },
        new[] {
            "Hmm, I should go for a new personal record",
            "...",
            "Damn didn't make it. Maybe next time"
        },
        new[] {
            "...",
            "...",
            "..."
        }
    };*/
    static private (string start, string[][] extensions)[] gymComment = new[] {
        ( "Hmm, I should go for a new personal record", new[] {
            new[] {
                "...",
                "Damn didn't make it. Maybe next time"
            },
            new[] {
                "...",
                "So close. I can definitely get there soon"
            },
            new[] {
                "...",
                "Yes! Finaly got that new record"
            }
        }),
        ( "Hmm, I should go for a new personal record", new[] {
            new[] {
                "...",
                "Damn didn't make it. Maybe next time"
            },
            new[] {
                "...",
                "So close. I can definitely get there soon"
            },
            new[] {
                "...",
                "Yes! Finaly got that new record"
            }
        }),
    };
    static private string[] randomGymComment = gymComment[0];

    //Homemade food choice
    static private string[] lunchComment = new[] {
        "..."
    };
    static private string randomLunchComment = lunchComment[0];
}
