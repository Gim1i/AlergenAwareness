using UnityEngine;

[CreateAssetMenu(fileName = "Display_Text", menuName = "Scriptable Objects/Display_Text")]
public class Display_Text : ScriptableObject
{
    private enum option { unchosen, one, two, three, four }; //Coppied from Game_Process_Manager to help with readability

    //
    // All text to display, sorted by section, if its event reliant and if its a player choice
    // Text formating (underscores mean anything here):
    //    "-_"  = Naration
    //    "*_*" = Narated actions
    //      
    public readonly (daySection section,(int eventId, (string text, int choice)[])[])[] textBySection = new[]
    { 
        (daySection.dayStart, new[]{
            (-1, new [] {                                  //Regular Process
                ("*Alarm beeping*", -1),
                ("...", -1),
                ("Uuurg, damn it", -1),
                ("*Gets up and dressed for work*", -1),
                ("Should I prepare lunch today?", 0),
                ("Time to head to work then", -1)
            }),
            (0, new [] {                                   //Early Wake
                ("...", -1),
                ("...", -1),
                ("Why don't I hear my alarm?", -1),
                ("*Jolts out of bed and checks the clock*", -1),
                ("Oh I woke up early *Sigh*", -1),
                ("*Gets dressed for work*", -1),
                ("Should I prepare lunch today?", 0),
                ("Time to head to work then", -1)
            }),
            (1, new [] {                                   //Late Wake
                ("...", -1),
                ("...", -1),
                ("Why don't I hear my alarm?", -1),
                ("*Jolts out of bed and checks the clock*", -1),
                ("Damn I'm late!", -1),
                ("*Dressed for work as fast as possible*", -1),
                ("Theres no time to prepare lunch today", -1),
                ("*Rushes out the door*", -1)
            })
        }),
        (daySection.workStartTravel, new[]{
            (-1, new [] {                                   //Regular Process
                ("", -1)
            }),
            (0, new [] {                                    //Car crash ahead
                ("...", -1),
                ("...", -1),
                ("Why am I queueing for so long?", -1),
                ("If this keeps up I might not make it to work on time", -1),
                ("...", -1),
                ("Finaly moving again", -1),
                ("*Drives by a car crash*", -1),
                ("Ahh that makes sense", -1)
            }),
            (1, new [] {                                    //Road closure
                ("...", -1),
                ("That road is closed now?", -1),
                ("I guess I'm going to have to switch route this week", -1),
                ("Hopefully I can still make it to work on time", -1)
            }),
            (2, new [] {                                    //Car doesn't start
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
                ("-Luckly the issue was quite minor so the car will be fixed by this evening", -1),
            })
        }),
        (daySection.firstWork, new[]{
            (-1, new [] {                                   //Regular Process
                ("Hi Tyler. Anything important I should know?", -1),
                ("Tyler: Nope, just a regular day!", -1),
                ("Good to hear! I'll get to working then", -1),
                ("*Tyler give a thumbs up*", -1)
            }),
            (0, new [] {                                    //Homemade food
                ("Hi Tyler. Whats happening over there?", -1),
                ("Tyler: Someone brought in some home-made food", -1),
                ("Tyler: It's really good, you should have some!", -1),
                ("Should I have some?", 1)
            }),
            (1, new [] {                                    //Shop food
                ("Hi Tyler. Whats happening over there?", -1),
                ("Tyler: Someone brought some food on their way to work", -1),
                ("Tyler: It's free so I recomend grabing some!", -1),
                ("Should I have some?", 2)
            }),
            (2, new [] {                                    //Down colleague
                ("Hi Tyler. Anything important I should know?", -1),
                ("Tyler: Someone called in sick earlier so we're going to be short-staffed today", -1),
                ("You know wether they'll be able to be here tommorow?", -1),
                ("Tyler: Luckly they said they should be here tommorow", -1),
                ("Good to hear! Give them a get well soon from me next time you speak to them", -1),
                ("Tyler: Will do!", -1),
                ("*Walk's of to start work*", -1)
            }),
            (3, new [] {                                    //Work celebration
                ("Hi Tyler. Why is the office is looking fun today?", -1),
                ("Tyler: Upper managment organised a party to celebrate us completing the project before the deadline", -1),
                ("Tyler: Theres free food and drinks if you want any", -1),
                ("Should I eat the food?", 3),
                ("Good to hear! Give them a get well soon from me next time you speak to them", -1),
                ("Tyler: Will do!", -1),
                ("*Walk's of to start work*", -1)
            })
        }),
        (daySection.lunch, new[]{
            (-1, new [] {                                   //Regular Process
                ("*Checks clock* Oh its lunch time!", -1),
                ("Where should I go for lunch?", 4)
            })
        }),
        (daySection.lunch, new[]{
            (-1, new [] {                                   //Regular Process
                ("...", -1)
            }),
            (0, new [] {                                    //Early Wake
                ("...", -1)
            }),
            (1, new [] {                                    //Late Wake
                ("...", -1)
            })
        })
    };

    //
    // SubSections are organised seperatly as they need unique ID-ing
    //
    public readonly (daySection section, (int linkedFrom, (string text, int choice)[])[])[] textForSubSections = new[] {
        (daySection.lunch, new[]{
            ((int)option.one, new [] {                           //Office break room
                ("...", -1),
                ("...", -1),
                ("*Under breath* Needs more mayo", -1),
                ("...", -1)
            }),
            ((int)option.two, new [] {                           //Local coffee shop
                ("...", -1)
            }),
            ((int)option.three, new [] {                         //Jenns
                ("...", -1)
            }),
            ((int)option.four, new [] {                          //Salad deli
                ("...", -1)
            })
        })
    };

    //
    // Lists all possible choices the player can make, alongside any unique dialouge, whether or not it causes a reaction check and any seconday choices
    //
    public (string text, int associatedOption, foodReactionChance reactionCheck, bool hasSubSection, string[] uniqueLinesAfter)[][] choiceText { get; } = new[] {
        //Day start
        new[] {                                             //Lunch prep
            ( "No", (int)option.two, foodReactionChance.none, false, new string[0]), 
            ( "Yes", (int)option.one, foodReactionChance.none, false,
                new[] {
                    "*Prepares lunch*"
                })
            },

        //Work 1
        new[] {                                             //Brought in home-made food
            ( "No", (int)option.two, foodReactionChance.none, false,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.homemadeFood, false,
                new[] {
                    "Will do!"
                })
            },
        new[] {                                             //Brought in shop food
            ( "No", (int)option.two, foodReactionChance.none, false,
                new[] {
                    "I'll pass on that, but thanks for offering"
                }),
            ( "Yes", (int)option.one, foodReactionChance.broughtInShopFood, false,
                new[] {
                    "Will do!"
                })
            },
        new[] {                                             //Work party
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

        //Lunch
        new[] {                                             //Lunch
            ( "Local coffee shop",           (int)option.two,   foodReactionChance.coffeeShop, true, new string[0]),
            ( "Jenns",                       (int)option.three, foodReactionChance.jenns,      true, new string[0]),
            ( "Salad deli bar",              (int)option.four,  foodReactionChance.saladDeli,  true, new string[0]),
            ( "Have prepared Lunch instead", (int)option.one,   foodReactionChance.none,       true, new string[0])
            },
    };

    public string testText { get; }
}
