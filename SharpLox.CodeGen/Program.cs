using SharpLox.CodeGen;

Console.WriteLine("Generating source code");

Generator.DefineAst2("./../../../../SharpLox/", "Expr", new List<string>()
{
    "Binary   : Expr Left, Token Opr, Expr Right",
    "Grouping : Expr Expression",
    "Literal  : object? Value",
    "Unary    : Token Opr, Expr Right"
});

Console.WriteLine("DONE!");
