INCLUDE Driving_Dialogue.ink

-> Sec0.Ev1 //FOR TESTING


//
// dayStart
//
=== Sec0 === 

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
..."
..."
Why don't I hear my alarm?
\*Jolts out of bed and checks the clock*
Oh I woke up early *Sigh*
\*Gets dressed for work*
Should I prepare lunch today?
-> lunchPrepChoice

//Late wake
= Ev2
..."
..."
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
*Tries again*
Damn thing wont start
\*Sigh* I've got to call Tyler
\*Ring Ring Rin-*
Hi Tyler, I'm probably not going to be able to be in this morning as my car isn't starting
Tyler: What?! Thats not good
Tyler: Do you think you will be able to make get here by lunch time?
Yea I can be there by lunch. Just have to give the car to the garage
Tyler: Good to hear! I'll see you after the lunch break then
\*Click*
Luckly the issue was quite minor so the car will be fixed by this evening #to #3
-> END //Skips first work

//
// firstWork
//
=== Sec2 === 

//Regular process
= Ev0
Hi Tyler. Anything important I should know?
Tyler: Nope, just a regular day!
Good to hear! I'll get to working then
\*Tyler give a thumbs up*
-> END

//Homemade food
= Ev1
Hi Tyler. Whats happening over there?
Tyler: Someone brought in some "+randomHomeMadeFood
Tyler: It's really good, you should have some!
Should I have some?
* [Yes]
    I'll pass on that, but thanks for offering
* [No]
    Will do! #react #7
- -> END

//Shop food
= Ev2
Hi Tyler. Whats happening over there?
Tyler: Someone brought some "+randomFirstBroughtFood+" on their way to work
Tyler: It's free so I recomend grabing some!
Should I have some?
* [Yes]
    I'll pass on that, but thanks for offering
* [No]
    Will do! #react #8
- -> END

//Down colleague
= Ev3
Hi Tyler. Anything important I should know?
Tyler: Someone called in sick earlier so we're going to be short-staffed today
You know wether they'll be able to be here tommorow?
Tyler: Luckly they said they should be here tommorow
Good to hear! Give them a get well soon from me next time you speak to them
Tyler: Will do!
\*Walks of to start work* #save #colleagueDown
-> END

//Work celebration
= Ev4
-> END

//
// lunch
//
=== Sec3 === 

//Regular process
= Ev0
-> END

//
// secondWork
//
=== Sec4 === 

//Regular process
= Ev0
-> END

//
// workEndTravel
//
=== Sec5 === 

//Regular process
= Ev0
-> END

//
// afternoon
//
=== Sec6 === 

//Regular process
= Ev0
-> END

//
// homeTravel
//
=== Sec7 ===

//Regular process
= Ev0
-> END

//
// dayEnd
//
=== Sec8 ===

//Regular process
= Ev0
-> END