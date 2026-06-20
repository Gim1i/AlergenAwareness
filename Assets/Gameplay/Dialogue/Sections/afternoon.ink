=== afternoon ===
#get #afternoonDriveDelay
#get #heavyDrinking
VAR afternoonDriveDelay = false
VAR heavyDrinking = false
-> Ev0

//Regular process
= Ev0
{ afternoonDriveDelay:
    What should I do with my remaining afternoon/evening?
- else:
    What should I do this afternoon/evening?
}

* [Relax at home]
    I don't really fancy going out tonight #prefChange #stress.1.-10 #prefChange #bored.1.5 #prefChange #happy.1.3
    { shuffle:
      - Actualy didn't a new show come out on fetnlix? I should watch that tonight
        ...
        ...
        This new show is fairly good but I'm starting to want dinner
      - I wonder if there's anything on TV today
        Finding Mira is on!? I never got to watch it so this is lucky
        ...
        That was a good movie... *Checks clock* Oh it's dinner time
      - I should play that new game I brought
        ...
        This isn't too bad
        ... I should get dinner at soon
    }
    -> HomeDinner
* [Go to the gym]
    I should head to the gym, I've been slacking on excercise lately #prefChange #stress.1.-9 #prefChange #bored.1.-11 #prefChange #happy.1.5
    \*Heads to the gym* #back #gym.day
    { shuffle:
      - Hmm, I should go for a new personal record
        { shuffle:
          - ...
            Damn didn't make it. Maybe next time
          - ...
            So close. I can definitely get there soon
          - ...
            Yes! Finaly got that new record #prefChange #happy.1.3
        }
      - It's really quiet today, I wonder why?
        { shuffle:
          - ...
            ...
          - ...
            ...
          - ...
            It's surprisingly nice when its quiet here #prefChange #happy.1.1
        }
      - Wow its busy
        ...
        ...
      - ...
        ...
        ...
    }
    -> HomeDinner
* { !afternoonDriveDelay } [Go to a resturaunt]
    Waterside cashier: Hello and welcome to The Waterside, what can I help you with today? #back #resturaunt.afternoon
    - - (rJumpBack)
    * * [Ask for table]
        ~ temp randomDrink = ""
        ~ randomDrink = "{~water|water|bepis|11up}"

        Can I get a table for 1 please?
        Waterside cashier: Sure, follow me
        ...
        Waterside cashier: Right here please. A waiter will come take your order shortly. #prefChange #bored.1.-12 #prefChange #stress.1.-4
        ...
        Waterside waiter: Hello! Are you ready to order yet?
        { shuffle:
          - { shuffle:
              - I'll need a minute to make a choice
                Waterside waiter: Ok, I'll let you to think and come back shortly
                ...
                Waterside waiter: Are you ready to order?
              - 
            }
            Yep
            Waterside waiter: Shall we start with a drink then?
            I'll take a {randomDrink} please
            Waterside waiter: And what main can I get for you?
          - I'll order the drink now and think about the meal
            Waterside waiter: Which drink do you want then?
            A glass of {randomDrink} please
            Waterside waiter: I'll go let the kitchen know then
            ...
            Waterside waiter: Are you ready to order?
            Yes
            Waterside waiter: Then what main would you like to order?
        }
        What should I order?
        ~ temp dinnerChoice = ""
        * * * [Chicken Pesto]
            I'll take the Chicken Pesto please #react #2.0
            ~ dinnerChoice = "Chicken Pesto"
        * * * [Lemon Sea Bass]
            ~ temp randomBassSide = ""
            ~ randomBassSide = "{~peas|chips}"
            
            I'll take the Lemon Sea Bass please
            Waterside waiter: Do you want peas or chips with that?
            I'll have {randomBassSide} please #react #2.1
            ~ dinnerChoice = "Lemon Sea Bass with {randomBassSide}"
        * * * [Waterside Specialty Burger]
            ~ temp randomBurgerSide = ""
            ~ randomBurgerSide = "{~onion rings|chips}"
            
            Can I get the Waterside Specialty Burger?
            Waterside waiter: Yes, is that with onion rings or chips?
            Hmm, {randomBurgerSide} sounds good #react #2.2
            ~ dinnerChoice = "Waterside Specialty Burger with {randomBurgerSide}"
        - - - Waterside waiter: Got it! Is that everything?
        Yep
        Waterside waiter: Ok, I'll go let the kitchen know and grab your drink
        ...
        Waterside waiter: Here's your {randomDrink}. The {dinnerChoice} should be out shortly
        Ok, thanks!
        ...
        Waterside waiter: The {dinnerChoice}?
        Yep that's mine
        Waterside waiter: Have a good meal!
        Thank you!
        This is looking really good #prefChange #happy.1.14
        ...
        ...
        {~That was a good meal... I should order it again next time|Even though that was good. I should order something different next time|Not too bad|Finished!}
        Time to go home then
        Waterside cashier: Thank you for visiting The Waterside. We hope to see you again soon!
        {~I definitely will!|Will do!|You too!}
        Hmm, I feel way too tired to drive
        I should call a taxi to get home
        \*Calls a taxi to go home*
        ...
        Taxi diver: Where to?
        \*Home address* please
    * * [Ask about allergies]
        I'm allergic to nuts, how are they handled here?
        Waterside cashier: While we do employ a rigorous anti-contaimination policy...
        Waterside cashier: We're unable to ensure that cross contaimination wont happen
        Waterside cashier: We do have an allergen table, if you would like to see it
        * * * Yes
            <> please #open #allergen table.resturaunt
        * * * [No]
            No thanks
        - - - Waterside cashier: What can I help you with today then?
        -> rJumpBack
