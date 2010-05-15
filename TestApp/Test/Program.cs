using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Calculator
{
	public class Expression<T>
	{
		public T Value;
		public string Str;

		public static IEqualityComparer<Expression<T>> DefaultComparer()
		{
			return new ExpressionComparer();
		}

		public override bool Equals( object obj )
		{
			if ( ReferenceEquals( this, obj ) )
				return true;

			Expression<T> exp = obj as Expression<T>;
			if ( exp == null )
				return false;

			return Str == exp.Str;
		}

		public override int GetHashCode()
		{
			return Str.GetHashCode();
		}

		public class ExpressionComparer : IEqualityComparer<Expression<T>>
		{
			public bool Equals( Expression<T> x, Expression<T> y )
			{
				return x.Str == y.Str;
			}

			public int GetHashCode( Expression<T> obj )
			{
				return obj.Str.GetHashCode();
			}
		}
	}

	public class EnumerableComparer<T> : IEqualityComparer<IEnumerable<T>>
	{
		public bool Equals( IEnumerable<T> x, IEnumerable<T> y )
		{
			return x.OrderBy( el => el )
				.SequenceEqual( y.OrderBy( el => el ) );
		}

		public int GetHashCode( IEnumerable<T> obj )
		{
			int hash = 0;
			foreach ( T el in obj )
				hash += el.GetHashCode();
			return hash;
		}
	}

	public static class Program
	{
		static void Main()
		{
			List<decimal> availablenumbers =
				new List<decimal> { 3, 19, 45, 21, 10, 4, 7 };

			List<char> availableoperators =
				new List<char> { '+', '-', '*', '/' };

			int cachenumber = 5;
			int targetvalue = 100;

			var cache =
				new Dictionary<IEnumerable<decimal>, List<Expression<decimal>>>(
					new EnumerableComparer<decimal>() );

			Func<IEnumerable<decimal>, List<Expression<decimal>>> getfromcache =
				numberset =>
				{
					List<Expression<decimal>> expressions;
					cache.TryGetValue( numberset, out expressions );
					return expressions;
				};

			Func<IEnumerable<decimal>, IEnumerable<Expression<decimal>>,
				IEnumerable<Expression<decimal>>> addtocache =
				( numberset, expressions ) =>
				{
					if ( numberset.Count() <= cachenumber )
					{
						var cached = expressions.ToList();
						cache.Add( numberset, cached );
						return cached;
					}

					return expressions;
				};

			Func<char, decimal, decimal, decimal> calculate =
				( op, left, right ) =>
				{
					try
					{
						switch ( op )
						{
							case '+':
								return left + right;
							case '-':
								return left - right;
							case '*':
								return left * right;
							case '/':
								return right != 0 ?
								left / right : decimal.MaxValue;
							default:
								return 0;
						}
					}
					catch ( OverflowException )
					{
						return decimal.MaxValue;
					}
				};

			Predicate<decimal> validate =
				value =>
					value > 0 &&
					value != decimal.MaxValue &&
					Math.Truncate( value ) == value;

			Func<char, string, string, string> makeexpression =
				( op, leftex, rightex ) => "(" + leftex + op + rightex + ")";

			Func<IEnumerable<decimal>, IEnumerable<Expression<decimal>>>
				build = numberlist =>
					ExpressionBuilder( numberlist,
						calculate, validate, makeexpression,
						getfromcache, addtocache,
						availableoperators );

			ulong totalcount = 0;
			ulong filteredcount = 0;

			Stopwatch sw = new Stopwatch();
			sw.Start();

			availablenumbers
				.Distinct()
				.Universe()
				.WithoutEmptySet()
				.OrderBy( ie => ie.Count() )
				.SelectMany( build )
				.ForEach( ( el, num ) => totalcount += 1 )
				.Where( ex => ex.Value == targetvalue )
				.ForEach( ( el, num ) => filteredcount += 1 )
				.ForEach( ( el, num ) =>
					Console.WriteLine( num + ": " + el.Str + " = " + el.Value ) )
				.First();

			sw.Stop();

			Console.WriteLine( "Total: " + totalcount );
			Console.WriteLine( "Filtered total: " + filteredcount );
			Console.WriteLine( "Time: " + sw.ElapsedMilliseconds.ToString( "N" ) );
		} // Main

		public static IEnumerable<T> ForEach<T>(
			this IEnumerable<T> source, Action<T, int> func )
		{
			int counter = 1;
			foreach ( var item in source )
			{
				func( item, counter++ );
				yield return item;
			}
		} // ForEach

		private static IEnumerable<Expression<T>> ExpressionBuilder<T>(
			IEnumerable<T> list,
			Func<char, T, T, T> calculate,
			Predicate<T> validate,
			Func<char, string, string, string> makeexpression,
			Func<IEnumerable<T>, List<Expression<T>>> getfromcache,
			Func<IEnumerable<T>, IEnumerable<Expression<T>>,
				IEnumerable<Expression<T>>> addtocache,
			List<char> operators )
		{
			Debug.Assert( list != null );
			Debug.Assert( list.Count() != 0 );

			var precalculated = getfromcache( list );
			if ( precalculated != null )
				return precalculated;

			if ( list.Count() == 1 )
			{
				var res =
					new List<Expression<T>> {
                        new Expression<T> {
                        Value = list.First(),
                        Str = list.First().ToString()
                    }}
					.Where( el => validate( el.Value ) );
				addtocache( list, res );
				return res;
			}
			var expressions = list
				.Universe()
				.WithoutEmptySet()
				.WithoutMaxSet()
				.Select( ie => new
				{
					LeftNumbers = ie,
					RightNumbers = list.Except( ie ),
				} )
				.Select( arg => new
				{
					LeftExpressions = ExpressionBuilder( arg.LeftNumbers,
						calculate, validate, makeexpression,
						getfromcache, addtocache, operators ),
					RightExpressions = ExpressionBuilder( arg.RightNumbers,
						calculate, validate, makeexpression,
						getfromcache, addtocache, operators )
				} )
				.SelectMany( left => left.LeftExpressions.Select( l =>
					new
					{
						LeftValue = l,
						RightExpressions = left.RightExpressions
					} ) )
				.SelectMany( right => right.RightExpressions.Select( r =>
					new
					{
						LeftValue = right.LeftValue,
						RightValue = r
					} ) )
				.SelectMany( operands =>
					operators.Select( @operator =>
						new Expression<T>
						{
							Value = calculate( @operator,
								operands.LeftValue.Value, operands.RightValue.Value ),
							Str = makeexpression( @operator,
								operands.LeftValue.Str, operands.RightValue.Str )
						} ) )
				.Where( el => validate( el.Value ) );

			return addtocache( list, expressions );

		} // ExpressionBuilder

		private static IEnumerable<IEnumerable<T>> Universe<T>(
			this IEnumerable<T> numberset )
		{
			Debug.Assert( numberset != null );

			if ( numberset.Count() == 0 )
				return new List<IEnumerable<T>> { new List<T>() };

			IEnumerable<T> head = numberset.Take( 1 );
			IEnumerable<T> tail = numberset.Skip( 1 );
			IEnumerable<IEnumerable<T>> subsets = tail.Universe();

			return subsets.Concat(
				   from ie in subsets
				   select head.Concat( ie ).OrderBy( el => el ).AsEnumerable()
				   );
		} 

		private static IEnumerable<IEnumerable<T>> WithoutEmptySet<T>(
			this IEnumerable<IEnumerable<T>> set )
		{
			return set.Where( ie => ie.Count() != 0 );
		}

		private static IEnumerable<IEnumerable<T>> WithoutMaxSet<T>(
			this IEnumerable<IEnumerable<T>> set )
		{
			var max = set.Max( ie => ie.Count() );
			return set.Where( ie => ie.Count() != max );
		}
	}
}
