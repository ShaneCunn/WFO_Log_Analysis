#!/Perl/bin/perl -w

use POSIX;


open(INFILE, "< acr2.log")
         or die "Cant open file : $!";

my $ParseMessageOut=strftime("Acr.Messagetest_%d-%m-%Y--%H-%M-%S%p.log", localtime);
print "ParseMessageOut is $ParseMessageOut\n";
open(OUT, ">$ParseMessageOut")
         or die "Cant open new file : $!";

use strict;
my $pattern = "Error while detecting new item";
my @history = ("") x 3;
while (<INFILE>) {
print OUT @history if m/tag=824172303f43/;
    push @history, $_;
    shift @history;
}

print $.;
close INFILE;
close OUT;
exit;


