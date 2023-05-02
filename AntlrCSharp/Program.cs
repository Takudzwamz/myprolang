using Antlr4.Runtime;
using AntlrCSharp.Content;

var fileName = "Content/test.ss";

var fileContents = File.ReadAllText(fileName);



var inputStream = new AntlrInputStream(fileContents);

var sampleLexer = new SampleLexer(inputStream);

var commonTokenStream = new CommonTokenStream(sampleLexer);

var sampleParser = new SampleParser(commonTokenStream);

var sampleContext = sampleParser.program();

var visitor = new SampleVisitor();  

visitor.Visit(sampleContext);