import os, re
#User interface
#       Greeting
#       Scan for files

print "Salutations!\n"
allFiles = False

def missingFiles():
        files = os.listdir(os.getcwd())
        layoutTest = "layout.txt" in files
        panelTest = "panel.txt" in files
        pcbTest = "pcb.txt" in files
        componentTest = "component.txt" in files
        packageTest = "package.txt" in files
        allFiles = True
        missingFiles = "The following are needed to begin:\n"
        if not layoutTest:
                missingFiles += "\tlayout.txt\n"
                allFiles = False
        if not panelTest:
                missingFiles += "\tpanel.txt\n"
                allFiles = False
        if not pcbTest:
                missingFiles += "\tpcb.txt\n"
                allFiles = False
        if not componentTest:
                missingFiles += "\tcomponent.txt\n"
                allFiles = False
        if not packageTest:
                missingFiles += "\tpackage.txt\n"
                allFiles = False
        if not allFiles:
                print missingFiles
        return allFiles

while not missingFiles():
        response = raw_input("Please verify all files are in the same directory as this script and named correctly, then press Enter to retry.\n")

print "All necessary files located, generating report...\n"

#To keep iterators from missing the last line
def newLine(string):
        newLine = open (string, "a")
        newLine.write("\n")
        newLine.close()

newLine("layout.txt")
newLine("panel.txt")
newLine("pcb.txt")
newLine("component.txt")
newLine("package.txt")


#Build lists from Layouts:
#       layouts with no associated panels
#       all panels associated with a layout (for next step)
associatedPanels = set()
associatedPCBS = set()
layOrphans = []
layoutIn = open("layout.txt", "r")

layRaw = []
for line in layoutIn:
        if (line[0:3] == "S1 " or line[0:3] == "S3P" or line[0:3] == "S3B"):
            layRaw.append(line[0:-1])
layRaw.append("EoF")
layoutIn.close()

numLayouts = 0
for i in range(0, len(layRaw) - 1):
        if layRaw[i][0:2] == "S1":
                numLayouts += 1
        if layRaw[i][0:3] == "S3P":
                associatedPanels.add(layRaw[i][4:])
        if layRaw[i][0:3] == "S3B":
                associatedPCBS.add(layRaw[i][4:])
        elif (layRaw[i][0:2] == "S1" and  layRaw[i + 1][0:2] != "S3"):
                layOrphans.append(layRaw[i])

#-----------------------Write to Files------------------------------#
layoutOrphansOut = open("Orphan Layouts.txt", "w")
layoutOrphansOut.write("Total layouts scanned: " + str(numLayouts) + "\n")
layoutOrphansOut.write("Obsolete layouts identified: " + str(len(layOrphans)) + "\n\n")
for x in layOrphans:
    layoutOrphansOut.write(x + "\n")
layoutOrphansOut.close()

#Writes associated panels to a file for testing purposes

##layoutOut = open("AssociatedPanels.txt", "w")
##layoutOut.write("Number of associated panels: " + str(len(associatedPanels)) + "\n\n")
##for x in associatedPanels:
##    layoutOut.write(x + "\n")
##layoutOut.close()
#-------------------------------------------------------------------#

#Build lists from Panels:
#       panels with no associated pcbs
#       PCBs associated with panels not in previous step
#               Panels should be added to orphans
#       List of all PCBs (for next step)

#associatedPCBS = set()
panOrphans = []
panelIn = open("panel.txt", "r")

raw = []
for line in panelIn:
    if line[0:2] == "P1" or line[0:2] == "P7":
       raw.append(line[0:-1])
raw.append("EoF")
panelIn.close()

numPanels = 0
for i in range(0, len(raw) - 1):
        if (raw[i][0:2] == "P1"):
            numPanels += 1
        if (raw[i][0:2] == "P7" and raw[i - 1][3:] in associatedPanels):
                associatedPCBS.add(raw[i][3:])
        elif (raw[i][0:2] == "P1" and raw[i + 1][0:2] != "P7"):
                panOrphans.append(raw[i])
        elif (raw[i][0:2] == "P1" and raw[i][3:] not in associatedPanels):
                panOrphans.append(raw[i])

#-----------------------Write to Files------------------------------#
panOrphansOut = open ("Orphan Panels.txt", "w")
panOrphansOut.write("Total panels scanned: " + str(numPanels) + "\n")
panOrphansOut.write("Obsolete panels identified: " + str(len(panOrphans)) + "\n\n")
for x in panOrphans:
    panOrphansOut.write(x + "\n")
panOrphansOut.close()

#Writes associated PCBs to a file for testing purposes

##assocPCBSOut = open("Associated PCBS.txt", "w")
##assocPCBSOut.write("Number of associated PCBS: " + str(len(associatedPCBS)) + "\n\n")
##for x in associatedPCBS:
##        assocPCBSOut.write(x + "\n")
##assocPCBSOut.close()
#------------------------------------------------------------------#

#Build lists from PCBS:
#       PCBs with no associated componenets
#       PCBs with components not associated with Panels from previous step
#       List of all components (for next step)
#       Generate frequency counts for Pick Data
associatedComponents = set()
pcbOrphans = []
pcbsIn = open("pcb.txt", "r")

pcbRaw = []
for line in pcbsIn:
    if (line[0:3] == "F1 " or line[0:3] == "F8 "):
        pcbRaw.append(line)
pcbRaw.append("EoF")
pcbsIn.close()

