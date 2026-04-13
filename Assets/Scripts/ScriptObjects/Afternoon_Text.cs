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
            ( "Go to a resturaunt",     (int)option.three, foodReactionChance.none, 3,  new string[0]),
            ( "Go to a party",          (int)option.four,  foodReactionChance.none, 4,  new string[0])
        },
        new[] {
            ( "Relax at home",          (int)option.one,   foodReactionChance.none, 0,  new string[0]),
            ( "Go to the gym",          (int)option.two,   foodReactionChance.none, 1,  new string[0])
        },
        //Home dinner choices
        new[] {
            ( "Cook", (int)option.one, foodReactionChance.none, -1, new[] {
                    "I'll just cook at home, no need for anything fancy today",
                    randomCookedChoice,
                    "...",
                    randomCookedComment
                }),
            ( "Order a takeaway", (int)option.two, foodReactionChance.none, 2, new[] {
                    "Cooking sounds like too much of a hastle right now, I'll just order a takeaway instead"
                })
        },
        new[] {
            ( "Pizza", (int)option.one, foodReactionChance.pizza, -1, new[] {
                    "I'll just order a pizza",
                    "*Ring Ring R-*",
                    "Phone attendent: Hello, this is Mahjong pizza. What can I help you with today?",
                    "I'd like to order a "+randomPizzaSize+" "+randomPizzaChoice+" pizza please",
                    "Phone attendent: Would you like anything else with that?",
                    randomPizzaExtra,
                    "Phone attendent: Ok, do you want it delivered or will you pick it up?",
                    "Deliver it please",
                    "Phone attendent: Ok, that will arive in approximately 30 minutes",
                    "Ok, have a good day!",
                    "Phone attendent: Thank you, you too!",
                    "*Click*",
                    "Guess I should find something to do for 30 minues",
                    "...",
                    "...",
                    "...",
                    "*Ding-Dong* Oh thats probably the pizza",
                    "Mahjong driver: "+randomPizzaChoice+" pizza right?",
                    "Yep thats mine",
                    "Mahjong driver: Here you go then",
                    "Thanks, have a good night!",
                    "Mahjong driver: Thank you, you too!",
                    "*Opens lid* This is looking good, time to dig in!",
                    "...",
                    "That was a good pizza"
                }),
            ( "Chinese", (int)option.two, foodReactionChance.chinese, -1, new[] {
                    "Chinese sounds good to me",
                    "*Ring Ring Ri-*",
                    "Phone attendent: Hello, this is _. What can I help you with today?",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
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
            ("*Heads to the gym*", -1),
            (randomGymComment[0], -1),
            (randomGymComment[1], -1),
            (randomGymComment[2], -1),
            ("What should I for dinner?", 2)
        }),
        //Home dinner sub choice
        (new [] {("What should I order?", 3)}),
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
        randomCookedChoice = cookedChoice[Random.Range(0, cookedChoice.Length)];
        randomCookedComment = cookedComment[Random.Range(0, cookedComment.Length)];
        randomPizzaChoice = pizzaChoice[Random.Range(0, pizzaChoice.Length)];
        randomPizzaSize = pizzaSize[Random.Range(0, pizzaSize.Length)];
        randomPizzaExtra = pizzaExtra[Random.Range(0, pizzaExtra.Length)];

        int gymCmntSctd = Random.Range(0, gymComment.Length);
        int gymSubCmntSctd = Random.Range(0, gymComment[gymCmntSctd].extensions.Length);
        randomGymComment = new[]{ //Complex method of selecting a the random gym comment said
            gymComment[gymCmntSctd].start,
            gymComment[gymCmntSctd].extensions[gymSubCmntSctd][0],
            gymComment[gymCmntSctd].extensions[gymSubCmntSctd][1]
        };
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

    //Gym set of (3) lines
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
        ( "It's really quiet today, I wonder why?", new[] {
            new[] {
                "...",
                "..."
            },
            new[] {
                "...",
                "..."
            },
            new[] {
                "...",                                          
                "It's surprisingly nice when its quiet here"    
            }                                                   
        }),                                                     
        ( "Wow its busy", new[] { new[] { "...", "..." }}),     
        ( "...", new[] { new[] { "...", "..." } })
    };
    static private string[] randomGymComment = new[]{ gymComment[0].start, gymComment[0].extensions[0][0], gymComment[0].extensions[0][1] };

    //Homemade food stuff
    static private string[] cookedChoice = new[] {
        "Spaghetti bolognaise sounds good",
        "Chicken strips would be nice and easy to make",
        "I fancy chili con carne tonight... Wait did I get beef mince?",
        "Chicken soup sounds like a good idea tonight",
        "Mac & cheese should be easy to make",
        "Bacon & mushroom risotto would be a good choice, the bacon needs to be using anyway",
        "I should try and make that ghormeh sabzi recipie I found earlier"
    };
    static private string randomCookedChoice = cookedChoice[0];
    static private string[] cookedComment = new[] {
        "I should use less salt next time",
        "...",
        "...",
        "Huh, that was really good... I should write that down",
        "*Sigh* I still have to clean up"
    };
    static private string randomCookedComment = cookedComment[0];

    //Homemade food stuff
    static private string[] pizzaChoice = new[] {
        "Margerehta",
        "Margerehta",
        "Peperoni",
        "Peperoni",
        "Ham and pineapple",
        "Ham and pineapple",
        "Sausage and chorizo"
    };
    static private string randomPizzaChoice = pizzaChoice[0];
    static private string[] pizzaSize = new[] {
        "10\"",
        "10\"",
        "12\"",
        "12\"",
        "12\"",
        "14\""
    };
    static private string randomPizzaSize = pizzaSize[0];
    static private string[] pizzaExtra = new[] {
        "No thank you",
        "No thank you",
        "No thank you",
        "Some small chips please",
        "A can of bepis please"
    };
    static private string randomPizzaExtra = pizzaExtra[0];
}
