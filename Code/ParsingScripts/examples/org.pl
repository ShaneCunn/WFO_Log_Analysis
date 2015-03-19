#!/usr/bin/env perl

use strict;

my $hits           = 0; 

my $string_to_find = "test";
my $file           = "acr2.log";

open( LOGFILE, $file );
my @cont = <LOGFILE>;
close(LOGFILE);

for ( my $i = 0; $i <= $#cont; $i++ ) {
    my $line = $cont[$i];

    if ( $line =~ /$string_to_find/i ) {
        my $st;
        ( $i <= 5 ) ? ( $st = 0 ) : ( $st = $i - 5 );
        my $ln = $i - 1;

        my $eln = $i + 1;
        my $en  = $i + 5;
        ( $en > $#cont ) ? ( $en = $#cont ) : ();

        print @cont[ $st .. $ln ];
        print $line;
        print @cont[ $eln .. $en ];

    }
}
