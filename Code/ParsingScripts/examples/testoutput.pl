#!usr/bin/perl


open my $out_file, '>>', 'myoutputfilename.txt' or die "$!";
my $output = 'blablabla';
print $output;           # print to screen
print $out_file $output; # print to file