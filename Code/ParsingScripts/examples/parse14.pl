#!/usr/local/bin/perl
 use strict;
 use warnings ;


  my $mtch1 = 'tag=824172303f43' ;
  my $found ;
  
  my $out = 'output9.txt' ; 
      open my $out_fh,'>', $out or die "Can't open $out $!\n" ;
         
  my $inf = 'acr.log' ;  
      open my $inf_fh,'<',$inf or die "Can't open $inf $!\n" ; 
      
       while ( my $line = <$inf_fh> ) {
                  chomp $line ;
                  
             if ( $line =~ /^$mtch1/ ) {
               print $line;
               
               print scalar <$inf_fh> for 1 .. 2 ;
               
             found = scalar <$inf_fh> for 1 .. 2 ;
                      print $out_fh $found, "\n" ;
             }
           }