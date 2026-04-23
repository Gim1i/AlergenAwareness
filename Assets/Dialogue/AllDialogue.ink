//
// dayStart
//
=== Sec0 === 
#back #bedroom.day
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
\*Alarm beeping*
...
Uuurg, damn it
\*Gets up and dressed for work*
Should I prepare lunch today?
-> lunchPrepChoice

//Early wake
= Ev1
...
...
Why don't I hear my alarm?
\*Jolts out of bed and checks the clock*
Oh I woke up early *Sigh*
\*Gets dressed for work*
Should I prepare lunch today?
-> lunchPrepChoice

//Late wake
= Ev2
...
...
Why don't I hear my alarm?
\*Jolts out of bed and checks the clock*
Damn I'm late!
\*Dressed for work as fast as possible*
Theres no time to prepare lunch today
\*Rushes out the door*
-> END

//Prep Choice
= lunchPrepChoice //(-> returnTo)
* [Yes]
    \*Prepares lunch* #save #prepLunch
* [No]
- Time to head to work then
-> END




//
// workStartTravel
//
=== Sec1 === 
#back #driving.day
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
...
-> END

//Car crash ahead
= Ev1
...
...
Why am I queueing for so long?
If this keeps up I might not make it to work on time
...
Finaly moving again
\*Drives by a car crash*
Ahh that makes sense
-> END

//Road closure
= Ev2
...
That road is closed now?
I guess I'm going to have to switch route this week
Hopefully I can still make it to work on time
-> END

//Car doesn't start
= Ev3
\*Tries to start car*
\*Tries again*
...
Damn thing wont start
\*Sigh* I've got to call Tyler
\*Ring Ring Rin-*
Hi Tyler, I'm probably not going to be able to be in this morning as my car isn't starting
Tyler: What?! Thats not good
Tyler: Do you think you will be able to make get here by lunch time?
Yea I can be there by lunch. Just have to give the car to the garage
Tyler: Good to hear! I'll see you after the lunch break then
\*Click*
Luckly the issue was quite minor so the car will be fixed by this evening #save #skipFirstWork
-> END //Skips first work




//
// firstWork
//
=== Sec2 === 
#back #officeJob.day
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
Hi Tyler. Anything important I should know?
Tyler: Nope, just a regular day!
Good to hear! I'll get to working then
\*Tyler give a thumbs up*
-> END

//Homemade food
= Ev1
~ temp randomHomeMadeFood = ""
~ randomHomeMadeFood = "{~brownies|sponge cake|chocolate cake|cupcakes|cookies}"

Hi Tyler. Whats happening over there?
Tyler: Someone brought in some {randomHomeMadeFood}
Tyler: It's really good, you should have some!
Should I have some?
* [Yes]
    Will do! #react #7.0
* [No]
    I'll pass on that, but thanks for offering
- -> END

//Shop food
= Ev2
~ temp randomBroughtFood1 = ""
~ randomBroughtFood1 = "{~cookies|cookies|doughnuts|doughnuts|muffins|muffins|cake}"

Hi Tyler. Whats happening over there?
Tyler: Someone brought some {randomBroughtFood1} on their way to work
Tyler: It's free so I recomend grabing some!
Should I have some?
* [Yes]
    Will do! #react #8.0
* [No]
    I'll pass on that, but thanks for offering
- -> END

//Down colleague
= Ev3
Hi Tyler. Anything important I should know?
Tyler: Someone called in sick earlier so we're going to be short-staffed today
You know wether they'll be able to be here tommorow?
Tyler: Luckly they said they should be here tommorow
Good to hear! Give them a get well soon from me next time you speak to them
Tyler: Will do!
\*Walks of to start work*
-> END

//Work celebration
= Ev4
Hi Tyler. Why is the office is looking fun today?
Tyler: Upper managment organised a party to celebrate us completing the project before the deadline
Tyler: Theres free food and drinks if you want any
Should I get some free food?
* [Yes]
    ~ temp randomWorkPizzaChoice = ""
    ~ randomWorkPizzaChoice = "{~pepperoni|margherita}"
    
    Will do! which do you recomend?
    Tyler: The pepperoni was quite good when I tried it, though margherita is always a solid choice
    Yea {randomWorkPizzaChoice} sounds like a good choice. Thanks!
    Tyler: No problem! *Thumbs up*
    \*Walks over to the free food table and grabs a slice of {randomWorkPizzaChoice} pizza*
    ...
    Huh, thats some decent pizza. Glad I had some #react #9.0
    Anyway time to start work"
