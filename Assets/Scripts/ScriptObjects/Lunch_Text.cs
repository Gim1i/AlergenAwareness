using UnityEngine;

[CreateAssetMenu(fileName = "Lunch_Text", menuName = "Scriptable Objects/Lunch_Text")]
public class Lunch_Text : ScriptableObject
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
    //    "-_"  = Naration
    //    "*_*" = Narated actions
    //

    //
    //  Lunch choices
    //
    static public readonly (int eventId, (string text, int choiceID)[])[] text = new[] {
        (-1, new [] {                                  //Regular Process
            ("Where shoud I go for lunch today?", 0)
        })
    };

    // Lists the choices the player can make, alongside all relevant information
    static public readonly (string text, int associatedOption, foodReactionChance reactionCheck, int subSectionID, string[] uniqueLinesAfter)[][] choiceText = new[] {
        //Initial lunch choice
        new[] {                                             
            ( "Local coffee shop",           (int)option.two,   foodReactionChance.none, 0,  new string[0]),
            ( "Jenns",                       (int)option.three, foodReactionChance.none, 1,  new string[0]),
            ( "Salad deli bar",              (int)option.four,  foodReactionChance.none, 3,  new string[0]),
            ( "Have prepared Lunch instead", (int)option.one,   foodReactionChance.none, -1, new[] {
                    "...",
                    "...",
                    randomLunchComment,
                    "..."
                })
        },
        //Coffee shop choice
        new[] {
            ( "Coffee and a sandwitch", (int)option.one, foodReactionChance.none, -1, new[] {
                    "Can I get a latte and a "+randomSandwitchChoice,
                    "Barista 1: You can! Would you like anything else?",
                    "No thanks, thats all I want",
                    "Barista 1: Ok, the coffee will be ready in 3 minutes and your number is "+randomLunchOrderNumber,
                    "Thanks!",
                    "...",
                    "...",
                    "Barista 2: "+randomLunchOrderNumber+"!",
                    "Thats me, thanks",
                    "Barista 2: Have a good day!",
                    "You too!"
                }),
            ( "Tea and cake", (int)option.two, foodReactionChance.none, -1, new[] {
                    "Can I get a latte and... What cake options do you have?",
                    "Barista 1: We have chocolate cake, sponge cake and cheese cake right now",
                    "Barista 1: Which would you like?",
                    "I'll take a slice of "+randomCakeChoice+" please.",
                    "Barista 1: I'll go get that for you now then",
                    "...",
                    "Barista 1: Heres your chocolate cake and your coffee will be ready in 2 minutes",
                    "Barista 1: Your order number is "+randomLunchOrderNumber,
                    "Thanks, have a good day!",
                    "...",
                    "...",
                    "Barista 2: "+randomLunchOrderNumber+"!",
                    "Thats me, thanks",
                    "Barista 2: Enjoy your lunch!",
                    "Thank you!"
                }),
            ( "Just a sandwitch", (int)option.three, foodReactionChance.none, -1, new[] {
                    "Can I get a "+randomSandwitchChoice,
                    "Barista 1: You can! Would you like anything else?",
                    "No thanks, thats all I want",
                    "Barista 1: Heres your sandwitch, have a good day!",
                    "Thanks, you too!"
                }),
            ( "Ask about allergies", (int)option.alergy, foodReactionChance.none, -1, new[] {
                    "I have an allergy to nuts, how are nuts handled here?",
                    "Barista 1: We're a nut free shop, so you wont have to worry about that here!",
                    "Awsome, thats great to hear!",
                    "Barista 1: So what can I get for you then?"
                })
        },
        //Jenns choices
        new[] {
            ( "Sausage roll and a coffee", (int)option.one, foodReactionChance.jenns, -1, new[] {
                    ""
                }),
            ( "Baguete and a coffee", (int)option.two, foodReactionChance.jenns, -1, new[] {
                    "Can I get a "+randomBaguetteChoice+" and a latte",
                    "Jenns worker: "
                }),
            ( "Sausage roll and a cookie", (int)option.three, foodReactionChance.jenns, -1, new[] {
                    ""
                }),
            ( "Ask about allergies", (int)option.alergy, foodReactionChance.none, 2, new[] {
                    "I have an allergy to nuts, how are nuts handled here?",
                    "Jenns worker: We are unable to garantee there are no nuts in our products due to us using nuts in our kitchen",
                    "Jenns worker: But, we have an alergen table you could look though to see what you can have"
                })
        },
        new[] {
            ( "No", (int)option.two, foodReactionChance.none, -1, new[] { "No thanks" }),
            ( "Yes", (int)option.one, foodReactionChance.none, -1, new[] { "Yes please" })
        },
    };

    // All sub sections
    static public readonly (string text, int choiceID)[][] subSectionText = new[] {
        //Local coffee shop
        (new [] {                           
            ("*Door bell dings*", -1),
            ("Barista 1: What can I do for you today?", -1),
            ("What should I choose?", 1),
        }),
        //Jenns
        (new [] {                         
            ("Jenns worker: Hello, welcome to Jenns! What can I get for you today?", -1),
            ("What should I choose?", 2)
        }),
        (new [] {
            ("Jenns worker: Would you like to see it?", 3)
        }),
        //Salad deli
        (new [] {                          
            ("...", -1)
        })
    };

    //
    // All random elements for these sections
    //
    public void RegenerateRandomElements() //Get new a new random selection for this day's version of this section
    {
        randomLunchComment      = lunchComment[Random.Range(0, lunchComment.Length)];
        randomSandwitchChoice   = sandwitchChoice[Random.Range(0, sandwitchChoice.Length)];
        randomCakeChoice        = cakeChoice[Random.Range(0, cakeChoice.Length)];
        randomBaguetteChoice    = baguetteChoice[Random.Range(0, baguetteChoice.Length)];

        randomLunchOrderNumber  = Random.Range(100, 1000);
    }

    //Homemade food choice
    static private string[] lunchComment = new[] {
        "*Under breath* Needs more mayo",
        "mmmm",
        "...",
        "...",
        "...",
        "...",
        "Hmm, kinda plain"
    };
    static private string randomLunchComment = lunchComment[0];

    //Coffee shop choice
    static private int randomLunchOrderNumber = 242;
    static private string[] sandwitchChoice = new[] {
        "ham & cheese sandwitch",
        "ham & cheese sandwitch",
        "ham & cheese sandwitch",
        "ham sandwitch",
        "tuna salad sandwitch",
    };
    static private string randomSandwitchChoice = sandwitchChoice[0];
    static private string[] cakeChoice = new[] {
        "chocolate cake",
        "sponge cake",
        "cheese cake"
    };
    static private string randomCakeChoice = cakeChoice[0];

    //Jenns choice
    static private string[] baguetteChoice = new[] {
        "Spicy chicken baguette for once",
        "Ham and cheese baguette",
        "Ham and cheese baguette",
        "Ham and cheese baguette",
        "Ham baguette",
        "Ham baguette",
        "Bacon baguette",
    };
    static private string randomBaguetteChoice = baguetteChoice[0];
}
