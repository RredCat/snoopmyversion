using System;

namespace Miracle.Silverlight.Themes
{
	public class PairKeyValue
	{
		#region Constants
		/// <summary>
		/// Presents exception's message.
		/// </summary>
		private const string c_EXCEPTION_MESSAGE = "Key variable not must be null";
		#endregion

		#region Private members
		/// <summary>
		/// Key for search target.
		/// </summary>
		private string m_key;
		#endregion

		#region Public properties
		/// <summary>
		/// Gets or sets the key in key-value pair.
		/// </summary>
		public string Key
		{
			get
			{
				return m_key;
			}
			set
			{
				if ( null == value )
				{
					throw new ArgumentNullException( c_EXCEPTION_MESSAGE );
				}

				m_key = value;
			}
		}
		/// <summary>
		/// Gets or sets the value in key-value pair.
		/// </summary>
		public object Value
		{
			get;
			set;
		}
		#endregion
	}
}