* [No]
    I'll pass on that, but thanks for offering
- -> END




//
// lunch
//
=== Sec3 === 
//No background as it changes after chosing a lunch option
#get #prepLunch
VAR prepLunch = false
-> Ev0

//Regular process
= Ev0
~ temp randomLunchOrderNumber = 0
~ randomLunchOrderNumber = RANDOM(100, 999)

* [Local coffee shop]
    \*Door bell dings* #back #coffeeShop.day
    Barista 1: What can I do for you today?
    - - (lcsJumpBack)
    What should I choose?
    * * [Coffee and a Sandwitch]
        Can I get a latte and a {~Ham & Cheese Sandwitch|Ham & Cheese Sandwitch|Ham & Cheese Sandwitch|Ham Sandwitch|Tuna Salad Sandwitch}
        Barista 1: You can! Would you like anything else?
        No thanks, thats all I want
        Barista 1: Ok, the coffee will be ready in 3 minutes and your number is {randomLunchOrderNumber}
        Thanks!
        ...
        ...
        Barista 2: {randomLunchOrderNumber}!,
        Thats me, thanks
        Barista 2: Have a good day!
        You too!
    * * [Tea and Cake]
        ~ temp randomCake = ""
        ~ randomCake = "{~Chocolate Cake|Sponge Cake|Cheese Cake}"
        Can I get a latte and... What cake options do you have?
        Barista 1: We have chocolate cake, sponge cake and cheese cake right now
        Barista 1: Which would you like?
        I'll take a slice of {randomCake} please.
        Barista 1: I'll go get that for you now then
        ...
        Barista 1: Heres your {randomCake} and your latte will be ready in 2 minutes
        Barista 1: Your order number is {randomLunchOrderNumber}
        Thanks, have a good day!
        ...
        ...
        Barista 2: {randomLunchOrderNumber}!
        Thats me, thanks
        Barista 2: Enjoy your lunch!
        Thank you!
    * * [Just a sandwitch]
        Can I get a {~Ham & Cheese Sandwitch|Ham & Cheese Sandwitch|Ham & Cheese Sandwitch|Ham Sandwitch|Tuna Salad Sandwitch}
        Barista 1: You can! Would you like anything else?
        No thanks, thats all I want
        Barista 1: Heres your sandwitch, have a good day!
        Thanks, you too!
    * * [Ask about allergies]
        I have an allergy to nuts, how are nuts handled here?
        Barista 1: We're a nut free shop, so you wont have to worry about that here!
        Awsome, thats great to hear!
        Barista 1: So what can I get for you then?
        -> lcsJumpBack
        
* [Jenns]
    Jenns worker: Hello, welcome to Jenns! What can I get for you today? #back #jenns.day
    - - (jJumpBack)
    What should I choose?
    * * [Sausage roll and a coffee]
        Can I get a Sausage Roll and a latte
        Jenns worker: You can although the latte will take a minute to be ready
        Ok. I don't mind that
        Jenns worker: I'll grab those for you now then
        ...
        ...
        Jenns worker: Heres your latte and Sausage Roll #react #0.0
    * * [Baguete and a coffee]
        ~ temp randomBaguetteChoice = ""
        ~ randomBaguetteChoice = "{~spicy chicken baguette|ham and cheese baguette|chicken and mayonase baguette|chicken and mayonase baguette|chicken salad baguette|ham baguette|bacon baguette}"
        
        Can I get a {randomBaguetteChoice} and a latte
        Jenns worker: Yes, I'll grab those for you now
        ...
        ...
        Jenns worker: Heres your latte and {randomBaguetteChoice} #react #0.1
    * * [Sausage roll and a Cookie]
        ~ temp randomCookieChoice = ""
        ~ randomCookieChoice = "{~Double Chocolate|Milk Chocolate|White Chocolate}"
        
        Can I get a Sausage Roll and...
        What cookie options do you have?
        Jenns worker: We have Double Chocolate, Milk Chocolate and White Chocolate
        I'll take a {randomCookieChoice} too
        Jenns worker: So a Sausage Roll and a {randomCookieChoice}?
        Yep, thats right
        Jenns worker: Ok, I'll grab those for you then
        Jenns worker: Heres your {randomCookieChoice} and Sausage Roll #react #0.2
    * * [Ask about allergies]
        I have an allergy to nuts, how are nuts handled here?
        Jenns worker: We are unable to garantee there are no nuts in our products due to us using nuts in our kitchen
        Jenns worker: But we do go though rigorous procedures to prevent cross-contamination in all of our food
        Jenns worker: We have an allergen table you could look though that I could get if you'd want
        Jenns worker: Would you like to see it?
        * * * Yes
            <> please #open #allergen table.jenns
        * * * [No]
            No thanks
        Jenns worker: So, what can I get for you today then?
        - - - -> jJumpBack
    - - Jenns worker: Have a good day
    You too!
    
