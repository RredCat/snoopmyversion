using System;
using System.Security.Cryptography;
using System.Text;

namespace LiteOT
{
	/// <summary>
	/// Presents helper class.
	/// </summary>
	public static class Utility
	{
		#region Helper methods
		/// <summary>
		/// Gets the encrypted password.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public static string GetEncryptedPassword( String password )
		{
			byte[] buffer;

			using( MD5 md5 = new MD5CryptoServiceProvider() )
			{
				byte[] bytes = Encoding.UTF8.GetBytes( password );
				buffer = md5.ComputeHash( bytes );
			}

			return Convert.ToBase64String( buffer );
		}
		/// <summary>
		/// Casts the specified obj.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static T Cast<T>( object obj, T type )
		{
			return (T)obj;
		}
		#endregion
	}
}
