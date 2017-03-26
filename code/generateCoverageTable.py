#!/usr/bin/python
import sys

"""
Script for parsing DNA coverage among 2 or more organisms at certain 
positions on chromosomes and outputting a summary text file which 
displays every position and how many of the organisms had the same
nucleotide in that spot or a contrary one.

Usage:      python coverage_table_make.py dichotoma 4220 8540
Parameters: organism name, start position, end position

Ezra Huscher
Kane Lab, University of Colorado Boulder

last updated July 2016
"""

# ----------------------------------------------
# All Script Parameters
# ----------------------------------------------
showScaffoldName = False  # True/False
endingStringOfStrainFiles = ".sorted.bam_depth"
informationFile = "info_depth_coverage_flock.txt"
organismList = "names_67strains_nospaces_05042016.txt"
geneName = sys.argv[1]
geneStartPos = int(sys.argv[2])
geneEndPos = int(sys.argv[3])
outputFile = "scaffold"+geneName+"_pk_67individuals.txt"
strainFolder = "depth/"
# ----------------------------------------------
print "start pos: ",geneStartPos," stop pos: ",geneEndPos

# Open connection to writable text files
if organismList != "":
	allStrainNames = open(organismList, "rw+")
if outputFile != "":
	outputfile1 = open(outputFile, "w+") # will create the file if it does not exist

f = open(informationFile, 'r')
infolines = f.readlines()
f.close()

# Loop through strains in organismList text file
try:
	line = allStrainNames.readlines()
	for strainName in line:
	    strainName = strainName.replace("\n","")
	    if strainName != "":	      
	    	
			fullPath = strainFolder + strainName + endingStringOfStrainFiles
			getStrainsLines = open(fullPath, "r+")
			print "Processing file:",fullPath

			# Loop through rows of each strain, format it like we like, and output to file
			linez = getStrainsLines.readlines()
			currentPos = geneStartPos
			for thisLine in linez:
				posnum= int(thisLine.split("\t")[1])
			
				tempGeneName = "scaffold" + geneName + "," 	
				if outputFile != "" and thisLine.find( tempGeneName ) != -1:
					# find this strain in the informationFile containing additional relevant information
					for linezz in infolines:
						if strainName in linezz:
							if int(thisLine.split("\t")[2].replace("\n", "")) != 0: # prevent dividing by 0
								v0 = "\t" + str(round(int(thisLine.split("\t")[2].replace("\n", "")) / float(linezz.split(",")[3]),4))
							else:
								v0 = "\t" + "-"
							v1 = "\t" + linezz.split(",")[1]
							v2 = "\t" + linezz.split(",")[4]
							v3 = "\t" + linezz.split(",")[5]
					var1 = strainName
					var2 = "\t" + thisLine.split("\t")[0]
					var3 = "\t" + thisLine.split("\t")[1]
					var4 = "\t" + thisLine.split("\t")[2].replace("\n", "")
					var5 = "\t" + v1.replace("\n", "")
					var6 = "\t" + v2.replace("\n", "")
					var7 = "\t" + v3
					                 
					if posnum <= geneEndPos and posnum >= geneStartPos:
						# print "test: ",currentPos," ",posnum
						# coverage at this position is 0, so display filler row
						while (currentPos < posnum): 
							outputfile1.write(var1)
							if (showScaffoldName == True):
								outputfile1.write(var2)
							outputfile1.write("\t" + str(currentPos))
							outputfile1.write("\t" + "0")
							outputfile1.write("\t" + "0") # two \t's here?
							outputfile1.write(var5)
							outputfile1.write(var6)
							outputfile1.write(var7)
							currentPos+=1
						
						# position found, display row with desired data 
						currentPos+=1
						outputfile1.write(var1)
						if (showScaffoldName == True):
							outputfile1.write(var2)
						outputfile1.write(var3)
						outputfile1.write(var4)
						outputfile1.write(v0)
						outputfile1.write(var5)
						outputfile1.write(var6)
						outputfile1.write(var7)
           
	    # This section is repeated here to make filler rows if there 
	    # is no coverage on the last positions in the gene's stop position  
	    while (currentPos <= geneEndPos):
            	outputfile1.write(var1)
                if (showScaffoldName == True):
                	outputfile1.write(var2)
                outputfile1.write("\t" + str(currentPos))
                outputfile1.write("\t" + "0")
                outputfile1.write("\t" + "0")
                outputfile1.write(var5)
                outputfile1.write(var6)
                outputfile1.write(var7)
                currentPos+=1

	    print strainName + " completed."
finally:
	if organismList != "":
		allStrainNames.close()
	if outputFile != "":
		outputfile1.close()