* [Salad deli bar]
    Deli cashier: Welcome to Sarah's Salad Deli! #back #saladDeli.day
    Deli cashier: What can I do for you?
    - - (sdbJumpBack)
    * * [Ask for table]
        I'd like a table for one please
        Deli cashier: Perfect, we have a table over here for you!
        Deli cashier: Please follow me
        \*You follow the cashier*
        ...
        Deli cashier: This table right here please
        Deli cashier: Heres the menu and someone will come around to get your order in approximately 5 minutes
        Deli cashier: Any questions?
        Nope, I'm good for now
        Deli cashier: Have a good meal then!
        ...
        ...
        Deli server: Hello, what can I get for you today?
        * * * [Deli bar]
            I'll have the deli bar option please
            Deli server 1: Sure, I'll go grab your bowl then
            ...
            Deli server 1: Here's your bowl, feel free to go grab anything you want from the counters over there!
            Deli server 1: Is that all?
            Yes. Thank you!
            Deli server 1: Your welcome!
            ... 20 minutes later ... #react #1.0
            This deli bar hasn't been that bad, I'll definitely consider coming again
        * * * [Lasagne with chips]
            Can I get the Lasagne please?",
            Deli server 1: With chips or salad?
            Chips please
            Deli server 1: Ok, do you want anything else?
            No thank you, thats all I want for now
            Deli server 1: I'll let the kitchen know then. It should arive in about 30 minutes
            Thanks.
            ...
            Deli server 2: Lasagne with chips?
            Yes thats mine. Thanks
            Hmm, this looks good!
            ... #react #1.1
            That was some good lasagne, I should come here again!
        * * * [Mac & Cheese]
            Can I get the Mac & Cheese please?
            Deli server 1: You can. Do you want anything else with that?
            Nope, thats all I want
            Deli server 1: I'll let the kitchen know then. It should arive in about 30 minutes
            Thank you, have a good day!
            Deli server 1: You too!
            ...
            Deli server 2: The Mac & Cheese?
            Yep thats mine. Thanks
            Hmm, this looks good!
            ... #react #1.2
            That Mac & Cheese was fairly good, I should consider coming here again!
        - - - \*Gets up and leaves*
        Deli cashier: Thank you for visiting today, and I hope to see you again soon!
    * * [Ask about allergies]
        I have an allergy to nuts, how are nuts handled here?
        Deli cashier: We are unable to garantee there are no nuts in our products due to us using nuts in our kitchen
        Deli cashier: And, while we do have procedures in place to prevent cross-contaimation in our kitchen...
        Deli cashier: we can't garentee any security with the salad bar
        Deli cashier: Would you like to see our allergen table?
        * * * Yes
            <> please #open #allergen table.salad deli
        * * * [No]
            No thanks
        Deli cashier: What can I help you with today then?
        - - - -> sdbJumpBack

* { prepLunch } [Have prepared Lunch instead] #back #officeBreakRoom.day
    ...
    ...
    {~*Under breath* Needs more mayo|mmmm|...|...|...|...Hmm, kinda plain}
- ...
Time to get back working
-> END




//
// secondWork
//
=== Sec4 === 
#back #officeJob.day
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
...
-> END

//Shop food
= Ev1
~ temp randomBroughtFood2 = ""
~ randomBroughtFood2 = "{~cookies|cookies|doughnuts|doughnuts|muffins|muffins|cake}"

