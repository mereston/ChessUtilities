grammar PortableGameNotation;

/*
 * Parser Rules
 
 */

pgnfile             : metadatasection NEWLINE movesection EOF;
metadatasection     : METATDATAATTR (NEWLINE METATDATAATTR)* ;
movesection         : movelist WHITESPACE SCORE ;
movelist            : move (WHITESPACE move)* ;
move                : movenumber (WHITESPACE | NEWLINE) halfmove (WHITESPACE | NEWLINE) halfmove? ;
halfmove            : HALFMOVE ;
movenumber          : MOVENUMBER ;

/*
 * Lexer Rules
 */
fragment FILE       : ('a'|'b'|'c'|'d'|'e'|'f'|'g'|'h') ;
fragment RANK       : [1-8] ;
fragment PAWN       : 'P' ;
fragment NONPAWN    : ('B'|'N'|'R'|'Q');
fragment KING       : 'K' ;
fragment CAPTURES   : 'x' ;
fragment CSTLKING   : ('0-0' | 'O-O') ;
fragment CSTLQUEEN  : ('0-0-0' | 'O-O-O');
fragment EVENTTAG   : 'Event' ;
fragment SITETAG    : 'Site' ;
fragment DATETAG    : 'Date' ;
fragment ROUNDTAG   : 'Round' ;
fragment COLORTAG   : ('White'|'Black') ;
fragment RESULTTAG  : 'Result' ;
fragment TMCTLTAG   : 'TimeControl' ;
fragment ELOTAG     : ('WhiteElo'|'BlackElo') ;
fragment TERMTAG    : 'Termination' ;
fragment CHECK      : '+' ;
fragment CHECKMATE  : '#' ;

HALFMOVE            : (PIECEMOVE | CASTLE) CHECK? ;
PAWNMOVESTUB        : PAWN? FILE PROMOTION? (CAPTURES FILE)? ;
PIECEMOVE           : (PAWNMOVESTUB | NONPAWNMOVESTUB | KINGMOVESTUB) RANK ;
PROMOTION           : (('=' | '/') NONPAWN) | (NONPAWN | '(' NONPAWN ')') ;
NONPAWNMOVESTUB     : NONPAWN FILE? RANK? CAPTURES? FILE ;
KINGMOVESTUB        : KING FILE CAPTURES? ;
TEXT                : '"' .*? '"' -> skip ;
WHITESPACE          : (' '|'\t')+ ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
METATDATAATTR       : '[' (EVENTTAG|SITETAG|DATETAG|ROUNDTAG|COLORTAG|RESULTTAG|TMCTLTAG|ELOTAG|TERMTAG) WHITESPACE TEXT ']' ;
MOVENUMBER          : [1-9][0-9]* '.';
CASTLE              : (CSTLKING | CSTLQUEEN) ;
SCORE               : ('1-0' | '0-1' | '.5-.5') ;