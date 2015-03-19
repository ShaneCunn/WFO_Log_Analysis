use strict;
use warnings;
use POSIX;

open( my $fh, '<', 'acr2.log') 
    or die "Cant open new file : $!\n";
	
my $ParseMessageOut=strftime("Acr.Messagetest_%d-%m-%Y--%H-%M-%S%p.log", localtime);
print "ParseMessageOut is $ParseMessageOut\n";
open(OUT, ">$ParseMessageOut")
         or die "Cant open new file : $!\n";

my $this_line = "";
my $do_next = 0;

while(<$fh>) {
    my $last_line = $this_line;
    $this_line = $_;

    if ($this_line =~ /test/) {
        print OUT $last_line unless $do_next;
        print OUT $this_line;
        $do_next = 0;
    } else {
        print OUT $this_line if $do_next;
        $last_line = "";
        $do_next = 0;
    }
}
close ($fh);