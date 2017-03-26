#!/bin/bash 

# This script tries many combinations of K and d values for SOAP, 
# a genomic alignment tool,
# when assembling a genome. It returns the number of scaffolds and 
# longest scaffold length for each.

# Takes two parameters: 
#    1.) K - starting K-mer length (will try up to 63)  default: 25
#    2.) d - frequency cutoff (needs at least this many of a K-mer to keep it)  default: 5

# Ezra Huscher, Spring 2015

if [[ $1 -eq 0 ]] ; then
    $1=25
fi
if [[ $2 -eq 0 ]] ; then
    $2=5
fi
START=$1
FREQ=$2
let COUNTER=$START-2
while [ $COUNTER -lt 63 ]; do
     let COUNTER=$COUNTER+2
     VAR="K"$COUNTER"d"$FREQ
     VAR2=$VAR".scafStatistics"
     soapdenovo2-63mer all -s soap.config -K $COUNTER -d $FREQ -R -F -o $VAR 1> K49.err 2> K43.log
     q=$(echo "~ K = "$COUNTER "and d = "$FREQ)
     q+=$(grep -m2 "caffold" $VAR2 | tail -n1 | awk '{print " --- Scaffolds: "$2}')
     q+=$(grep -m1 "Longest" $VAR2 | tail -n1 | awk '{print " --- Longest: "$2}')
     echo "${q}"
done
