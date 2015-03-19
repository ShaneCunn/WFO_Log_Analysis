#!/usr/bin/perl
use warnings;
use strict;
use POSIX;
use Fcntl;
use diagnostics;

#open (FILE2, ">output.txt") or die "Cant open file : $!";

my $FILE2 = strftime( "Acr.MessageOUT_%d-%m-%Y--%H-%M-%S%p.log", localtime );
print "FILE2 is $FILE2\n";
open( FILE2, ">$FILE2" )
    or die "Cant open new file : $!";

#print FILE2 "$FILE2";

# my $ParseMessageOut=strftime("Acr.MessageOUT_%d-%m-%Y--%H-%M-%S%p.log", localtime);
# print "ParseMessageOut is $ParseMessageOut\n";
# open(OUT, ">$ParseMessageOut")
#          or die "Cant open new file : $!";

#my $something = @ARGV;
#print $something;
#my $other = @ARGV[0];
#print $other;
#my $mylist = [""];
my @mylist;

# my @address;
# my $relativePath = File::Spec->abs2rel ($filePath,  $parentPath);

my @address;

#my $stuff = File::Spec->abs2rel ($filePath,  $parentPath);

#my @word = "";
my @buffer;    # a queue data structure

foreach my $stuff (@ARGV) {
    if ( $stuff =~ "/Q:\\/E" ) {

        #print $stuff;
        #@mylist = "$stuff\n";
        push( @mylist, "$stuff\n" );

        #print $mylist;
    }
    else {
        @address = $stuff;
    }
}

open FILE, "<acr2.log" or die "Cant open file : $!";

#print @mylist;

while (<FILE>) {
    foreach $a (@mylist) {
        my @word = ( split /\:/, $a )[0];
        my @linesabove = -( split /\:/, $a )[1];
        my @linesbelow = ( split /\:/, $a )[2];
        if ( $_ =~ @word ) {

            #print FILE2 "$_\n";
            while ( "@linesabove" < 0 ) {
                print FILE2 @buffer[@linesabove];    # 3 lines before
                @linesabove = "@linesabove" + 1;
            }

            print FILE2;                             # the matching line

            #print @linesbelow;
            for ( my $i = 0; $i < "@linesbelow"; $i++ ) {
                print FILE2 scalar <FILE>;

            }                                        # 1 line following

            print FILE2 "*********\n";
            last;                                    # all done
        }
    }
    push @buffer, $_;
    shift @buffer if @buffer > 30;
}

close(FILE);
close(FILE2);

#close OUT;
exit;
