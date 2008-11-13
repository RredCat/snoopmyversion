using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LiteOT
{
	/// <summary>
	/// Presents aplication.
	/// </summary>
	public class App
	{
		#region Constants
		private const string CONNECTION_PATH = @"Data Source={0};Persist Security Info=True;User ID={1};Password={2}";
		//  connectionString="Data Source=www3.syncfusion.com,8754;
		//Initial Catalog=ontime_es;Persist Security Info=True;User ID=ontime_es_user"
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

			//while( isUnAauthorized )
			//{
			//    AccessWindow dialogWin = new AccessWindow();
			//    dialogWin.ShowDialog();

			//    if( true == dialogWin.DialogResult )
			//    {
			//        try
			//        {
			Int32 userId = GetUserId( data, m_LoginID, m_Password );
			isUnAauthorized = false;

			MainWindow mainWindow = new MainWindow( data, userId );
			mainWindow.ShowDialog();
			//        }
			//        catch( AccessDeniedException )
			//        {
			//        }
			//    }
			//    else
			//        break;
			//}
		}
		/// <summary>
		/// Gets the user id.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="loginID">The login ID.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		private static Int32 GetUserId( OTDataDataContext data, String loginID, String password )
		{
			var result = from user in data.Users
						 where user.LoginId == loginID && user.Password == GetEncryptedPassword( password )
						 select user.UserId;
			int count = result.Count();

			if( 1 == count )
			{
				foreach( int userId in result )
				{
					return userId;
				}
			}
			else if( 1 < count )
			{
				throw new ApplicationException( "Double row in data base" );
			}

			throw new AccessDeniedException( "Incorrect loggin or password" );
		}
		/// <summary>
		/// Gets the connect string.
		/// </summary>
		/// <param name="serverName">Name of the server.</param>
		/// <param name="dbUserId">The db user id.</param>
		/// <param name="dbPassword">The db password.</param>
		/// <returns></returns>
		private static string GetConnectString( String serverName, String dbUserId, string dbPassword )
		{
			return string.Format( CONNECTION_PATH, serverName, dbUserId, dbPassword );
		}
		/// <summary>
		/// Gets the encrypted password.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <returns></returns>
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
