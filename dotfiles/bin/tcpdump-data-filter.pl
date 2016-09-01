#!/usr/bin/perl
# This code is hereby placed in the public domain by its author,
# Marc Horowitz .  If you use it, it would be polite if you left
# my name on it, but there's no requirement.
$| = 1;
while(<>) {
   if (/^\s/) {
	   ($nospc = $_) =~ s/\s+//g;
	   ($spc = $nospc) =~ s/(....)/$1 /g;
	   ($bin = pack("H*",$nospc)) =~ tr/\000-\037\177-\377/./;
	   printf("%16s%-45s%s\n","",$spc,$bin);
   } else {
	   print;
   }
}
