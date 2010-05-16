using System.Collections.Generic;
using System.Linq;

namespace TestApp
{
	public static class CalculateHelper
	{
		#region Public methods
		/// <summary>
		/// Gets the structure.
		/// </summary>
		/// <param name="depth">The depth.</param>
		/// <returns></returns>
		public static List<KeyList<Calculate>> GetStructure( int depth )
		{
			var result = new List<KeyList<Calculate>>();

			for ( int i = 1; i < depth; ++i )
			{
				var keyList = new KeyList<Calculate>
				{
					Key = i + 1,
					List = GetAllTrees( i ),
				};
				result.Add( keyList );
			}

			return result;
		}
		#endregion

		#region Implemenations
		/// <summary>
		/// Gets all variants of trees.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		private static List<Calculate> GetAllTrees( int size )
		{
			if ( size == 0 )
				return new List<Calculate> { null };
			return ( from i in Enumerable.Range( 0, size )
					 from first in GetAllTrees( i )
					 from second in GetAllTrees( size - 1 - i )
					 select new Calculate( first, second ) ).ToList();
		}
		#endregion
	}
}