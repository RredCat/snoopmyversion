using System.Collections.Generic;

namespace TestApp
{
	public class KeyList
	{
		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyList"/> class.
		/// </summary>
		public KeyList()
		{
			Key = 0;
			List = new List<int>();
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
		public List<int> List { get; set; }
		#endregion
	}
}