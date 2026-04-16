using UnityEngine;

public enum modalVariant { happy, sad, angry, pain, tired, stress, bored, soreThroat }
public enum emotionState { happy, sad, angry, pain, tired, stress, bored } //Possible emotion modals
public enum afflictState { soreThroat } //Possible afflict modals
public enum playerStatLevel { none, low, medium, high }
public enum daySection { dayStart, workStartTravel, firstWork, lunch, secondWork, workEndTravel, afternoon, homeTravel, dayEnd }
public enum foodReactionChance { jenns, saladDeli, resturaunt, lightDrinking, heavyDrinking, pizza, chinese, broughtInHomeFood, broughtInShopFood, workCelebration, none }

//
// Player's hidden stats
//
public static class playerStats
{
    private static bool isEveningDriveDelayed = false; //Stores if an evening drive was delayed (effects datStart event chances)
    public static void EveningDriveDelayed() { isEveningDriveDelayed = true; }
    public static void ResetDriveDelayBool() { isEveningDriveDelayed = false; }
    public static bool GetEveningDriveDelayedState() { return isEveningDriveDelayed; }

    private static (emotionState emotion, playerStatLevel level)[] emotions = { //All of the players current emotions
        (emotionState.happy,   playerStatLevel.none),
        (emotionState.sad,     playerStatLevel.none),
        (emotionState.angry,   playerStatLevel.none), 
        (emotionState.pain,    playerStatLevel.none),
        (emotionState.tired,   playerStatLevel.none),
        (emotionState.stress,  playerStatLevel.none),
        (emotionState.bored,   playerStatLevel.none)
    };

    private static (afflictState afflict, bool isActive)[] afflicts = {  //All of the players potencial afflicts
        (afflictState.soreThroat, false)
    };

    public static void AlterEmotionLevel(emotionState emotion, int scale) { //Alter an emotion up or down by scale
        for (int i = 0; i < emotions.Length; i++) { //Find the emotion
            if (emotions[i].emotion == emotion) {
                int newLevel = ((int)emotions[i].level + scale); //Make enum int and add scale
                if (newLevel > 3) { newLevel = 3; } //Constrict number to an acceptable range
                else if (newLevel < 0) { newLevel = 0; }
                emotions[i].level = (playerStatLevel)newLevel; //Save as new level
                return;
            }
        }
    }

    public static void UpdateAfflictBool(afflictState afflict, bool newState) { //Toggle an afflict's state
        for (int i = 0; i < afflicts.Length; i++) { //Find the afflict
            if (afflicts[i].afflict == afflict) {
                afflicts[i].isActive = newState; //Set the new bool state
                return;
            }
        }
    }
}

//
// All the stats surounding randomness. Everything is readonly to prevent issues
//
public static class randomnessArray 
{
    private static readonly int[] drivingEvents = new[] { //Saves writing out driving events multiple times
        5, //Car crash ahead
        5, //Road closure
        5  //Car doesn't start
    };

    public static readonly (daySection section, int[] eventChances)[] daySections = //Chances are C/1000
        { //All day section random events and their chances (int[0] means no random events)
        new (daySection.dayStart,
            new[] {
                5, //Early wake
                5  //Late wake
            }
        ),
        new (daySection.workStartTravel, drivingEvents),
        new (daySection.firstWork,
            new[] {
                5, //Homemade food
                5, //Shop food
                5, //Down colleague
                5  //Work celebration
            }
        ),
        new (daySection.lunch, new int[0]),
        new (daySection.secondWork,
            new[] {
                5, //Shop food
                5  //Early end
            }
        ),
        new (daySection.workEndTravel, drivingEvents),
        new (daySection.afternoon, new int[0]),
        new (daySection.homeTravel, drivingEvents),
        new (daySection.dayEnd, new int[0])
    };

    public static readonly int[] drinkingChances = new[] { //Chances are C/1000. This only exists because the party event is unique
        550, //Light drinking
        450  //Heavy drinking
    };