* { !afternoonDriveDelay } [Go to a party]
    ~ temp wasInvited = false
    
    { shuffle:
      - Going to a party sounds like fun
        I'll head out in 15 minutes
        ...
      - \*Ring-Ring-Ring Ring-R*
        Who's calling me?
        Co-worker: Heya, a couple of us are going to party. Wanna tag along?
        Sure! I was thinking about going to a party anyway!
        Co-worker: Awsome! We're planning to go to The LeadWheel in about an hour, so I'll see you there
        Yep, see you there!
        \*Click*
        ...
        I should head out if I want to make it
        ~ wasInvited = true
    } #back #pub.afternoon #prefChange #bored.1.-10 #prefChange #stress.1.-6
    \*Leaves to go to the party*
    ...
    { wasInvited:
        Where did they say we were meeting?
        Co-worker: Over here!
        Ah. *Strides over*
        Co-worker: glad to see you made it.
        { shuffle:
          - Co-worker: You were the last to arive so we'll head in right now
          - Co-worker: We're still waiting on one more person to arive
            ...
            Co-worker: Ah, there they are. I'll go grab them and then we'll head in
        } #prefChange #happy.1.6
    }
    ... 30 minutes later ...
    { heavyDrinking:
      -> heavyDrink
      - else:
      -> lightDrink
    }
    

- -> END

= HomeDinner
What should I for dinner? #back #livingRoom.afternoon
* [Cook]
    I'll just cook at home, no need for anything fancy today #prefChange #stress.1.-5 #prefChange #bored.1.2 #prefChange #happy.1.3
    {~Spaghetti bolognaise sounds good|Chicken strips would be nice and easy to make|I fancy chili con carne tonight... Wait did I get beef mince?|Chicken soup sounds like a good idea tonight|Mac & cheese should be easy to make|Bacon & mushroom risotto would be a good choice, the bacon needs to be using anyway|I should try and make that ghormeh sabzi recipie I found earlier}
    ...
    {~I should use less salt next time|...|...|Huh, that was really good... I should write that down|\*Sigh* I still have to clean up}
