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
			var list = new List<int> { 1, 2, 3 };
			//var list = new List<int> { 1, 3, 7, 10, 25, 50 };
			var target = 765;
			//var list = new List<int> { 3, 19, 45, 21, 10, 4, 7 };
			//var target = 100;
			var sw = new Stopwatch();

			var list1 = GetLists( list ).Select( p => p.List ).ToList();
			var l = new List<List<int>>();
			//l.Concat<List<int>>( (from n in list1
			//        select n.))

			var i = GetResult( list, target );

			//foreach (var list1 in lists)
			//{
			//    foreach ( var li in list1 )
			//        Console.Write( "{0},",li );
			//    Console.WriteLine();
			//}

			sw.Stop();
			Console.WriteLine( "Time: {0}", sw.ElapsedMilliseconds );
			Console.ReadKey();

		}
		#endregion

		#region Implementations
		/// <summary>
		/// Gets the lists.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		private static List<KeyBlock> GetLists( IList<int> list )
		{
			var result = new List<KeyBlock>();
			var first = new KeyBlock
			{
				List = new List<KeyList>
				{
					new KeyList
					{
						Key = 0,
						List = new List<int>{list[0]},
					},
				},
			};
			result.Add( first );
			var next = first;

			for ( int i = 1; i < list.Count; ++i )
			{
				var temp = GetNewKeyBlock( list[ i ], next );
				next = temp;
				result.Add( temp );
			}

			return result;
		}
		/// <summary>
		/// Gets the new key block.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="replication">The replication.</param>
		/// <returns></returns>
		private static KeyBlock GetNewKeyBlock( int value, KeyBlock replication )
		{
			var result = new KeyBlock();

			foreach ( var list in replication.List )
			{
				PrepareCommonCase( list, value, result );
				PrepareIncreaseCase( list, value, result );
				PrepareMixedIncreaseCase( list, value, result );
			}

			return result;
		}
		/// <summary>
		/// Prepares the mixed increase case.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		private static void PrepareMixedIncreaseCase( KeyList list, int value, KeyBlock result )
		{
			foreach ( var item in list.List )
			{
				var mixedList = new List<int>();
				mixedList.AddRange( list.List );

				int key = list.List.IndexOf( item );
				var keyValue = item;
				mixedList.RemoveAt( key );
				mixedList.Insert( key, value );

				var keyList = new KeyList
								{
									Key = ++key,
								};
				keyList.List.Add( keyValue );
				keyList.List.AddRange( mixedList );

				result.List.Add( keyList );
			}
		}
		/// <summary>
		/// Prepares the increase case.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		private static void PrepareIncreaseCase( KeyList list, int value, KeyBlock result )
		{
			var genericKeyList = new KeyList();
			genericKeyList.List.Add( value );
			genericKeyList.List.AddRange( list.List );
			result.List.Add( genericKeyList );
		}
		/// <summary>
		/// Prepares the common case.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		private static void PrepareCommonCase( KeyList list, int value, KeyBlock result )
		{
			var tempList = new List<int>();
			tempList.AddRange( list.List );

			int key = list.Key;
			tempList.RemoveAt( key );
			tempList.Insert( key, value );

			result.List.Add( new KeyList
								{
									Key = key,
									List = tempList,
								} );
		}

		private static int GetResult( List<int> list, int target )
		{
			var last = list.Count - 1;

			//int result = list[ last ] * list[ last - 1 ];



			//if ( result < target )
			//{
			//}
			//else if ( result > target )
			//{
			//}
			//else
			//{
			//    return result;
			//}

			return 0;
		}
		#endregion
	}
}