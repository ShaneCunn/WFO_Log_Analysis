use strict;
use warnings;

my $in_file = 'acr2.log';
open my $in_fh, '<', $in_file or die "Could not open file $in_file: $!
+";
my $out_file = 'output155.txt';
open my $out_fh, '>', $out_file or die "Could not open file $out_file:
+$!";
my $print_next = 0;
while ( my $line = <$in_fh> )
{
  if ($print_next){
     print $out_fh $line;  #No need to chomp - we print the "\n"
  }
  $print_next =  ($line =~ /^tag=824172303f43/);
}
close $in_fh or die "Could not close file $in_file: $!";
close $out_fh or die "Could not close file $out_file: $!";