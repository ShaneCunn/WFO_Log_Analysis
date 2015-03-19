#!usr/bin/perl
use strict;
use warnings;

# To run on windows install strawberry perl and
# then run this cmd command: perl -w parse.pl > testout.txt

#Lexical variable for filehandle is preferred, and always error check opens.
open my $keywords,    '<', 'search.txt' or die "Can't open keywords: $!";
open my $search_file, '<', 'acr2.log'   or die "Can't open search file: $!";

my $keyword_or = join '|', map {chomp;qr/\Q$_\E/} <$keywords>;
my $regex = qr|\b($keyword_or)\b|;

while (<$search_file>)
{
    while (/$regex/g)
    {
        print "$.: $1\n";
		
		
    }
	
}
