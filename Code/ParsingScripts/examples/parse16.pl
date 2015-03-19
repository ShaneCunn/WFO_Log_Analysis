#!/usr/bin/perl
use strict;

my $other = @ARGV;\
open(INFILE, "< acr2.log")
         or die "Cant open file : $!";

open(OUT, "> output22.txt")
         or die "Cant open new file : $!";

my @buffer;   # a queue data structure
#my $numabove = 7
while ( <INFILE> ) {
    if ( /tag=824172303f43/ ) {
        print OUT @buffer;          # 3 lines before
        print OUT;                  # the matching line
        for (my $i=0; $i < 7; $i++) {
        
        print OUT scalar <INFILE>;
         
        }# 1 line following
        last;                   # all done
}
push @buffer, $_;
shift @buffer if @buffer > 2;
}
close(INFILE);
close(OUT);
exit;
