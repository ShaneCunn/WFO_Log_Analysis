#!/usr/bin/perl
use strict;

my $other = @ARGV;\
open(INFILE, "< ASTUFF.txt")
         or die "Cant open file : $!";

open(OUT, "> output22.txt")
         or die "Cant open new file : $!";

my @buffer;   # a queue data structure

while ( <INFILE> ) {
    if ( /inum=833275000008366/ ) {
        print OUT @buffer;          # 3 lines before
        print OUT;                  # the matching line
        for (my $i=0; $i < 3; $i++) {
             print scalar OUT;
        }# 1 line following
        last;                   # all done
}
push @buffer, $_;
shift @buffer if @buffer > 3;
}
close(INFILE);
close(OUT);
exit;
