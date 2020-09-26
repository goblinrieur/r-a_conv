#! /usr/local/bin/gforth-fast
create (arabic)
	1000 128 * char M + ,
	500 128 * char D + ,
	100 128 * char C + ,
	50 128 * char L + ,
	10 128 * char X + ,
	5 128 * char V + ,
	1 128 * char I + ,
does>
	7 cells bounds do
		i @ over over 
		127 and = if 
			nip 7 rshift leave 
		else 
			drop 
		then
		1 cells 
	+loop dup
;
\ here we have arabic convertion table
 
: >arabic
	0 dup >r >r
	begin
		over over
		while
		c@ dup (arabic) rot <>
		while
		r> over r> over over 
		> if 
			2* negate + 
		else 
			drop 
		then 
		+ swap >r >r 1 /string
	repeat then drop 2drop r> r> drop
	. \ immediate display of result 
;
\ original >arabic function comes from rosetta code site.
\ here starts my own work : 
 
create romans 73 c, 86 c, 88 c, 76 c, 67 c, 68 c, 77 c,
\ here we go with a table of romans numbers chars

variable column# ( current-offset )
\ check char by char where to go 

: ones
	0 column# !
;

: tens
	2 column# !
;

: hundreds
	4 column# !
;

: thousands
	6 column# !
;

: column ( -- column-adr ) \ selection 
	romans column# @ +
;

: .symbol ( offset -- ) \ prints char
	column + c@ emit 
;

: oner
	0 .symbol
;

: fiver
	1 .symbol
;

: tener
	2 .symbol
;

: oners ( #of-ones -- ) \ select how many ones to print
	?dup if
		0 do
			oner
		loop
	then
;

: almost ( quotient-of-5/ -- ) \ select five or tens 
	oner if
		tener
	else
		fiver
	then
;

: digit ( digit -- ) 
	5 /mod over 
	4 = if 
		almost 
		drop
	else
		if
			fiver
		then
		oners
	then
;

: >roman ( number -- ) 
	1000 /mod	thousands 	digit
	100 /mod 	hundreds 	digit
	10 /mod 	tens		digit
			ones		digit
;

: bootmessage
	500 ms 
	page
	cr
	s" convert arabic > roman numbers : 	14   >roman" type cr
	s" convert roman > arabic numbers :	s“ XIV “ >arabic " type cr \ careful on ”“ char for string display
	s" type bye at ok prompt to exit" cr cr 
;

bootmessage
