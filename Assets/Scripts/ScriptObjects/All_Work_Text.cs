using UnityEngine;

[CreateAssetMenu(fileName = "All_Work_Text", menuName = "Scriptable Objects/All_Work_Text")]
public class All_Work_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four, alergy }; //Coppied from Game_Process_Manager to help with readability
    public readonly bool[] sectionConfiguration = new[] { true, false, false }; //Wether these sections have choices, sub sections or random elements
    public readonly (daySection section,
        (int eventId, (string text, int choiceID)[])[] text,
        (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] choices)[]
        sectionsCovered = new[] //Sections covered by this text store with their text, choices and sub sections after
    {
        (daySection.firstWork, firstWorkText, firstWorkChoiceText),
        (daySection.secondWork, secondWorkText, secondWorkChoiceText)
    };

    //
    // All other text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "*_*" = Narated actions
    //

    //
    //  First work
    //
    static public readonly (int eventId, (string text, int choiceID)[])[] firstWorkText = new[] {
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
            ("Tyler: Someone brought in some "+randomHomeMadeFood, -1),
            ("Tyler: It's really good, you should have some!", -1),
            ("Should I have some?", 0)
        }),
        //Shop food
        (1, new [] {                                    
            ("Hi Tyler. Whats happening over there?", -1),
            ("Tyler: Someone brought some "+randomFirstBroughtFood+" on their way to work", -1),
            ("Tyler: It's free so I recomend grabing some!", -1),
            ("Should I have some?", 1)
        }),
        //Down colleague
        (2, new [] {                                    
            ("Hi Tyler. Anything important I should know?", -1),
            ("Tyler: Someone called in sick earlier so we're going to be short-staffed today", -1),
            ("You know wether they'll be able to be here tommorow?", -1),
            ("Tyler: Luckly they said they should be here tommorow", -1),
            ("Good to hear! Give them a get well soon from me next time you speak to them", -1),
            ("Tyler: Will do!", -1),
            ("*Walks of to start work*", -1)
        }),
        //Work celebration
        (3, new [] {                                   
            ("Hi Tyler. Why is the office is looking fun today?", -1),
            ("Tyler: Upper managment organised a party to celebrate us completing the project before the deadline", -1),
            ("Tyler: Theres free food and drinks if you want any", -1),
            ("Should I get some free food?", 2)
        })
    };

    // Lists the choices the player can make on day start, alongside all relevant information
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] firstWorkChoiceText = new[] {
        //Brought in home-made food
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, -1,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.broughtInHomeFood, -1,
                new[] {
                    "Will do!",
                    "",
                })
        },
        //Brought in shop food
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, -1,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.broughtInShopFood, -1,
                new[] {
                    "Will do!"
                })
        },
        //Work party
        new[] {                                             
            ( "No", (int)option.two, foodReactionChance.none, -1,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.workCelebration, -1,
                new[] {
                    "Will do! which do you recomend?",
                    "Tyler: The pepperoni was quite good when I tried it, though margherita is always a solid choice",
                    "Yea "+randomWorkPizzaChoice+" sounds like a good choice. Thanks!",
                    "Tyler: No problem! *Thumbs up*",
                    "*Walks over to the free food table and grabs a slice of "+randomWorkPizzaChoice+" pizza*",
                    "...",
                    "Huh, thats some decent pizza. Glad I had some",
                    "Anyway time to start work"
                })
        },

    };

    //
    //  Second work
    //
    static public readonly (int eventId, (string text, int choiceID)[])[] secondWorkText = new[] {
        //Regular Process
        (-1, new [] {
            ("Time to get back working", -1)
        }),
        //Shop food
        (0, new [] {
            ("Hi Tyler. Whats happening over there?", -1),
            ("Tyler: Someone brought some "+randomSecondBroughtFood+" on their way to work", -1),
            ("Tyler: It's free so I recomend grabing some!", -1),
            ("Should I have some?", 0)
        }),
        //Early end due to very bad weather
        (1, new [] {
            ("Time to get back working", -1),
            ("...", -1),
            ("...", -1),
            ("...", -1),
            ("*Looks out the window* Uhhh, that weather's looking real bad", 1),
            ("Hey Tyler, any chance we could finish early today?", -1),
            ("Getting home might become impossible for some if this weather keeps up", -1),
            ("Tyler: *Looks outside* Oh yea thats really bad, I might not make it home myself", -1),
            ("Tyler: Ok we'll finish early today, feel free to go and I'll let the rest of the office know shortly", -1),
            ("Thanks Tyler!", -1),
            ("Tyler: No problem!", -1)
        }),
        //Down colleague (NOT RANDOMLY GENERATED, continues if was selected in first work)
        (2, new [] {
            ("Tyler: Just a reminder, we're short-staffed so I'd appreciate if you got on with work promptly", -1),
            ("Oh yea, I forgot about that", -1),
            ("I'll get on with it immediately then", -1),
            ("*Tyler give a thumbs up*", -1)
        })
    };

    // Lists the choices the player can make on day start, alongside all relevant information
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] secondWorkChoiceText = new[] {
        //Brought in shop food
        new[] {
            ( "No", (int)option.two, foodReactionChance.none, -1,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.broughtInShopFood, -1,
                new[] {
                    "Will do!"
                })
        }
    };

    //
    // All random elements for these sections
    //
    public void RegenerateRandomElements() //Get new a new random selection for this day's version of this section
    {
        randomWorkPizzaChoice          = workPizzaChoice[Random.Range(0, workPizzaChoice.Length)];
        randomHomeMadeFood             = homeMadeFood[Random.Range(0, homeMadeFood.Length)];
        randomFirstBroughtFood         = broughtFood[Random.Range(0, broughtFood.Length)];
        for (int i = 0; i < 10; i++){ //Try to make the 2 random brought foods random (but don't force it)
            randomSecondBroughtFood    = broughtFood[Random.Range(0, broughtFood.Length)];
            if (randomSecondBroughtFood != randomFirstBroughtFood) { break; }
        }
    }

    //Homemade food brought in
    static private string[] homeMadeFood = new[] {
        "brownies",
        "sponge cake",
        "chocolate cake",
        "cupcakes",
        "cookies"
    };
    static private string randomHomeMadeFood = homeMadeFood[0];

    //Shop food brought in
    static private string[] broughtFood = new[] {
        "cookies",
        "cookies",
        "doughnuts",
        "doughnuts",
        "muffins",
        "muffins",
        "cake"
    };
    static private string randomFirstBroughtFood = broughtFood[0];
    static private string randomSecondBroughtFood = broughtFood[2];

    //Work food choice
    static private string[] workPizzaChoice = new[] {
        "pepperoni",
        "margherita"
    };
    static private string randomWorkPizzaChoice = workPizzaChoice[0];
}
