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
            ( "Salad deli bar",              (int)option.four,  foodReactionChance.none, 2,  new string[0]),
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
                    "Can I get a sausage roll and a latte",
                    "Jenns worker: You can although the latte will take a minute to be ready",
                    "Ok. I don't mind that",
                    "Jenns worker: I'll grab those for you now then",
                    "...",
                    "...",
                    "Jenns worker: Heres your latte and sausage roll",
                    "Jenns worker: Have a good day",
                    "You too!"
                }),
            ( "Baguete and a coffee", (int)option.two, foodReactionChance.jenns, -1, new[] {
                    "Can I get a "+randomBaguetteChoice+" and a latte",
                    "Jenns worker: Yes, I'll grab those for you now",
                    "...",
                    "...",
                    "Jenns worker: Heres your latte and "+randomBaguetteChoice,
                    "Jenns worker: Have a good day",
                    "You too!"
                }),
            ( "Sausage roll and a cookie", (int)option.three, foodReactionChance.jenns, -1, new[] {
                    "Can I get a sausage roll and...",
                    "What cookie options do you have?",
                    "Jenns worker: We have double chocolate, milk chocolate and smarties cookies",
                    "I'll take a "+randomCookieChoice+" too",
                    "Jenns worker: So a sausage roll and a "+randomCookieChoice+"?",
                    "Yep, thats right",
                    "Jenns worker: Ok, I'll grab those for you then",
                    "Jenns worker: Heres your "+randomCookieChoice+" and sausage roll",
                    "Jenns worker: Have a good day",
                    "You too!"
                }),
            ( "Ask about allergies", (int)option.alergy, foodReactionChance.none, -1, new[] {
                    "I have an allergy to nuts, how are nuts handled here?",
                    "Jenns worker: We are unable to garantee there are no nuts in our products due to us using nuts in our kitchen",
                    "Jenns worker: But we do go though rigorous procedures to prevent cross-contamination in all of our food",
                    "Jenns worker: We have an allergen table you could look though that I could get if you'd want",
                    "Jenns worker: Would you like to see it?"
                })
        },
        //Salad deli choices
        new[] {
            ( "Ask for table", (int)option.one, foodReactionChance.none, 3, new[] {
                    "I'd like a table for one please",
                    "Deli cashier: Perfect, we have a table over here for you!",
                    "Deli cashier: Please follow me"
                }),
            ( "Ask about allergies", (int)option.alergy, foodReactionChance.none, -1, new[] {
                    "I have an allergy to nuts, how are nuts handled here?",
                    "Deli cashier: We are unable to garantee there are no nuts in our products due to us using nuts in our kitchen",
                    "Deli cashier: And, while we do have procedures in place to prevent cross-contaimation in our kitchen...",
                    "Deli cashier: we can't garentee any security with the salad bar",
                    "Deli cashier: Would you like to see our allergen table?"
                })
        },
        new[] {
            ( "Deli bar", (int)option.one, foodReactionChance.saladDeli, -1, new[] {
                    "I'll have the deli bar option please",
                    "Deli server: Sure, I'll go grab your bowl then",
                    "...",
                    "Deli server: Here's your bowl, feel free to go grab anything you want from the counters over there!",
                    "Deli server: Is that all?",
                    "Yes. Thank you!",
                    "Deli server: Your welcome!",
                    "... 20 minutes later ...",
                    "This deli bar hasn't been that bad, I'll definitely consider coming again",
                    "*Gets up and leaves*",
                    "Deli cashier: Thank you for visiting today, and I hope to see you again soon!"
                }),
            ( "Lasagne with chips", (int)option.two, foodReactionChance.saladDeli, -1, new[] {
                    "Can I get the Lasagne please?",
                    "Deli server: With chips or salad?",
                    "Chips please",
                    "Deli server: Ok, do you want anything else?",
                    "No thank you, thats all I want for now",
                    "Deli server: Have a good meal then!",
                    "Thanks!",
                    "...",
                    "Deli server 2: Lasagne with chips?",
                    "Yes thats mine. Thanks",
                    "Hmm, this looks good!",
                    "...",
                    "That was some good lasagne, I should come here again!",
                    "*Gets up and leaves*",
                    "Deli cashier: Thank you for visiting today, and I hope to see you again soon!"
                }),
            ( "Mac & cheese", (int)option.three, foodReactionChance.saladDeli, -1, new[] {
                    "..."
                })
        }
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
        //Salad deli
        (new [] {                          
            ("Deli cashier: Welcome to Sarah's Salad Deli!", -1),
            ("Deli cashier: What can I do for you?", 3)
        }),
        (new [] {
            ( "Deli cashier: This table right here please", -1),
            ( "Deli cashier: Heres the menu and someone will come around to get your order in approximately 5 minutes", -1),
            ( "Deli cashier: Any questions?", -1),
            ( "Nope, I'm good for now", -1),
            ( "Deli cashier: Have a good meal then!", -1),
            ( "...", -1),
            ( "...", -1),
            ( "Deli server: Hello, what can I get for you today?", 4)
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
        randomCookieChoice      = cookieChoice[Random.Range(0, cookieChoice.Length)];

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
        "tuna salad sandwitch"
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
        "spicy chicken baguette for once",
        "ham and cheese baguette",
        "chicken and mayonase baguette",
        "chicken and mayonase baguette",
        "chicken salad baguette",
        "ham baguette",
        "bacon baguette"
    };
    static private string randomBaguetteChoice = baguetteChoice[0]; 
    static private string[] cookieChoice = new[] {
        "double chocolate",
        "milk chocolate",
        "smarties cookies"
    };
    static private string randomCookieChoice = cookieChoice[0];
}
