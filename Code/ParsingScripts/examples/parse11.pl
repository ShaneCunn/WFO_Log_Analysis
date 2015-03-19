use strict;
use warnings;

my %lookupTable;

print "Enter the file path of lookup table: \n";
chomp( my $lookupTableFile = <> );

print "Enter the file path that contains keywords: \n";
chomp( my $keywordsFile = <> );

open my $ltFH, '<', $lookupTableFile or die $!;

while (<$ltFH>) {
    chomp;
    undef @{ $lookupTable{$_} };
}

close $ltFH;

open my $kfFH, '<', $keywordsFile or die $!;

while (<$kfFH>) {
    chomp;
    for my $keyword ( split /\t+/ ) {
        push @{ $lookupTable{$keyword} }, $. if defined $lookupTable{$keyword};
    }
}

close $kfFH;

open my $slFH, '>', 'SampleLineNum2.txt' or die $!;

print $slFH "$_: @{ $lookupTable{$_} }\n"
  for sort { lc $a cmp lc $b } keys %lookupTable;

close $slFH;

print "Done!\n";