using System;
using System.Collections.Generic;
using UnityEngine;

public class Reaction_And_Event_Processing : MonoBehaviour
{
    public Reactions reactions;
    public Events events = new Events();

    private void Start()
    {
        reactions = new Reactions(transform.GetComponent<Modal_Managment>());
    }
    //
    // Processing for reaction stuffs
    //
    public class Reactions {
        enum reactionLevel { low, med, high }

        private Modal_Managment modalScript;

        //Get the modal script needed
        public Reactions(Modal_Managment modalScript) {
            this.modalScript = modalScript; ;
        }

        //
        // Do a random roll to determine if an reaction happens during an event or not
        //
        public void RollEventReaction(foodReactionSource eventID, int subID) {
            short[] eventChances = eventReactionChances[eventID][subID]; //Get the proper Sub-event
            short eventNumber = (short)UnityEngine.Random.Range(1, 1001);
            for (short i = 0; i < eventChances.Length; i++) { //Cycle though all the chances
                eventNumber -= eventChances[i];
                if (eventNumber <= 0) { //If one of the reaction chances was rolled
                    Debug.Log("Rolled " + (reactionLevel)i + " reaction for " + eventID);
                    ApplyReactionChanges((reactionLevel)i);
                    modalScript.ApplyEmotionChanges();
                    return;
                }
            }
        }

        // A debug only method of testing reactions
        #if DEBUG
        public void TestReactions(int level) {
            ApplyReactionChanges((reactionLevel)level);
        }
        #endif

        // Applies reaction changes to their PlayerPref
        private void ApplyReactionChanges(reactionLevel level)
        {
            var reactionChanges = sympoms[level]; //Get all listed changes for the reaction
            for (short e = 0; e < reactionChanges.emotions.Length; e++) //Update all emotion values
            {
                int currentValue = PlayerPrefs.GetInt(reactionChanges.emotions[e].variant.ToString());
                PlayerPrefs.SetInt(reactionChanges.emotions[e].variant.ToString(), currentValue + reactionChanges.emotions[e].change);
            }
            for (short a = 0; a < reactionChanges.afflicts.Length; a++) //Update all afflict states
            {
                PlayerPrefs.SetInt(reactionChanges.afflicts.ToString(), 1); //Sets afflict to true
            }
        }

        //
        // The stats for reaction chances. Chances are C/1000
        //
        private readonly Dictionary<foodReactionSource, short[][]> eventReactionChances = new Dictionary<foodReactionSource, short[][]>
        { //All potencial raction sources with their choice's chances for each reaction level and highest possible reaction
            { foodReactionSource.jenns, new[] {
                new short[]{ 215          }, //Sausage roll and a coffee
                new short[]{ 170          }, //Baguete and a coffee
                new short[]{ 325, 10      }  //Sausage roll and a cookie
            }},
            { foodReactionSource.saladDeli, new[] {
                new short[]{ 900, 75      }, //Deli bar
                new short[]{ 275, 15      }, //Lasagne with chips
                new short[]{ 250, 10      }  //Mac & Cheese
            }},
            { foodReactionSource.resturaunt, new[] {
                new short[]{ 300, 30      }, //Chicken pesto
                new short[]{ 300, 30      }, //Lemon sea bass
                new short[]{ 300, 30      }  //Waterside specialty burger
            }},
            { foodReactionSource.lightDrinking,     new[] {new short[]{ 320          }}},
            { foodReactionSource.heavyDrinking,     new[] {new short[]{ 535, 140     }}},
            { foodReactionSource.pizza,             new[] {new short[]{ 150          }}},
            { foodReactionSource.chinese,           new[] {new short[]{ 950, 125     }}},
            { foodReactionSource.broughtInHomeFood, new[] {new short[]{ 120          }}},
            { foodReactionSource.broughtInShopFood, new[] {new short[]{ 300          }}},
            { foodReactionSource.workCelebration,   new[] {new short[]{ 720, 30      }}}
        };
    
