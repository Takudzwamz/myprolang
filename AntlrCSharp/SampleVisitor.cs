using Antlr4.Runtime.Misc;
using AntlrCSharp.Content;

public class SampleVisitor: SampleBaseVisitor <object?>
{
    private Dictionary<string, object?> Variables { get;} = new(); //dictionary to store variables
    public SampleVisitor()
    {
        Variables.Add("pi", MathF.PI);
        Variables.Add("e", MathF.E);

        Variables["Write"]  =  new Func<object?[], object?>(Write);
    }

    private object? Write(object?[] args)
    {
        foreach (var arg in args)
        {
            Console.Write(arg);
        }

        return null;
    }

    public override object? VisitFunctionCall(SampleParser.FunctionCallContext context)
    {
        var functionName = context.IDENTIFIER().GetText();

        var args = context.expression().Select(Visit).ToArray();

        if (!Variables.ContainsKey(functionName))
            throw new Exception($"Function {functionName} does not exist");

        if (Variables[functionName] is not Func<object?[], object?> func)
           throw new Exception($"{functionName} is not a function");

        return func(args);
    }
    
    public override object? VisitAssignment(SampleParser.AssignmentContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

        Variables[varName] = value; //store variable in dictionary

        return null;
    }

    public override object? VisitIdentifierExpression(SampleParser.IdentifierExpressionContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        if (Variables.ContainsKey(varName))
        {
            throw new Exception($"Variable {varName} does not exist");
        }
            
        return Variables[varName];
    }

    public override object? VisitConstant(SampleParser.ConstantContext context)
    {
        if(context.INTEGER() is {} i)
            return int.Parse(i.GetText());

        if(context.FLOAT() is {} f)
            return float.Parse(f.GetText());

        if(context.STRING() is {} s)
            return s.GetText()[1..^1]; //remove quotes

        if (context.BOOL() is {} b)
            return b.GetText() == "true";

        if (context.NULL() is {})
            return null;
        
        throw new NotImplementedException();
        
    }

    public override object? VisitAdditiveExpression(SampleParser.AdditiveExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        var op = context.addOP().GetText();

        return op switch
        {
            "+" => Add(left, right),
            "-" => Subtract(left, right),
            _ => throw new NotImplementedException()
        };

    }

    private object? Add(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i + j;

        if (left is float f && right is float g)
            return f + g;
        
        if (left is int lint && right is float rf)
            return lint + rf;

        if (left is float lf && right is int rint)
            return lf + rint;

        if (left is string || right is string)
            return $"{left}{right}";

        throw new Exception($"Cannot add values of type {left?.GetType()} and {right?.GetType()}");
    }

    private object? Subtract(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i - j;

        if (left is float f && right is float g)
            return f - g;
        
        if (left is int lint && right is float rf)
            return lint - rf;

        if (left is float lf && right is int rint)
            return lf - rint;
        
        if (left is string || right is string)
            throw new Exception("Cannot subtract strings");

        throw new Exception($"Cannot subtract values of type {left?.GetType()} and {right?.GetType()}");
    }

    public override object? VisitMultiplicativeExpression(SampleParser.MultiplicativeExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        var op = context.multOP().GetText();

        return op switch
        {
            "*" => Multiply(left, right),
            "/" => Divide(left, right),
            _ => throw new NotImplementedException()
        };
    }

    private object? Multiply(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i * j;

        if (left is float f && right is float g)
            return f * g;
        
        if (left is int lint && right is float rf)
            return lint * rf;

        if (left is float lf && right is int rint)
            return lf * rint;
        
        if (left is string || right is string)
            throw new Exception("Cannot multiply strings");

        throw new Exception($"Cannot multiply values of type {left?.GetType()} and {right?.GetType()}");
    }

    private object? Divide(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i / j;

        if (left is float f && right is float g)
            return f / g;
        
        if (left is int lint && right is float rf)
            return lint / rf;

        if (left is float lf && right is int rint)
            return lf / rint;
        
        if (left is string || right is string)
            throw new Exception("Cannot divide strings");

        throw new Exception($"Cannot divide values of type {left?.GetType()} and {right?.GetType()}");
    }

    public override object? VisitWhileBlock(SampleParser.WhileBlockContext context)
    {
        Func<object?, bool> condition = context.WHILE().GetText() == "while"
            ? IsTrue
            : IsFalse
        ;

        if(condition(Visit(context.expression())))
        {
            do
            {
                Visit(context.block());
            } while (condition(Visit(context.expression())));
        }
        else
        {
            Visit(context.elseIfBlock());
        }

        return null;
    }

    private bool IsTrue(object? value)
    {
        if (value is bool b)
            return b;

        throw new Exception($"Cannot convert {value?.GetType()} to bool");
    }

    private bool IsFalse(object? value) => !IsTrue(value);

    public override object? VisitComparisonExpression(SampleParser.ComparisonExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        var op = context.compareOP().GetText();

        return op switch
        {
            "==" => Equal(left, right),
            "!=" => NotEqual(left, right),
            "<" => LessThan(left, right),
            ">" => GreaterThan(left, right),
            "<=" => LessThanOrEqual(left, right),
            ">=" => GreaterThanOrEqual(left, right),
            _ => throw new NotImplementedException()
        };
    }

    private bool Equal(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i == j;

        if (left is float f && right is float g)
            return f == g;
        
        if (left is int lint && right is float rf)
            return lint == rf;

        if (left is float lf && right is int rint)
            return lf == rint;
        
        if (left is string ls && right is string rs)
            return ls == rs;

        if (left is bool lb && right is bool rb)
            return lb == rb;

        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}");
    }

    private bool NotEqual(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i != j;

        if (left is float f && right is float g)
            return f != g;
        
        if (left is int lint && right is float rf)
            return lint != rf;

        if (left is float lf && right is int rint)
            return lf != rint;
        
        if (left is string ls && right is string rs)
            return ls != rs;

        if (left is bool lb && right is bool rb)
            return lb != rb;

        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}");
    }

    private bool LessThan(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i < j;

        if (left is float f && right is float g)
            return f < g;
        
        if (left is int lint && right is float rf)
            return lint < rf;

        if (left is float lf && right is int rint)
            return lf < rint;
        
        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}");
    }

    private bool GreaterThan(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i > j;

        if (left is float f && right is float g)
            return f > g;
        
        if (left is int lint && right is float rf)
            return lint > rf;

        if (left is float lf && right is int rint)
            return lf > rint;
    
        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}");
    }

    private bool LessThanOrEqual(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i <= j;

        if (left is float f && right is float g)
            return f <= g;
        
        if (left is int lint && right is float rf)
            return lint <= rf;

        if (left is float lf && right is int rint)
            return lf <= rint;
        
        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}");
    }

    private bool GreaterThanOrEqual(object? left, object? right)
    {
        if (left is int i && right is int j)
            return i >= j;

        if (left is float f && right is float g)
            return f >= g;
        
        if (left is int lint && right is float rf)
            return lint >= rf;

        if (left is float lf && right is int rint)
            return lf >= rint;
        
        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}");
    }
}