Hi Tyler. Whats happening over there?
Tyler: Someone brought some {randomBroughtFood2} on their way to work
Tyler: It's free so I recomend grabing some!
Should I have some?
* [Yes]
    Will do! #react #8.0
* [No]
    I'll pass on that, but thanks for offering
- -> END

//Early end due to very bad weather
= Ev2
Time to get back working
...
...
...
\*Looks out the window* Uhhh, that weather's looking real bad
Hey Tyler, any chance we could finish early today?
Getting home might become impossible for some if this weather keeps up
Tyler: *Looks outside* Oh yea thats really bad, I might not make it home myself
Tyler: Ok we'll finish early today, feel free to go and I'll let the rest of the office know shortly
Thanks Tyler!
Tyler: No problem!
-> END

//Down colleague (NOT RANDOMLY GENERATED)
= Ev3
Tyler: Just a reminder, we're short-staffed so I'd appreciate if you got on with work promptly
Oh yea, I forgot about that
I'll get on with it immediately then
\*Tyler give a thumbs up*
-> END




//
// workEndTravel
//
=== Sec5 === 
#back #driving.day
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
...
-> END

//Car crash ahead
= Ev1
...
...
Why am I queueing for so long?
If this keeps up I might not have much time this evening
...
Finaly moving again,
\*Drives by a car crash*
Oh, that explains the delay
-> END

//Road closure
= Ev2
...
That road is closed now?
I guess I'm going to have to switch route this week
-> END

//Car doesn't start
= Ev3
...
\*Tries to start car*
\*Tries again*
Damn thing wont start
\*Sigh* I guess I'm not doing anything tonight
Hopefully I can still make it into work tommorow
...
Luckly the issue was quite minor so the car will be fixed by tommorow morning #save #afternoonDriveDelay
-> END




//
// afternoon
//
=== Sec6 ===
#back #livingRoom.day
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
    I don't really fancy going out tonight
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
    I should head to the gym, I've been slacking on excercie lately
    \*Heads to the gym* #back #gym.day
    { shuffle:
      - Hmm, I should go for a new personal record
        { shuffle:
          - ...
            Damn didn't make it. Maybe next time
          - ...
            So close. I can definitely get there soon
          - ...
            Yes! Finaly got that new record
        }
      - It's really quiet today, I wonder why?
        { shuffle:
          - ...
            ...
          - ...
            ...
          - ...
            It's surprisingly nice when its quiet here
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
        Waterside cashier: Right here please. A waiter will come take your order shortly.
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
        This is looking really good
        ...
        ...
        {~That was a good meal... I should order it again next time|Even though that was good. I should order something different next time|Not too bad|Finished!}
        Time to go home then
        Waterside cashier: Thank you for visiting The Waterside. We hope to see you again soon!
        {~I definitely will!|Will do!|You too!}
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
    } #back #pub.afternoon
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
        }
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
    I'll just cook at home, no need for anything fancy today
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
        \*Opens lid* This is looking good, time to dig in! #react #6.0
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
        That was some good {randomChineseChoice}, I should order that again. #react #7.0
        
- Guess its time for bed #save #skipHomeTravel
-> END

= heavyDrink
Hmmm... I'm hungry
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
    }  #react #4.0
    \*Walks off* #back #resturaunt.evening
* [Don't get snacks]
    Uh, nevermind
    Bartender: Ok, have a good night!
    Thanks! #back #resturaunt.evening

- ... 1 hour later ...
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
This is really fun, though I would love a snack
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
    } #react #3.0
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
        
- ... 1 hour later ...
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




//
// homeTravel
//
=== Sec7 ===
#back #driving.evening
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
...
-> END

//Car crash ahead
= Ev1
...
...
Taxi diver: This queue is taking a long time to get though?
...
Taxi diver: Finaly though it
\*Drives by a car crash* #save #lateHomeArival
-> END

//Road closure
= Ev2
...
Taxi diver: That road is closed now?
Taxi diver: Damn, I'll have to take a different route #save #lateHomeArival
-> END




//
// dayEnd
//
=== Sec8 ===
#back #bedroom.evening
-> Ev0 //Here to prevent errors

//Regular process
= Ev0
{~It's been along day. Time for sleep...|...|...} #endday #0
-> END