using Antlr4.Runtime;
using AntlrCSharp;
using AntlrCSharp.Content;

//var fileName = "Content\\testfunction.ss";
//var fileName = "Content\\test.ss";
var fileName = "Content\\test2.ss";

var fileContents = File.ReadAllText(fileName);

var inputStream = new AntlrInputStream(fileContents);

var sampleLexer = new SampleLexer(inputStream);

var commonTokenStream = new CommonTokenStream(sampleLexer);

var sampleParser = new SampleParser(commonTokenStream);

var sampleContext = sampleParser.program();

var visitor = new SampleVisitor(new Dictionary<string, object?>(), new Dictionary<string, Func<object?[], object?>>());

visitor.Visit(sampleContext);