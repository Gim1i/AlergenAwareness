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
            short eventNumber = (short)Random.Range(1, 1001);
            for (short i = 0; i < eventChances.Length; i++) { //Cycle though all the chances
                eventNumber -= eventChances[i];
                if (eventNumber <= 0) { //If one of the reaction chances was rolled
                    Debug.Log("Rolled " + (reactionLevel)i + " reaction for " + eventID);
                    var reactionChanges = sympoms[(reactionLevel)i]; //Get all listed changes for the reaction
                    for (short e = 0; e < reactionChanges.emotions.Length; e++) //Update all emotion values
                    {
                        int currentValue = PlayerPrefs.GetInt(reactionChanges.emotions[e].variant.ToString());
                        PlayerPrefs.SetInt(reactionChanges.emotions[e].variant.ToString(), currentValue + reactionChanges.emotions[e].change);
                    }
                    for (short a = 0; a < reactionChanges.afflicts.Length; a++) //Update all emotion values
                    {
                        PlayerPrefs.SetInt(reactionChanges.afflicts.ToString(), 1); //Sets afflict to true
                    }
                    modalScript.ApplyEmotionChanges();
                    return;
                }
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

    }
}
