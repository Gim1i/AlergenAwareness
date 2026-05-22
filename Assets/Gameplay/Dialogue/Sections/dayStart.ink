=== dayStart === 

//Regular process
= Ev0
\*Alarm beeping* #prefChange #tired.1.-30 #prefChange #happy.1.6 
...
Uuurg, damn it
\*Gets up and dressed for work*
Should I prepare lunch today?
-> lunchPrepChoice

//Early wake
= Ev1
...  #prefChange #tired.1.-22
...
Why don't I hear my alarm?
\*Jolts out of bed and checks the clock*
Oh I woke up early *Sigh*  #prefChange #happy.1.6
\*Gets dressed for work*
Should I prepare lunch today?
-> lunchPrepChoice

//Late wake
= Ev2
... #prefChange #tired.1.-22
...
Why don't I hear my alarm?
\*Jolts out of bed and checks the clock*
Damn I'm late! #prefChange #stress.1.10
\*Dressed for work as fast as possible*
Theres no time to prepare lunch today
\*Rushes out the door*
-> END

//Prep Choice
= lunchPrepChoice //(-> returnTo)
* [Yes]
    \*Prepares lunch* #save #prepLunch #prefChange #bored.1.-3
* [No]
- Time to head to work then
-> END