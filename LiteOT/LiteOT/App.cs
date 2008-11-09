using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AlternativeOT
{
	/// <summary>
	/// Presents aplication.
	/// </summary>
	public class App
	{
		#region Constants
		private const string CONNECTION_PATH = @"Data Source={0};Persist Security Info=True;User ID={1};Password={2}";
		#endregion

		#region Implementation
		/// <summary>
		/// Mains this instance.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			const String m_ServerName = "www3.syncfusion.com,8754";
			const String m_DBUserName = "ontime_es_user";
			const String m_DBPassword = "esiscool";

			const String m_LoginID = "taras.romanik";
			const String m_Password = "taras.romanik";

			OTDataDataContext data = new OTDataDataContext( GetConnectString( m_ServerName, m_DBUserName, m_DBPassword ) );
			bool isUnAauthorized = true;

			while( isUnAauthorized )
			{
				AccessWindow dialogWin = new AccessWindow();
				dialogWin.ShowDialog();

				if( true == dialogWin.DialogResult )
				{
					try
					{
						Int32 userId = GetUserId( data, m_LoginID, m_Password );
						isUnAauthorized = false;

						MainWindow w = new MainWindow(data, userId );
						w.ShowDialog();
					}
					catch( AccessDeniedException )
					{
					}
				}
				else
					break;
			}
		}
		private static Int32 GetUserId( OTDataDataContext data, String loginID, String password )
		{
			var result = from user in data.Users
					where user.LoginId == loginID && user.Password == GetEncryptedPassword( password )
					select user.UserId;

			if( 1 == result.Count() )
			{
				foreach( int userId in result )
				{
					return userId;
				}
			}

			throw new AccessDeniedException( "Incorrect loggin or password" );
		}

		private static string GetConnectString( String serverName, String dbUserId, string dbPassword )
		{
			return string.Format( CONNECTION_PATH, serverName, dbUserId, dbPassword );
		}

		private static string GetEncryptedPassword( String password )
		{
			byte[] buffer;

			using( MD5 md5 = new MD5CryptoServiceProvider() )
			{
				byte[] bytes = Encoding.UTF8.GetBytes( password );
				buffer = md5.ComputeHash( bytes );
			}

			return Convert.ToBase64String( buffer );
		}
		#endregion
	}
}