* [Order a takeaway]
    Cooking sounds like too much of a hastle right now, I'll just order a takeaway instead
    What should I order?
    * * [Pizza]
        ~ temp randomPizzaChoice = ""
        ~ randomPizzaChoice = "{~Margerehta|Margerehta|Peperoni|Peperoni|Ham and Pineapple|Ham and Pineapple|Sausage and Chorizo}"

        I'll just order a pizza
        \*Ring Ring R-*
        Phone attendent: Hello, this is Mahjong pizza. What can I help you with today?
        I'd like to order a {~10"|10"|12"|12"|12"|14"} {randomPizzaChoice} pizza please
        Phone attendent: Would you like anything else with that?
        No thanks
        Phone attendent: Ok, do you want it delivered or will you pick it up?
        Deliver it please
        Phone attendent: Ok, that will arive in approximately 30 minutes
        Ok, have a good day!
        Phone attendent: Thank you, you too!
        \*Click*
        Guess I should find something to do for 30 minues
        ...
        ...
        ...
        \*Ding-Dong* Oh thats probably the pizza
        Mahjong driver: {randomPizzaChoice} pizza right?
        Yep thats mine
        Mahjong driver: Here you go then
        Thanks, have a good night!
        Mahjong driver: Thank you, you too!
        \*Opens lid* This is looking good, time to dig in! #react #5.0 #prefChange #stress.1.-7 #prefChange #happy.1.8
        ...
        That was a good pizza
    * * [Chinese]
        ~ temp randomChineseChoice = ""
        ~ randomChineseChoice = "{~Crispy Duck with Orange Sauce|Sweet & Sour|Spicy Szechuan}"

        Chinese sounds good to me
        \*Ring Ring Ri-*
        Phone attendent: Hello, this is Serpent City Chinese. What can I help you with today?
        I'd like to order a {randomChineseChoice} please
        Phone attendent: Any side dishes?
        No thanks
        Phone attendent: Is this a delivery or pickup?
        Delivery please
        Phone attendent: Ok, it should arive in about 24 minutes
        Thanks, have a good day!
        Phone attendent: You too!
        \*Click*
        24 minutes huh...
        ...
        ...
        \*Ding-Dong* Chinese is here!
        Serpent City driver: A {randomChineseChoice}?
        Yea thats what I ordered
        Serpent City driver: Here you go
        Thank you!
        Serpent City driver: No problem!
        ...
        That was some good {randomChineseChoice}, I should order that again. #react #6.0 #prefChange #stress.1.-7 #prefChange #happy.1.8
        
- Guess its time for bed #save #skipHomeTravel
-> END

= heavyDrink
Hmmm... I'm hungry #prefChange #stress.1.-15 #prefChange #bored.1.-13 #prefChange #happy.1.14
Where's the bar?
\*Walks over to the bar*
Bartender: What can I help you with?
* [Get snacks]
    Snacks please
    Bartender: We have crisps and packed nuts. Or you could order chips if those don't work
    { shuffle:
      - Crisps please
        Bartender: Can do, what kind do you want?
        {~Salty|Cheese and onion}
        Bartender: Ok, here you go
      - Chips please
        Bartender: Do you want cheese on those?
        {~Yes|Nah}
        Bartender: Ok those should be out in 5 minutes, please wait there
        ...
        Bartender: Here's your chips
    }  #react #4.0 #prefChange #happy.1.14
    \*Walks off* #back #resturaunt.evening
* [Don't get snacks]
    Uh, nevermind
    Bartender: Ok, have a good night!
    Thanks! #back #resturaunt.evening

- ... 1 hour later ... #prefChange #tired.1.20
...
Co-worker: Heya man, you seem very drunk
Yea...
Co-worker: Should I call you a taxi?
Yes please
\*Co-worker calls a taxi to go home*
...
Taxi diver: Where to?
Co-worker: Uh I'm unsure
...
Huh? Oh, *Home address*
-> END

= lightDrink
This is really fun, though I would love a snack #prefChange #stress.1.-8 #prefChange #bored.1.-7 #prefChange #happy.1.5
Maybe the bar has some?
\*Walks over to the bar*
Bartender: What can I help you with?
* [Get snacks]
    What snacks have you got here?
    - - (pJumpBack)
    Bartender: We have crisps and packed nuts. Or you could order chips if those don't work
    I'll <>
    { shuffle:
      - take crisps please
        Bartender: Can do, what kind do you want?
        {~Ready salted|Cheese and onion}
        Bartender: Ok, here you go
      - order chips please
        Bartender: Do you want cheese on those?
        {~Yes please|Nope}
        Bartender: Ok those should be out in 5 minutes, please wait there
        ...
        Bartender: Here's your chips
    } #react #3.0 #prefChange #happy.1.10
    Thanks! #back #resturaunt.evening
* [Don't get snacks]
    On second thought, nevermind
    Bartender: Ok, have a good night!
    Thanks! #back #resturaunt.evening
* [Ask about allergies]
    Do you use nuts here? I'm allergic
    Bartender: We have packed nuts available for purchase, although they do remain packed until someone buys them
    Bartender: Do you still want that snack?
    * * [Yes]
        Sure, what snacks have you got?
        -> pJumpBack
    * * [No]
        No thanks #back #resturaunt.evening
        
- ... 1 hour later ... #prefChange #tired.1.17
Hey, it's getting kinda late I'm going to leave
Co-worker: No problem. It's been fun
Yep it has been. Be sure to let me know if you do this again, I'll definitely say yes
Co-worker: I will do, bye!
See you later!
\*Calls a taxi to go home*
...
Taxi diver: Where to?
\*Home address* please
-> END