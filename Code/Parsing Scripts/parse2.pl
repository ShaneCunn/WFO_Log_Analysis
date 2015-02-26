#!usr/bin/perl
use strict;
use warnings;

my $linenum = 0;

print "Enter the file path of lookup table:";
my $filepath1 = <>;

print "Enter the file path that contains keywords :";
my $filepath2 = <>;

open( FILE1, "< $filepath1" );
open FILE2, "< $filepath2" ;

# Read in all of the keywords
my @keywords = <FILE2>; 

# Close the file2
close(FILE2);

# Remove the line returns from the keywords
chomp @keywords;

# Sort and reverse the items to compare the maximum length items
# first (hello there before hello)
@keywords = reverse sort @keywords;

foreach my $k ( @keywords)
{
  print "$k\n";
}
open OUT, ">", "SampleLineNum.txt";
my $line;
# Counter for the lines in the file
my $count = 0;
while( $line = <FILE1> )
{
    # Increment the counter for the number of lines
    $count++;
    # loop through the keywords to find matches
    foreach my $k ( @keywords ) 
    {
        # If there is a match, print out the line number 
        # and use last to exit the loop and go to the 
        # next line
        if ( $line =~ m/$k/ ) 
        {
            print "$count\n";
            last;
        }
    }
}

close FILE1;