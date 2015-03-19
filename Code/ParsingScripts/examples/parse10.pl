#!/Perl/bin/perl -w

use POSIX;


open(INFILE, "< acr.log")
         or die "Cant open file : $!";

my $ParseMessageOut=strftime("Acr.Messagetest_%d-%m-%Y--%H-%M-%S%p.log", localtime);
print "ParseMessageOut is $ParseMessageOut\n";
open(OUT, ">$ParseMessageOut")
         or die "Cant open new file : $!";

#use strict;
my $pattern = "tag=824172303f43";


my @history = ("") x 5;
#my @below = ("") x 10;
while (<INFILE>) {
print OUT @history if /$pattern/;
#print OUT $_ if /$pattern/; # adds line with match
push @history, $_;
shift @history;

my $linebelow = 1000;
print OUT if( (/$pattern/ && ($end=$linebelow))..!$end-- )

}

#print $.;
close INFILE;
close OUT;
exit;

