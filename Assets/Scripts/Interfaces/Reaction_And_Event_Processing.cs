using System.Collections.Generic;
using UnityEngine;

public class Reaction_And_Event_Processing : MonoBehaviour
{
    enum reactionLevel { low, med, high }

    //
    // Do a random roll to determine if an reaction happens during an event or not
    //
    public void RollEventReaction(foodReactionSource eventID, int subID) {
        short[] eventChances = eventReactionChances[eventID][subID];
        short eventNumber = (short)Random.Range(1, 1001);
        for (short i = 0; i < eventChances.Length; i++) {
            eventNumber -= eventChances[i];
            if (eventNumber <= 0) {
                Debug.Log("");
                GetAndApplyReaction((reactionLevel)i);
                break;
            }
        }
    }

    private void GetAndApplyReaction(reactionLevel level) {
        
    }

    //
    // All the stats surounding randomness. Everything is readonly to prevent issues. Chances are C/1000
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
    
    private class reactions
    {
        //
        // All symptoms from each level of reaction
        //
        public readonly (emotionState variant, playerStatLevel level)[][] emotionSympoms = new[] {
            //Low level reaction
            new[] {
                (emotionState.pain, playerStatLevel.low),
                (emotionState.feelingSick, playerStatLevel.low),
                (emotionState.stress, playerStatLevel.low)
            },
            //Medium level reaction
            new[] {
                (emotionState.pain, playerStatLevel.low),
                (emotionState.feelingSick, playerStatLevel.low),
                (emotionState.stress, playerStatLevel.low),
                (emotionState.itchiness, playerStatLevel.medium)
            },
            //High level reaction
            new[] {
                (emotionState.pain, playerStatLevel.medium),
                (emotionState.feelingSick, playerStatLevel.medium),
                (emotionState.stress, playerStatLevel.medium),
                (emotionState.itchiness, playerStatLevel.medium),
                (emotionState.tired, playerStatLevel.medium)
            }
        };

        public readonly afflictState[][] afflictSympoms = new[] {
            //Low level reaction
            new[] {
                afflictState.tinglingThroat,
                afflictState.runnyNose
            },
            //Medium level reaction
            new[] {
                afflictState.runnyNose,
                afflictState.hardToBreath,
                afflictState.tinglingThroat
            },
            //High level reaction
            new[] {
                afflictState.runnyNose,
                afflictState.hardToBreath,
                afflictState.tinglingThroat,
                afflictState.tightChest,
                afflictState.sick
            }
        };
    }
}
