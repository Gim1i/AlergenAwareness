=== secondWork ===

//Regular process
= Ev0
Hi Tyler. Anything new?
Tyler: Nope!
Good to hear! #prefChange #bored.1.5
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
    Will do! #react #8.0 #prefChange #happy.1.15 #prefChange #bored.1.-6
* [No]
    I'll pass on that, but thanks for offering
- -> END

//Early end due to very bad weather
= Ev2
...
...
...
\*Looks out the window* Uhhh, that weather's looking real bad #prefChange #stress.1.10
Hey Tyler, any chance we could finish early today?
Getting home might become impossible for some if this weather keeps up
Tyler: *Looks outside* Oh yea thats really bad, I might not make it home myself
Tyler: Ok we'll finish early today, feel free to go and I'll let the rest of the office know shortly
Thanks Tyler! #prefChange #stress.1.-10
Tyler: No problem!
-> END

//Down colleague (NOT RANDOMLY GENERATED)
= Ev3
Tyler: Just a reminder, we're short-staffed so I'd appreciate if you got on with work promptly
Oh yea, I forgot about that #prefChange #stress.1.3 #prefChange #tired.1.8
I'll get on with it immediately then
\*Tyler give a thumbs up*
-> END