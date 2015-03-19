#!/usr/bin/env perl

use strict;
use diagnostics;
use POSIX;

my $hits           = 0;
my $string_to_find = "test";
my $file           = "acr2.log";

# open( INFILE, $ARGV[0] )
#     or die "Cant open file : $!";

my $WRITEFILE = strftime( "Acr.MessageOUT_%d-%m-%Y--%H-%M-%S%p.log", localtime );
print "WRITEFILE is $WRITEFILE\n";
open( WRITEFILE, ">$WRITEFILE" )
    or die "Cant open new file : $!";

open( LOGFILE, $file );
my @cont = <LOGFILE>; # store file in a var called @cont
close(LOGFILE);

for ( my $i = 0; $i <= $#cont; $i++ ) {  # for the amonunts line in cont, 
    my $line = $cont[$i]; # assign cont $0 , increase everytime

    if ( $line =~ /$string_to_find/i ) {  # it compares the line to the search string , it continue if it finds a match
        my $st; # create var ST
        ( $i <= 5 ) ? ( $st = 0 ) : ( $st = $i - 2 ); # if $i is less or equal to 5 or is eq to 0 then st is = to $i - 2
        my $ln = $i - 1;

        my $eln = $i + 1; # current line var + 1
        my $en  = $i + 9; # line below var + 9
        ( $en > $#cont ) ? ( $en = $#cont ) : (); # if $en is great than $ count  or is =  $cont   then do nothing  

        print WRITEFILE @cont[ $st .. $ln ]; # this prints the line in range of $st to $ln
        print WRITEFILE $line;
        print WRITEFILE @cont[ $eln .. $en ]; 

    }
}
close(WRITEFILE);

#close OUT;
exit;