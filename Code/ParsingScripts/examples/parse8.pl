

open(INFILE, "< acr.log")
         or die "Cant open file : $!";

open(OUT, "> results10.txt")
         or die "Cant open new file : $!";

$pattern = "tag=824172303f43";

my @file;
while (<INFILE>) {
    print OUT @file if (/$pattern/ and @file);
    push @file, $_;           # push on the current line
    shift @file while (@file > 303); # shorten to 3 elements
}

close INFILE;
close OUT;