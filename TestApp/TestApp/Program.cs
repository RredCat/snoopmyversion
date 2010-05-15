using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestApp
{
	class Program
	{
		/// <summary>
		/// Mains this instance.
		/// </summary>
		/// <remarks>string[] args</remarks>
		static void Main()
		{
			var list = new List<int> { 1, 2,3 };
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

		private static List<KeyBlock> GetLists( List<int> list )
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

		private static KeyBlock GetNewKeyBlock( int value, KeyBlock replication )
		{
			var result = new KeyBlock();

			foreach ( var list in replication.List )
			{
				var temp = new List<int>();
				temp.AddRange( list.List );
				
				int key = list.Key;
				temp.RemoveAt( key );
				temp.Insert( key, value );
				
				result.List.Add( new KeyList
				{
					Key = key,
					List = temp,
				} );
			}

			var higerList = new List<KeyList>();

			foreach ( var list in replication.List )
			{
				var keyList = new KeyList();
				keyList.List.Add( value );
				keyList.List.AddRange( list.List );
				higerList.Add( keyList );
			}

			result.List.AddRange( higerList );

			var higerMixList = new List<KeyList>();

			foreach ( var list in replication.List )
			{
				var temp = new List<int>();
				temp.AddRange( list.List );

				int key = list.Key;
				var keyValue = temp[ key ];
				temp.RemoveAt( key );
				temp.Insert( key, value );

				var keyList = new KeyList
				{
					Key=++key,
				};
				keyList.List.Add( keyValue );
				keyList.List.AddRange( temp );

				higerMixList.Add( keyList );
			}

			result.List.AddRange( higerMixList );

			return result;
		}

		private static void XXXX(List<List<int>> result,IEnumerable<int> list,  int step)
		{
			var first = list.Take( step );
			var nexts = list.Skip( step );

			foreach ( int i in nexts )
			{
				var temp1 = new List<int>();
				temp1.AddRange( first );
				temp1.Add( i );
				result.Add( temp1 );

				if ( 0 != first.Count() )
				{
					var temp2 = new List<int> { i };
					temp2.AddRange( first );
					result.Add( temp2 );
				}

				if ( nexts.Count() != step )
				{
					XXXX( result, nexts, ++step );
				}
			}
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
	}
}