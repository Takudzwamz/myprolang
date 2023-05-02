grammar Sample;

program: line* EOF;

line: statement | ifBlock | whileBlock;

statement: (assignment | functionCall) ';';

ifBlock: 'if'  expression  block ('else' elseIfBlock)?;

elseIfBlock: block | ifBlock;

whileBlock: WHILE expression block ('else' elseIfBlock);

WHILE: 'while' | 'until';

assignment: IDENTIFIER '=' expression;

functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')';

expression
    : constant                                  #constantExpression
    | IDENTIFIER                                #identifierExpression
    | functionCall                              #functionCallExpression
    | '(' expression ')'                        #parenthesizedExpression
    | '!' expression                            #notExpression
    | expression multOP expression              #multiplicativeExpression
    | expression addOP expression               #additiveExpression
    | expression compareOP expression           #comparisonExpression
    | expression boolOP expression              #booleanExpression
    ;

multOP: '*' | '/';
addOP: '+' | '-';
compareOP: '<' | '>' | '<=' | '>=' | '==' | '!=';
boolOP: 'and' | 'or' | 'xor';

constant: INTEGER | FLOAT | STRING | BOOL | NULL;


INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
NULL: 'null';

block: '{' line* '}';

WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;