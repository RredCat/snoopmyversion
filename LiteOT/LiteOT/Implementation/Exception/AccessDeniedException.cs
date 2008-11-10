using System;

namespace LiteOT
{
	/// <summary>
	/// Presents exseption that throw if loggin or password is incorrect.
	/// </summary>
	public class AccessDeniedException : ApplicationException
	{
		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="AccessDeniedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public AccessDeniedException( string message )
			: base( message )
		{
		}
		#endregion
	} 
}
