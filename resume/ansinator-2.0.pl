#!/usr/bin/perl

use strict;

my $UsageHelp = <<"ENDOFHEREDOC";
The Ansinator 2.1!
    By Nextraztus

Pipe your source file thru the program, here are the escape codes:
     (All codes are case-insensitive)

 \$\$ = draw a \$
 \$O = \x1b[1mbOld\x1b[0m
 \$N = Reset to Normal

 \$K = \x1b[47;30mNormal blacK\x1b[0m          \x1b[1;30mBold blacK\x1b[0m
 \$R = \x1b[31mNormal Red\x1b[1m            Bold Red\x1b[0m
 \$G = \x1b[32mNormal Green\x1b[1m          Bold Green\x1b[0m
 \$Y = \x1b[33mNormal Yellow\x1b[1m         Bold Yellow\x1b[0m
 \$B = \x1b[34mNormal Blue\x1b[1m           Bold Blue\x1b[0m
 \$M = \x1b[35mNormal Magenta\x1b[1m        Bold Magenta\x1b[0m
 \$C = \x1b[36mNormal Cyan\x1b[1m           Bold Cyan\x1b[0m
 \$W = \x1b[37mNormal White\x1b[1m          Bold White\x1b[0m

View file example.ansi to see how to use this program...
     run the following to view the output!

   cat example.ansi | ./ansinator-2.0.pl

ENDOFHEREDOC

if($ARGV[0])
{
	print $UsageHelp;
	exit;
}

while(<STDIN>)
{
	s/\$\$/\$°/g;
	
	s/\$k/\x1b[30m/gi;
	s/\$r/\x1b[31m/gi;
	s/\$g/\x1b[32m/gi;
	s/\$y/\x1b[33m/gi;
	s/\$b/\x1b[34m/gi;
	s/\$m/\x1b[35m/gi;
	s/\$c/\x1b[36m/gi;
	s/\$w/\x1b[37m/gi;

	s/\$O/\x1b[1m/gi;
	s/\$N/\x1b[0m/gi;

	s/°//g;

	print;
}