        //
        // All symptoms from each level of reaction
        //
        private readonly Dictionary<reactionLevel, ((emotionState variant, short change)[] emotions, afflictState[] afflicts)> sympoms =
        new Dictionary<reactionLevel, ((emotionState variant, short change)[], afflictState[])> {
            //Low level reaction
            { reactionLevel.low, (
                //Emotions
                new (emotionState variant, short change)[] {
                    (emotionState.pain, 16),
                    (emotionState.feelingSick, 6),
                    (emotionState.stress, 10)
                },
                //Afflicts
                new[] {
                    afflictState.tinglingThroat,
                    afflictState.runnyNose
                })
            },

            //Medium level reaction
            { reactionLevel.med, (
                //Emotions
                new (emotionState variant, short change)[] {
                    (emotionState.pain, 30),
                    (emotionState.feelingSick, 26),
                    (emotionState.stress, 35),
                    (emotionState.itchiness, 45)
                },
                //Afflicts
                new[] {
                    afflictState.runnyNose,
                    afflictState.hardToBreath,
                    afflictState.tinglingThroat
                })
            },

            //High level reaction
            { reactionLevel.high, (
                //Emotions
                new (emotionState variant, short change)[] {
                    (emotionState.pain, 64),
                    (emotionState.feelingSick, 80),
                    (emotionState.stress, 72),
                    (emotionState.itchiness, 75),
                    (emotionState.tired, 50)
                },
                //Afflicts
                new[] {
                    afflictState.runnyNose,
                    afflictState.hardToBreath,
                    afflictState.tinglingThroat,
                    afflictState.tightChest
                })
            },
        };
    }

    //
    // Processing for event stuffs
    //
    public class Events
    {
        // Generate all of todays random events
        public (Dictionary<daySection, int> chanceEvents, bool isHeavyDrinking) EvaliuateChanceEvents()
        {
            Dictionary<daySection, int> chanceEvents = new Dictionary<daySection, int>();
            bool isHeavyDrinking = false;

            for (int i = 0; i < randomnessArray.daySections.Length; i++) //For each day section
            {
                int eventNumber = UnityEngine.Random.Range(1, 1001); //Generate a random number to be the event chosen
                if (randomnessArray.daySections[i].eventChances.Length != 0) //If day section has no events skip event selection
                {
                    for (int f = 0; f < randomnessArray.daySections[i].eventChances.Length; f++) //Calculate which event was selected
                    {
                        eventNumber -= randomnessArray.daySections[i].eventChances[f];
                        if (eventNumber <= 0) {
                            chanceEvents.Add((daySection)i, f + 1); //Add the section and its event ID
                            break;
                        }
                    }
                    if (eventNumber > 0) {
                        chanceEvents.Add((daySection)i, 0); //No event was rolled
                    }
                } else {
                    chanceEvents.Add((daySection)i, 0); //No event to roll for
                }
            }

            if (chanceEvents[daySection.firstWork] == 3) { //Ensure both works have collegue down if selected
                chanceEvents[daySection.secondWork] = 3;
            }

            int drinkingEventNumber = UnityEngine.Random.Range(1, 1001);
            if (drinkingEventNumber > randomnessArray.drinkingChances[1]) { //Check for which drinking event was chosen
                isHeavyDrinking = true; //Enable heavy drinking if rolled
            }

            #if DEBUG //Debug only code to output all events selected for the day
                string concatEventDebug = "";
                for (int ced = 0; ced < chanceEvents.Count; ced++) {
                    concatEventDebug += ((daySection)ced).ToString() + " event: " + chanceEvents[(daySection)ced] + "\n";
                }
                concatEventDebug += "Heavy drinking: " + Convert.ToString(isHeavyDrinking);
                Debug.Log(concatEventDebug);
            #endif

            return (chanceEvents, isHeavyDrinking);
        }
    }


    //
    // The chances for all events to be rolled
    //
    private class randomnessArray
    {
        public static readonly (daySection section, int[] eventChances)[] daySections = //Chances are C/1000
        { //All day section random events and their chances (int[0] means no random events)
            new (daySection.dayStart,
                new[] {
                    75, //Early wake
                    22  //Late wake
                }
            ),
            new (daySection.workStartTravel,
                new[] {
                    45, //Car crash ahead
                    80, //Road closure
                    18  //Car doesn't start
                }
            ),
            new (daySection.firstWork,
                new[] {
                    30, //Homemade food
                    45, //Shop food
                    36, //Down colleague
                    5   //Work celebration
                }
            ),
            new (daySection.lunch, new int[0]),
            new (daySection.secondWork,
                new[] {
                    56, //Shop food
                    12  //Early end
                }
            ),
            new (daySection.workEndTravel,
                new[] {
                    50, //Car crash ahead
                    80, //Road closure
                    15  //Car doesn't start
                }
            ),
            new (daySection.afternoon, new int[0]),
            new (daySection.homeTravel,
                new[] {
                    55, //Car crash ahead
                    62  //Road closure
                }
            ),
            new (daySection.dayEnd, new int[0])
        };

        public static readonly int[] drinkingChances = new[] { //Chances are C/1000. This only exists because the party event is unique
            550, //Light drinking
            450  //Heavy drinking
        };
    }
}
