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
			//var inputList = new List<int> { 1, 2, };
			//var target = 3;
			//var inputList = new List<int> { 1, 2, 3, };
			//var target = 7;
			//var inputList = new List<int> { 1, 2, 3, 4, };
			//var target = 21;
			//var inputList = new List<int> { 1, 3, 7, 10, 25, 50, };
			//var target = 765;
			var inputList = new List<int> { 3, 19, 45, 21, 10, 4, 7, };
			var target = 100;

			if ( 2 > inputList.Count )
			{
				Console.WriteLine( "Input list is too short" );
				Console.ReadKey();
				return;
			}

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
			var structure = CalculateHelper.GetStructure( count );
			var calculate = Caluclate(structure, inputVarinats, operationVarinats, target);

			if ( null == calculate )
			{
				sw.Stop();
				Console.WriteLine( "Time: {0}", sw.ElapsedMilliseconds );
				Console.WriteLine( "Not found.." );
				Console.ReadKey();
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