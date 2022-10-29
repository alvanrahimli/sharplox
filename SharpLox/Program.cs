using SharpLox;

Console.WriteLine("Welcome to SharpLox transpiler");

var transpiler = new SharpLox.SharpLox();

if (args.Length > 1)
{
    Console.WriteLine("Usage: slox [script]");
}
else if (args.Length == 1)
{
    //transpiler.RunFile(args[0]);

    var expr = new Expr.Binary(
        new Expr.Unary(new Token(TokenType.MINUS, "-", null, 1), new Expr.Literal(15)),
        new Token(TokenType.STAR, "*", null, 2),
        new Expr.Grouping(new Expr.Literal("salam")));
    Console.WriteLine(new AstPrinter().Print(expr));
}
else
{
    transpiler.RunPrompt();
}