    public static readonly (foodReactionChance source, int[][] chance)[] foodReactionChances = //Chances are C/1000
    { //All potencial raction sources with their choice's chances for each reaction level and highest possible reaction
        (foodReactionChance.jenns, new[] {
            new[]{ 215          }, //Sausage roll and a coffee
            new[]{ 170          }, //Baguete and a coffee
            new[]{ 325, 10      }  //Sausage roll and a cookie
        }), 
        (foodReactionChance.saladDeli, new[] {
            new[]{ 900, 75      }, //Deli bar
            new[]{ 275, 15      }, //Lasagne with chips
            new[]{ 250, 10      }  //Mac & Cheese
        }),
        (foodReactionChance.resturaunt, new[] {
            new[]{ 300, 30      }, //
            new[]{ 300, 30      }, //
            new[]{ 300, 30      }  //
        }),
        (foodReactionChance.lightDrinking,     new[] {new[]{ 320          }}),
        (foodReactionChance.heavyDrinking,     new[] {new[]{ 870, 280, 12 }}),
        (foodReactionChance.pizza,             new[] {new[]{ 150          }}),
        (foodReactionChance.chinese,           new[] {new[]{ 950, 125     }}),
        (foodReactionChance.broughtInHomeFood, new[] {new[]{ 120          }}),
        (foodReactionChance.broughtInShopFood, new[] {new[]{ 300          }}),
        (foodReactionChance.workCelebration,   new[] {new[]{ 720, 30      }})
    };
}

//
// All the stats surounding randomness. Everything is readonly to prevent issues
//
public static class impactArray //IN PROGRESS (will store modal data).   UPDATE: likely depreciated as ink can handle this better
{
    public static readonly (daySection section, (string rEvent, int chance)[] events)[] daySections = //Chances are C/1000
        { //All day section random events and their chances
        new (daySection.dayStart,
            new[] {
                ("Early wake", 5),
                ("Late wake", 5)
            }
        ),
        new (daySection.workStartTravel,
            new[] {
                ("Car crash ahead", 5),
                ("Road closure", 5),
                ("Car doesn't start", 5)
            }
        ),
        new (daySection.firstWork,
            new[] {
                ("Homemade food", 5),
                ("Shop food", 5),
                ("Down colleague", 5),
                ("Work celebration", 5)
            }
        ),
        new (daySection.lunch, new (string rEvent, int chance)[0]),
        new (daySection.secondWork,
            new[] {
                ("Shop food", 5),
                ("Early end", 5)
            }
        ),
        new (daySection.workEndTravel,
            new[] {
                ("Car crash ahead", 5),
                ("New road closure", 5),
                ("Car doesn't start", 5)
            }
        ),
        new (daySection.afternoon, new (string rEvent, int chance)[0]),
        new (daySection.dayEnd, new (string rEvent, int chance)[0]),
    };
}

//
// Classes to be used in multiple scripts (single script classes are stored in their respective script)
//
[System.Serializable]
public class modalInformation //Simpler store for modal information (didnt wanna use a 3 value tuple across the whole game)
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private modalVariant variant;
    [SerializeField] private bool isEmotion;
    [SerializeField] private playerStatLevel level;

    public modalInformation(modalVariant variant, bool isEmotion, playerStatLevel level) { //Initalise with values
        this.variant = variant;
        this.isEmotion = isEmotion;
        this.level = level;
    }
    public void addSprite(Sprite sprite) {
        this.sprite = sprite;
    }

    public bool compaireWithoutSprite(modalInformation var1) { //Custom comparison script to account for sprites being optional in this class
        if (var1.variant == this.variant && var1.isEmotion == this.isEmotion && var1.level == this.level) {
            return true;
        }
        return false;
    }

    public Sprite getSprite() {return sprite; }
    public modalVariant getVariant() { return variant; }
    public bool getIsEmotion() { return isEmotion; }
    public playerStatLevel getLevel() { return level; }
}