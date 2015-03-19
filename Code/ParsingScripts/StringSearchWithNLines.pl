#!/usr/bin/env perl

use strict;
use POSIX;

my @string_to_find;
my @address;

my $ParseMessageOut=strftime("Acr.MessageOUT_%d-%m-%Y--%H-%M-%S%p.log", localtime);
open(WRITEFILE, ">$ParseMessageOut") or die "Cant open new file : $!";

foreach my $stuff(@ARGV)
{
    if ($stuff =~ (/\:\\/))
	{		
	    @address = $stuff;	
	}
	else
	{
	    push (@string_to_find, "$stuff");    
	}
}

open LOGFILE, "<@address" or die "Cant open file : $!";

#my $hits=0;
my @cont = <LOGFILE>;


for(my $i = 0; $i <= $#cont; $i++) 
{
    my $line = $cont[$i];
    foreach my $strings ( @string_to_find)
	{ 
	    my @word = (split /\:/, $strings)[0];
        my $linesabove = (split /\:/, $strings)[1];
        my $linesbelow = (split /\:/, $strings)[2];
        if ($line =~ @word) 
	    {
            my $st;
		    ($i <= 1) ? ($st = 0) : ($st = $i - $linesbelow);  #lines below
            my $ln = $i - 1;
			
            my $eln = $i + 1;
            my $en = $i + $linesabove;  #lines above
           ($en > $#cont) ? ($en = $#cont) : ();

           print WRITEFILE @cont[$st..$ln];
           print WRITEFILE $line;
           print WRITEFILE @cont[$eln..$en];
		   print WRITEFILE "***";

        }
	}
}

close(LOGFILE);
close(WRITEFILE);
