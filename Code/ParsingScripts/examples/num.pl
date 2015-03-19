use feature qw/say/;
for (1..6) {
    my $number = int(rand(58)) + 1;
    redo if $number == 13;
    say $number;
}