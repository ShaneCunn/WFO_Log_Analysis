#!/Perl/bin/perl -w

use POSIX;
use strict;
use warnings;

open(INFILE, "< acr.log")
         or die "Cant open file : $!";

my $ParseMessageOut=strftime("Acr.Messagetest_%d-%m-%Y--%H-%M-%S%p.log", localtime);
print "ParseMessageOut is $ParseMessageOut\n";
open(OUT, ">$ParseMessageOut")
         or die "Cant open new file : $!";

my $pattern = "tag=824172303f43|Total Memory = 133808128,|ID: ddfc277e412dde29ec16963ce3f53d2e872faed3e84e22382b2071c20e2cf28";

my @history = ("") x 5;

my $linebelow = 10;
while (<INFILE>) {
print OUT @history if /$pattern/;
#print OUT $_ if /$pattern/; # adds line with match
push @history, $_;
shift @history;

print OUT if( (/$pattern/ && ($linebelow))..!$linebelow-- ); # print current line and -n lines below 
}

#print $.;
close INFILE; 
close OUT;
exit;