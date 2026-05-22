=== firstWork === 

//Regular process
= Ev0
Hi Tyler. Anything important I should know?
Tyler: Nope, just a regular day!
Good to hear! I'll get to working then
\*Tyler give a thumbs up* #prefChange #bored.1.5
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
    Will do! #react #7.0 #prefChange #happy.1.15 #prefChange #bored.1.-6
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
    Will do! #react #8.0 #prefChange #happy.1.15 #prefChange #bored.1.-6
* [No]
    I'll pass on that, but thanks for offering
- -> END

//Down colleague
= Ev3
Hi Tyler. Anything important I should know?
Tyler: Someone called in sick earlier so we're going to be short-staffed today #prefChange #stress.1.3 #prefChange #tired.1.8
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
Tyler: Theres free food and drinks if you want any #prefChange #happy.1.5
Should I get some free food?
* [Yes]
    ~ temp randomWorkPizzaChoice = ""
    ~ randomWorkPizzaChoice = "{~pepperoni|margherita}"
    
    Will do! which do you recomend?  #prefChange #happy.1.12 #prefChange #bored.1.-10
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