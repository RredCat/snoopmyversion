using System.Collections.Generic;
using System.Linq;

namespace TestApp
{
	public static class VariantsHelper
	{
		#region Public methods
		/// <summary>
		/// Gets the variants list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static List<IGrouping<int, List<T>>> GetVariantsList<T>( IList<T> list )
		{
			var result = new List<List<T>>();
			var resultExpr = ( from lists in GetLists( list )
							   select ( from keyList in lists
										select keyList.List ).ToList() ).ToList();

			foreach ( var exp in resultExpr )
			{
				result.AddRange( exp );
			}

			return result.GroupBy( fuckList => fuckList.Count ).ToList();
		}
		/// <summary>
		/// Gets all variants list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="depth">The depth.</param>
		/// <returns></returns>
		public static List<KeyList<List<T>>> GetAllVariantsList<T>( List<T> list, int depth )
		{
			var result = new List<KeyList<List<T>>>();
			var currentResult = new List<List<T>>();

			foreach (var item in list)
			{
				currentResult.Add( new List<T> { item } );
			}
			
			int key = 2;
			result.Add( new KeyList<List<T>>
			{
				Key = key,
				List = currentResult,
			} );
			--depth;

			while ( 0 < --depth )
			{
				var previousResult = new List<List<T>>();
				previousResult.AddRange( currentResult );
				currentResult = new List<List<T>>();

				foreach ( var item in list )
				{
					foreach ( var res in previousResult )
					{
						var temp = new List<T> { item };
						temp.AddRange( res );
						currentResult.Add( temp );
					}
				}
				
				result.Add( new KeyList<List<T>>
				{
					Key = ++key,
					List = currentResult,
				} );
			}

			return result;
		}
		#endregion

		#region Impelmentations
		/// <summary>
		/// Gets the lists.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		private static List<List<KeyList<T>>> GetLists<T>( IList<T> list )
		{
			var result = new List<List<KeyList<T>>>();
			var first = new List<KeyList<T>>
			{
				new KeyList<T>
				{
					Key = 0,
					List = new List<T>{list[0]},
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
		private static List<KeyList<T>> GetNewKeyBlock<T>( T value, IEnumerable<KeyList<T>> replication )
		{
			var result = new List<KeyList<T>>();

			foreach ( var list in replication )
			{
				PreparePrevisuosCase( list, value, result );
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
		private static void PrepareMixedIncreaseCase<T>( KeyList<T> list, T value
			, ICollection<KeyList<T>> result )
		{
			foreach ( var item in list.List )
			{
				var mixedList = new List<T>();
				mixedList.AddRange( list.List );

				int key = list.List.IndexOf( item );
				var keyValue = item;
				mixedList.RemoveAt( key );
				mixedList.Insert( key, value );

				var keyList = new KeyList<T>
				{
					Key = ++key,
				};
				keyList.List.Add( keyValue );
				keyList.List.AddRange( mixedList );

				result.Add( keyList );
			}
		}
		/// <summary>
		/// Prepares the increase case.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		private static void PrepareIncreaseCase<T>( KeyList<T> list, T value
			, ICollection<KeyList<T>> result )
		{
			var genericKeyList = new KeyList<T>();
			genericKeyList.List.Add( value );
			genericKeyList.List.AddRange( list.List );
			result.Add( genericKeyList );
		}
		/// <summary>
		/// Prepares the previsuos case.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		private static void PreparePrevisuosCase<T>( KeyList<T> list, T value
			, ICollection<KeyList<T>> result )
		{
			var tempList = new List<T>();
			tempList.AddRange( list.List );

			int key = list.Key;
			tempList.RemoveAt( key );
			tempList.Insert( key, value );

			result.Add( new KeyList<T>
			{
				Key = key,
				List = tempList,
			} );
		}
		#endregion
	}
}