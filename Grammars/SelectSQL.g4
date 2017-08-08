grammar SelectSQL;


/*
 * Parser Rules
 */

compileUnit
	:	sql
	;
	 

A:('A'|'a');
B:('B'|'b');
C:('C'|'c');
D:('D'|'d');
E:('E'|'e');
F:('F'|'f');
G:('G'|'g');
H:('H'|'h');
I:('I'|'i');
J:('J'|'j');
K:('K'|'k');
L:('L'|'l');
M:('M'|'m');
N:('N'|'n');
O:('O'|'o');
P:('P'|'p');
Q:('Q'|'q');
R:('R'|'r');
S:('S'|'s');
T:('T'|'t');
U:('U'|'u');
V:('V'|'v');
W:('W'|'w');
X:('X'|'x');
Y:('Y'|'y');
Z:('Z'|'z');
COMMA:',';
STAR: '*';
L_BRACK:'(';
R_BRACK:')';
DOT:'.';
STRING:'\'' .*? '\'';
NUMBER: '0'..'9';
UNDER_LINE:'_';

OPS_EQ:'=';
OPS_LT:'<';
OPS_LET:'<=';
OPS_GT:'>';
OPS_GET:'>=';
OPS_NEQ:('!='|'<>'|'><');


SELECT: S E L E C T;
FROM: F R O M;
WHERE: W H E R E;
ORDERBY: O R D E R ' ' B Y;
AS: A S;
ORDERBY_ASC:A S C;
ORDERBY_DESC:D E S C;
AND_OR: (A N D|O R);
NOLOCK: N O L O C K;
READPAST: R E A D P A S T;


FUNCTIONS_PREFIX
	: C O U N T
	| M A X
	| M I N
	| S U M
	| A V G
	;
COLUMN_PREDICT
	: D I S T I N C T
	;

sql
	:select from? where? orderby?
	;

select
	:SELECT COLUMN_PREDICT? columns
	;

columns
	:column (COMMA column)*
	;

column
	: columnExpression (AS columnName)?
	;

columnExpression
	:functionableColumn
	|columnName
	;

functionableColumn
	:FUNCTIONS_PREFIX L_BRACK (columnName|NUMBER|STAR) R_BRACK
	;

columnName
	:identity
	|number
	|STAR
	;

from
	:FROM table tableLockType?
	;

tableLockType
	: L_BRACK NOLOCK R_BRACK
	| L_BRACK READPAST R_BRACK
	;

table
	: identity
	;

where
	:WHERE whereStmts
	;	
	
orderby
	:ORDERBY orderByStmts
	;
	
expression
	:identity
	;


identity
	:character (character|NUMBER|UNDER_LINE)*
	;
character
	:(A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z)
	;

number
	:NUMBER (NUMBER|DOT)*
	;
string
	:STRING
	;

orderByStmts
	: orderByStmt (COMMA orderByStmt)*
	;
	
orderByStmt
	:columnExpression orderByDirection?
	;

orderByDirection
	: ORDERBY_ASC
	| ORDERBY_DESC
	;
	
	
whereStmts
	: whereStmt
	;


whereStmt
	:	whereStmt AND_OR whereStmt
	|	whereCondition
	|	L_BRACK whereStmt R_BRACK
	;
	
whereCondition
	: comparableValue operators comparableValue
	;
	
operators
	:OPS_EQ
	|OPS_LT
	|OPS_LET
	|OPS_GT
	|OPS_GET
	|OPS_NEQ
	;
	
comparableValue
	:columnExpression
	|number
	|string
	;
	
	
	
	
	
	
	
	