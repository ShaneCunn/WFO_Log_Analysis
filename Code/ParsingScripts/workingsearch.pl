#!/Perl/bin/perl -w

use POSIXuse strict;
use warnings;

# open(INFILE, "< acr2.log")
#          or die "Cant open file : $!";
# $num_args = $#ARGV + 1;
# if ($num_args != 1) {
#     print "\nUsage: blah blah blah\n";
#     exit;
open( INFILE, $ARGV[0] )
    or die "Cant open file : $!";

my $ParseMessageOut
    = strftime( "Acr.MessageOUT_%d-%m-%Y--%H-%M-%S%p.log", localtime );
print "ParseMessageOut is $ParseMessageOut\n";
open( OUT, ">$ParseMessageOut" )
    or die "Cant open new file : $!";

open my $pattern, '<', "search.txt" or die "Can't open search words: ";

#open my $pattern, '<', "search.txt" or die "Can't open search words: ";

# (1) quit unless we have the correct number of command-line args

# my $pattern_or = join '|', map {chomp;qr/\Q$_\E/} <$pattern>;
# my $regex = qr|\b($pattern_or)\b|;
#my $pattern = ;
my $pattern = "test";

my @history = ("") x $ARGV[1];

#my @after = ("") x 0;
my $linebelow = $ARGV[2];

while (<INFILE>) {

    #print OUT @history if /$regex/;
    print OUT @history if /$pattern/;

    #print OUT $_ if /$regex/; # adds line with match
    push @history, $_;
    shift @history;

    #print OUT @after if /$regex/;
    #push @after, $_;
    #shift @after;

#print OUT if ((/$regex/ && ($linebelow))..!$linebelow-- ); # print current line and -n lines below
    print OUT if ( ( /$pattern/ && ($linebelow) ) .. !$linebelow-- );
}

#print $.;
close INFILE;
close OUT;
exit;
