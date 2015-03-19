# !/usr/bin/perl
#
# Andrew Travis
# 4/9/2012
# Script to read log file, look for string, write out to new log file
# syntax: perl logparser.pl log_file string

use strict;
use warnings;
use lib "C:\\Strawberry\\perl\\lib";

# declare variables
my $filename = $ARGV[0];

#my $filename = "acr.log";
my $search = $ARGV[1];

# open text file
open (IN,$filename) or die $!;
open (OUT,">>logtest10.txt");
while(){
if($_ =~ m/$search/){
print OUT $_;
}
}

# close the open text file
close(IN);
close(OUT);

exit;