pcbRawOut = open("pcbRaw.txt", "w")

#Count frequency of parts for Pick Data.
#Key is component SKU
#Value is 2D array
#       1st value is frequency
#       2nd value is number of assemblies using component
pickDataFreq = {}
pickDataCount = {}
numPCBS = 0
currentPCB = ""
for i in range (0, len(pcbRaw) - 1):
        pcbRawOut.write(pcbRaw[i] + "\n")
        if (pcbRaw[i][0:2] == "F1"):
                numPCBS += 1
                currentPCB = pcbRaw[i][3:-1]
        if (pcbRaw[i][0:2] == "F8" and currentPCB in associatedPCBS):
                x = 0
                if (pcbRaw[i].find("N N") > 0):
                        x = pcbRaw[i].find("N N")
                elif (pcbRaw[i].find("Y N") > 0):
                        x = pcbRaw[i].find("Y N")
                elif (pcbRaw[i].find("N Y") > 0):
                        x = pcbRaw[i].find("N Y")
                else:
                        x = pcbRaw[i].find("Y Y")

                #PickData Counts
                if (pcbRaw[i][x + 4:-1] in pickDataCount): 
                        if pcbRaw[i - 1][x + 4:-1] != pcbRaw[i][x + 4:-1]:
                                pickDataCount[pcbRaw[i][x + 4:-1]] += 1
                        pickDataFreq[pcbRaw[i][x + 4:-1]] += 1
                else:
                        pickDataCount[pcbRaw[i][x + 4:-1]] = 1
                        pickDataFreq[pcbRaw[i][x + 4:-1]] = 1
                        
                associatedComponents.add(pcbRaw[i][x + 4:])
        elif (i < len(pcbRaw) - 1 and pcbRaw[i][0:3] == "F1 " and pcbRaw[i + 1][0:2] != "F8"):
                pcbOrphans.append(pcbRaw[i])
        elif (pcbRaw[i][0:3] == "F1 " and pcbRaw[i][3:-1] not in associatedPCBS):
                pcbOrphans.append(pcbRaw[i])

pcbRawOut.close()

#-----------------------Write to Files------------------------------#
pcbOrphansOut = open("Orphan PCBs.txt", "w")
pcbOrphansOut.write("Total PCBs scanned: " + str(numPCBS) + "\n")
pcbOrphansOut.write("Orphan PCBs found: " + str(len(pcbOrphans)) + "\n\n")
for x in pcbOrphans:
    pcbOrphansOut.write(x)
pcbOrphansOut.close()

#Writes associated Components to a file for testing purposes

##assocComponents = open("Associated Components.txt", "w")
##assocComponents.write("Number of associated components: " + str(len(associatedComponents)) + "\n\n")
##for x in associatedComponents:
##        assocComponents.write(x)
##assocComponents.close()

pickData = open("Pick Data.txt", "w")
pickData.write("SKU ; Number of Assemblies ; Total Number of Placements\n")
for x in pickDataCount:
        pickData.write(x + " ; " + str(pickDataCount.get(x)) + " ; " + str(pickDataFreq.get(x)) +  "\n")
pickData.close()
##pickData.write("\nTotal Number of Placements:\n")
##for x in pickDataFreq:
##        pickData.write(x + " : " + str(pickDataFreq.get(x)) + "\n")
##pickData.close()
#-------------------------------------------------------------------#

#Build lists from Components:
#       Componenents with no associated packages
#       Componenents with packages but not associated with PCB from previous step
#       List of all Packages
associatedPackages = set()
componentOrphans = []
componentsIn = open("component.txt", "r")

componentRaw = []
for line in componentsIn:
        if (line[0:3] == "C00" or line[0:3] == "C01"):
                componentRaw.append(line)
componentRaw.append("EoF")
componentsIn.close()

numComponents = 0
for i in range(0, len(componentRaw)):
        if (componentRaw[i][0:3] == "C00"):
            numComponents += 1
        if (componentRaw[i][0:3] == "C01" and componentRaw[i - 1][4:] in associatedComponents):
                associatedPackages.add(componentRaw[i][4:])
        elif (componentRaw[i][0:3] == "C00" and componentRaw[i + 1][0:3] != "C01"):
                componentOrphans.append(componentRaw[i])
        elif (componentRaw[i][0:3] == "C00" and componentRaw[i][4:] not in associatedComponents):
                componentOrphans.append(componentRaw[i])
        

#-----------------------Write to Files------------------------------#
compOrphansOut = open("Orphan Components.txt", "w")
compOrphansOut.write("Total components scanned: " + str(numComponents) + "\n")
compOrphansOut.write("Orphan components found: " + str(len(componentOrphans)) + "\n\n") 
for x in componentOrphans:
        compOrphansOut.write(x)
compOrphansOut.close()

#Writes associated packages to a file for testing purposes

##assocPackages = open("Associated Packages.txt", "w")
##assocPackages.write("Number of associated packages: " + str(len(associatedPackages)) + "\n\n")
##for x in associatedPackages:
##        assocPackages.write(x)
##assocPackages.close()
#-------------------------------------------------------------------#

#Build lists from Packages:
#       Packages not associated with Componenent from previous step
#       List MyCronic default packages?
#This section is not currently being used, but might be added in the future

#-----------------------Write to Files------------------------------#
#-------------------------------------------------------------------#

#Notify user of status before closing
response = raw_input("Finished generating reports. Press Enter to close")
