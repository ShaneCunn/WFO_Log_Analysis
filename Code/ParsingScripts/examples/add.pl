#!/usr/bin/perl

use Cwd ‘abs_path';

my $root = $ARGV[1];

$root = abs_path($root);

print “$root\n”;