#! /usr/bin/python
# Strat-o-matic basic game player value calculator
# The simple calculator lets you evaluate how well a hitter will do against a pitcher
# trying to emulate  a sabermetic concept of RC27
# which reviews how many runs a team of the same player would score in a game

# we need a random number generator to get dice rolls 
import random

# What follows is the data structure for a hitter
# 11 item list that would include actions that result from 2D6 die roll, 2-12
# each action is either an strike out (so), which increments the out counter
# or is a homerun (HR) and will add a run
# For simplicity, I loaded a sample hitter
H1 = ['HR','so','so','so','so','so','so','so','so','so','HR',]
H2 = ['HR','HR','so','HR','so','so','so','so','so','so','HR',]
H3 = ['HR','so','so','so','so','so','so','so','so','so','HR',]
Hitter1 = [H1,H2,H3,]

#The follow is a data structure for a pitcher
# it follows the same format as a hitter
# again for simplicity, a sample pitcher
P4 = ['so','HR','so','so','so','so','so','so','so','HR','so',]
P5 = ['so','HR','so','so','so','so','so','so','so','HR','so',]
P6 = ['so','HR','so','so','so','so','so','so','so','HR','so',]
Pitcher1 = [P4,P5,P6,]


# on the board game, a 1,2 or 3 on the selector dice would be judged based on the hitters chart
# and a 4,5, or 6 would be based on the pitchers dice

# This getABResult takes the combined action chart for the hitter and batter
# It also takes the row value, which is the selected row that the result is derived from
# it will also take the column which is the location of the result in the row 
# This adjusts for Zero's based in index arrays and will give the AB result

def getABResult(stats, row, coln) :
	#Optional print line
	print("Dice results are: "); print(row); print(coln)
	# Update the fact that dice rolls don't have zero's but indexed arrays dp
	r_index = row-1
	c_index = coln-1
	#get the play line
	playRef = stats[r_index]
	#now return the result
	return playRef[c_index]


# gerRC27 - returns the runs score by that hitter 
# pass two players a hitter and a pitcher, 
# and the die chance to get a result back from the correct cards"
# 
def getRC27 (hitter, pitcher) :
	#create the combined player card/matrix
	comboPlayer = hitter+pitcher

	#Define the beginning of the game
	#start with no score
	score = 0
	# start with now outs
	outs = 0
	
	#Play the game
	# Check to see if the game is over
	#Once we get to 27 outs, the game ends
	while (outs < 27) :
		# Strat uses 3 dice to determine the outcome
		# The first dice define which activity row to use
		# It will be one of six batter or pitcher rows
		selectRow = random.choice=([1,2,3,4,5,6])

		# The next two dice defines the action from the activity line
		d6First = random.choice=([1,2,3,4,5,6])
		d6Sec = random.choice=([1,2,3,4,5,6])
		# the two dice are added together
		action = d6First + d6Sec

		#This is now an at bat, so we get the at bat result
		result = getABResult(comboPlayer, selectRow, action)

		#We check those result to evaluate the outcome of the at bat
		if result =='so' :
			outs += 1

		if result =='HR' :
			score += 1


	#After breaking out of while loop, the game ends 
	print("Game is over! The your scored: "); print(score); print(" runs! Good game")

#Start the game
#this functions uses the default hitter and pitcher define at the top  
getRC27(Hitter1, Pitcher1)