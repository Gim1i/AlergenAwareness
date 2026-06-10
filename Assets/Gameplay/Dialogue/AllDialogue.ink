INCLUDE Sections/dayStart.ink
INCLUDE Sections/workStartTravel.ink
INCLUDE Sections/firstWork.ink
INCLUDE Sections/lunch.ink
INCLUDE Sections/secondWork.ink
INCLUDE Sections/workEndTravel.ink
INCLUDE Sections/afternoon.ink
INCLUDE Sections/homeTravel.ink
INCLUDE Sections/dayEnd.ink

=== Sec0 === 
#back #bedroom.day

// Reset all afflicts
#prefChange #tinglingThroat.0.0
#prefChange #runnyNose.0.0
#prefChange #tightChest.0.0
#prefChange #hardToBreath.0.0
#prefChange #sick.0.0

// Reduce most negative emotions
#prefChange #sad.1.-22
#prefChange #angry.1.-28
#prefChange #pain.1.-25
#prefChange #stress.1.-20
#prefChange #feelingSick.1.-45

// Reduce happy to force the player into actions
#prefChange #happy.1.-15
#event #set.dayStart
-> dayStart

=== Sec1 === 
#back #driving.day
#event #set.workStartTravel
-> workStartTravel

=== Sec2 === 
#back #officeJob.day
#prefChange #tired.1.5
#event #set.firstWork
-> firstWork

=== Sec3 === //No background as it changes after chosing a lunch option
#prefChange #stress.1.-15
#event #set.lunch
-> lunch

=== Sec4 === 
#back #officeJob.day
#prefChange #tired.1.5
#event #set.secondWork
-> secondWork

=== Sec5 === 
#back #driving.day
#event #set.workEndTravel
-> workEndTravel

=== Sec6 ===
#back #livingRoom.day
#event #set.afternoon
-> afternoon

=== Sec7 ===
#back #driving.evening
#prefChange #tired.1.7
#event #set.homeTravel
-> homeTravel

=== Sec8 ===
#back #bedroom.evening
#event #set.dayEnd
-> dayEnd