#!/usr/bin/perl

use POSIX;

my $AcrMessage=strftime('Acr.Message_%H:%M--%d-%m-%y.log', localtime);
print "AcrMessage is $AcrMessage\n";
open(FILE, ">$AcrMessage") || die("Couldn't open file");

print FILE "hello world\n";

close(FILE);
exit(0);

