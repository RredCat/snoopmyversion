using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestApp
{
	static class Program
	{
		#region Main method
		/// <summary>
		/// Mains this instance.
		/// </summary>
		/// <remarks>string[] args</remarks>
		static void Main()
		{
			var inputList = new List<int> { 1, 2, 3 };
			var target = 7;
			//var list = new List<int> { 1, 3, 7, 10, 25, 50 };
			//var target = 765;
			//var list = new List<int> { 3, 19, 45, 21, 10, 4, 7 };
			//var target = 100;

			if ( inputList.Contains( target ) )
			{
				Console.WriteLine( "Input list contains target" );
				Console.ReadKey();
				return;
			}

			var sw = new Stopwatch();

			int count = inputList.Count;
			var operationList = new List<OperationMode> {
				OperationMode.Add,
				OperationMode.Sub,
				OperationMode.Mul,
				OperationMode.Div, };

			var inputVarinats = VariantsHelper.GetVariantsList( inputList );
			var operationVarinats = VariantsHelper.GetAllVariantsList( operationList, count );

			PrintInputVarinats(inputVarinats);
			PrintOperationVarinats(operationVarinats);

			var structure = GetStructure( count);
			var calculate = Caluclate(structure, inputVarinats, operationVarinats, target);

			if ( null == calculate )
			{
				sw.Stop();
				Console.WriteLine( "Not found.." );
			}
			else
			{
				var expression = calculate.GetExspretion();
				sw.Stop();
				Console.WriteLine( "Time: {0}", sw.ElapsedMilliseconds );
				Console.WriteLine( "Target {0}; Expression: {1};", target, expression );
				Console.ReadKey();
			}
		}
		#endregion

		#region Implementations
		private static List<KeyList<Calculate>> GetStructure( int depth )
		{
			var result = new List<KeyList<Calculate>>();
			
			var c21 = new Calculate();
			var c22 = new Calculate();
			var c2 = new Calculate( c21, c22 );

			var two = new KeyList<Calculate>
			{
				Key = 2,
				List = new List<Calculate> { c2 },
			};
			result.Add( two );

			var c31 = new Calculate();
			var c32 = new Calculate();
			var c33 = new Calculate();
			var c312 = new Calculate( c31, c32 );

			var c3a = new Calculate( c33, c312 );
			var c3b = new Calculate( c312, c33 );

			var three = new KeyList<Calculate>
			{
				Key = 3,
				List = new List<Calculate> { c3a, c3b },
			};
			result.Add( three );

			return result;
		}
		/// <summary>
		/// Prints the operation varinats.
		/// </summary>
		/// <param name="operationVarinats">The operation varinats.</param>
		private static void PrintOperationVarinats( IEnumerable<KeyList<List<OperationMode>>> operationVarinats )
		{
			foreach ( var res in operationVarinats )
			{
				Console.WriteLine( "Count {0}", res.Key );

				foreach ( var item in res.List )
				{
					foreach ( var it in item )
						Console.Write( "{0},", it );

					Console.WriteLine();
				}
			}
		}
		/// <summary>
		/// Prints the input varinats.
		/// </summary>
		/// <param name="inputVarinats">The input varinats.</param>
		private static void PrintInputVarinats( IEnumerable<IGrouping<int, List<int>>> inputVarinats )
		{
			foreach ( var res in inputVarinats )
			{
				Console.WriteLine( "Count {0}", res.Key );

				foreach ( var item in res )
				{
					foreach ( var it in item )
						Console.Write( "{0},", it );

					Console.WriteLine();
				}
			}
		}
		/// <summary>
		/// Caluclates the specified structure.
		/// </summary>
		/// <param name="structure">The structure.</param>
		/// <param name="inputVarinats">The input varinats.</param>
		/// <param name="operationVarinats">The operation varinats.</param>
		/// <param name="target">The target.</param>
		/// <returns></returns>
		private static Calculate Caluclate( IEnumerable<KeyList<Calculate>> structure
			, IEnumerable<IGrouping<int, List<int>>> inputVarinats
			, IEnumerable<KeyList<List<OperationMode>>> operationVarinats
			, int target )
		{
			foreach ( var list in structure )
			{
				var key = list.Key;
				var input = inputVarinats.Where( p => p.Key == key )
					.Select( p => p.ToList() ).ToList()[ 0 ];
				var operation = operationVarinats.Where( p => p.Key == key )
					.Select( p => p.List ).ToList()[ 0 ];

				foreach ( var calculate in list.List )
				{
					foreach ( var inp in input )
					{
						foreach ( var ope in operation )
						{
							var inpParam = new List<int>( inp );
							var opeParam = new List<OperationMode>( ope );
							calculate.CalculateValue( inpParam, opeParam );

							if ( calculate.IsCorrect
								&& calculate.Value == target )
							{
								return calculate;
							}
						}
					}
				}
			}

			return null;
		}
		#endregion
	}
}