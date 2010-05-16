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
			var inputList = new List<int> { 1, 2, 3, 4 };
			//var list = new List<int> { 1, 3, 7, 10, 25, 50 };
			//var target = 765;
			var target = 20;
			//var list = new List<int> { 3, 19, 45, 21, 10, 4, 7 };
			//var target = 100;
			var sw = new Stopwatch();

			var operationList = new List<OperationMode> {
				OperationMode.Add,
				OperationMode.Sub,
				OperationMode.Mul,
				OperationMode.Div, };

			var inputVarinats = VariantsHelper.GetVariantsList( inputList );
			var operationVarinats = VariantsHelper.GetVariantsList( operationList );
			
			var i = GetResult( inputList, target );

			foreach ( var res in inputVarinats )
			{
				foreach ( var item in res )
					Console.Write( "{0},", item );
				Console.WriteLine();
			}

			foreach ( var res in operationVarinats )
			{
				foreach ( var item in res )
					Console.Write( "{0},", item );
				Console.WriteLine();
			}

			sw.Stop();
			Console.WriteLine( "Time: {0}", sw.ElapsedMilliseconds );
			Console.ReadKey();

		}
		#endregion

		#region Implementations
		private static object GetStructure( int depth )
		{
			return default( object );
		}

		private static int GetResult( List<int> list, int target )
		{
			return 0;
		}
		#endregion
	}
}