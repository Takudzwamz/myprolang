grammar Sample;

program: line* EOF;

line: statement | ifBlock | whileBlock| forBlock | switchBlock;

statement: (assignment | functionCall) ';';

forBlock: 'for' '(' (assignment)? ';' expression? ';' assignment? ')' block;

switchBlock: 'switch' expression '{' switchCase* ('default' ':' block)? '}';

switchCase: 'case' expression ':' block;

ifBlock: 'if'  expression  block ('else' elseIfBlock)?;

elseIfBlock: block | ifBlock;

whileBlock: WHILE expression block ('else' elseIfBlock);

WHILE: 'while' | 'until';

assignment: IDENTIFIER '=' expression;

functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')';

functionDefinition: 'function' IDENTIFIER '(' (parameterList)? ')' block;

parameterList: IDENTIFIER (',' IDENTIFIER)*;

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
boolOP: 'and' | 'or';

constant: INTEGER | FLOAT | STRING | BOOL | NULL;


INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
NULL: 'null';

COMMENT: '//' ~[\r\n]* -> skip;

block: '{' line* '}';

WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;