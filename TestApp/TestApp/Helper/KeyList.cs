using System.Collections.Generic;

namespace TestApp
{
	public class KeyList<T>
	{
		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyList&lt;T&gt;"/> class.
		/// </summary>
		public KeyList()
		{
			Key = 0;
			List = new List<T>();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
		public int Key
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the list.
		/// </summary>
		/// <value>The list.</value>
		public List<T> List
		{
			get;
			set;
		}
		#endregion
	}
}