namespace SharpLox;
abstract record Expr
{
	public abstract T Accept<T>(Visitor<T> visitor);

	internal record Binary(Expr Left, Token Opr, Expr Right) : Expr
	{
		public override T Accept<T>(Visitor<T> visitor)
		{
			return visitor.VisitBinaryExpr(this);
		}
	}

	internal record Grouping(Expr Expression) : Expr
	{
		public override T Accept<T>(Visitor<T> visitor)
		{
			return visitor.VisitGroupingExpr(this);
		}
	}

	internal record Literal(object? Value) : Expr
	{
		public override T Accept<T>(Visitor<T> visitor)
		{
			return visitor.VisitLiteralExpr(this);
		}
	}

	internal record Unary(Token Opr, Expr Right) : Expr
	{
		public override T Accept<T>(Visitor<T> visitor)
		{
			return visitor.VisitUnaryExpr(this);
		}
	}


	public interface Visitor<T>
	{
		T VisitBinaryExpr(Binary expr);
		T VisitGroupingExpr(Grouping expr);
		T VisitLiteralExpr(Literal expr);
		T VisitUnaryExpr(Unary expr);
	}
}
