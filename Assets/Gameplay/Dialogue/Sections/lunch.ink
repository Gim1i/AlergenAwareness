=== lunch === 
#get #prepLunch
VAR prepLunch = false
-> Ev0

//Regular process
= Ev0
~ temp randomLunchOrderNumber = 0
~ randomLunchOrderNumber = RANDOM(100, 999)

* [Local coffee shop]
    \*Door bell dings* #back #coffeeShop.day #prefChange #bored.1.-10 #prefChange #stress.1.-12
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
        You too! #prefChange #happy.1.6
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
        Thank you! #prefChange #happy.1.7
    * * [Just a sandwitch]
        Can I get a {~Ham & Cheese Sandwitch|Ham & Cheese Sandwitch|Ham & Cheese Sandwitch|Ham Sandwitch|Tuna Salad Sandwitch}
        Barista 1: You can! Would you like anything else?
        No thanks, thats all I want
        Barista 1: Heres your sandwitch, have a good day!
        Thanks, you too! #prefChange #happy.1.5
    * * [Ask about allergies]
        I have an allergy to nuts, how are nuts handled here?
        Barista 1: We're a nut free shop, so you wont have to worry about that here!
        Awsome, thats great to hear!
        Barista 1: So what can I get for you then?
        -> lcsJumpBack
        
* [Jenns]
    Jenns worker: Hello, welcome to Jenns! What can I get for you today? #back #jenns.day #prefChange #bored.1.-12 #prefChange #stress.1.-5
    - - (jJumpBack)
    What should I choose?
    * * [Sausage roll and a coffee]
        Can I get a Sausage Roll and a latte
        Jenns worker: You can although the latte will take a minute to be ready
        Ok. I don't mind that
        Jenns worker: I'll grab those for you now then
        ...
        ...
        Jenns worker: Heres your latte and Sausage Roll #react #0.0 #prefChange #happy.1.6
    * * [Baguete and a coffee]
        ~ temp randomBaguetteChoice = ""
        ~ randomBaguetteChoice = "{~spicy chicken baguette|ham and cheese baguette|chicken and mayonase baguette|chicken and mayonase baguette|chicken salad baguette|ham baguette|bacon baguette}"
        
        Can I get a {randomBaguetteChoice} and a latte
        Jenns worker: Yes, I'll grab those for you now
        ...
        ...
        Jenns worker: Heres your latte and {randomBaguetteChoice} #react #0.1 #prefChange #happy.1.8
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
        Jenns worker: Heres your {randomCookieChoice} and Sausage Roll #react #0.2 #prefChange #happy.1.8
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
    Deli cashier: Welcome to Sarah's Salad Deli! #back #saladDeli.day #prefChange #bored.1.-16 #prefChange #stress.1.-10
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
            ... 20 minutes later ... #react #1.0 #prefChange #happy.1.4
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
            ... #react #1.1 #prefChange #happy.1.5
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
            ... #react #1.2 #prefChange #happy.1.5
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

* { prepLunch } [Have prepared Lunch instead] #back #officeBreakRoom.day #prefChange #bored.1.5 #prefChange #stress.1.-5
    ...
    ...
    {~*Under breath* Needs more mayo|mmmm|...|...|...|...Hmm, kinda plain}
- ...
Time to get back working
-> END