                                                         atBatSimple.py 
Overview:

Thank you for looking at the simple at bat RC27 creation engine. I created this to quickly analyze total value for a hitter based on a pitching matchup. The simple tool uses the Strat-o-matic basic game rules which have 3 hitter lines and 3 pitcher lines for each player. The advanced rules have 6 lines for all players

Info about strat-o-matic (SOM): http://www.strat-o-matic.com/

Info about the real RC27: http://en.wikipedia.org/wiki/Runs_created#Basic_runs_created

RC27 is an advanced baseball metric that answers the question, "How many runs would a player score if he was the only batter in a game?" It is a useful metric to understand the complete offensive production of a player. The higher the number the more they contribute to a teams offense.  

I'm new to SOM and I wanted to try and find a way to compare how different hitters would perform against different pitchers, using only the SOM rules. I've created this program to do that analysis. Ideally, I can run the simulation a thousand times for each player match up and average the production so I can understand who I should play at any given time. 

Note about player information: There is no easy way to create the player cards. SOM keeps that information to themselves so its not digitalized. I own the cards and still need to manually enter the results for each player. A dictionary with some players will probably be available in the future. 

How to run:
$Python atBatSimple.py 
This should output a total score for a 27 out game. for the pre built 'Hitter' and 'Pitcher' 

todo: I still need to update it to take players as arguments, I would need the player dictionary to reference


How it works: 
The game will line up a hitter card with a pitcher card into a unified element. SOM is a dice based game, so I will use a random number generator to simulate the dice rolls. I will then look up the result of the "At Bat" using those dice rolls against the combined player card. You then evaluate the result to see what happens. 


Changes for SparkFun: 
I've streamline the program for inclusion in the SparkFun repo. Baseball can be a complicated game, and SOM seems to thrive on that complication. Below is a list of activities related to simplifying the program

Major changes: 
I've loaded a sample Hitter and Pitcher into the main body. 
The hitters and pitcher only strike out or hit home runs. 
The logic behind base running on hits is complicated so I moved all of that out. 
Since there are no base runners I eliminated the logic that checks to see if the inning ended that clears the bases. I've eliminated the concept of bases all together. 
Not needing innings, I updated the out count to just run until we have 27 outs.  

The game can now run and output a score directly. 

Seeing dice rolls:
Currently  line 39 is commented out, if you uncomment that it will print out the dice rolls for each at bat
todo: make this optional from the command line. 
