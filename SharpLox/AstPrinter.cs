using System.Text;

namespace SharpLox;

internal class AstPrinter : Expr.Visitor<string>
{
    public string Print(Expr expr) => expr.Accept(this);

    public string VisitBinaryExpr(Expr.Binary expr)
    {
        return Paranthesize(expr.Opr.Lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
        return Paranthesize("group", expr.Expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr)
    {
        // TODO: Fix this nullable thing
        return expr.Value is null ? "nil" : expr.Value.ToString()!;
    }

    public string VisitUnaryExpr(Expr.Unary expr)
    {
        return Paranthesize(expr.Opr.Lexeme, expr.Right);
    }

    private string Paranthesize(string name, params Expr[] exprs)
    {
        var builder = new StringBuilder();
        builder.Append('(')
            .Append(name);

        foreach (var expr in exprs)
        {
            builder.Append(' ')
                .Append(expr.Accept(this));
        }

        builder.Append(')');
        return builder.ToString();
    }
}
