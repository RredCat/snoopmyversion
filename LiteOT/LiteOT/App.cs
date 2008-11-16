using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace LiteOT
{
	/// <summary>
	/// Presents aplication.
	/// </summary>
	public class App
	{
		#region Implementation
		/// <summary>
		/// Mains this instance.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			bool isUnAauthorized = true;

			while( isUnAauthorized )
			{
				AccessWindow window = new AccessWindow();
				window.ShowDialog();

				if( true == window.DialogResult )
				{
					try
					{
						OTDataDataContext data = new OTDataDataContext(window.GetConnectString() );
						Int32 userId = GetUserId( data, window.Login, window.Password);
						isUnAauthorized = false;

						MainWindow mainWindow = new MainWindow( data, userId );
						mainWindow.ShowDialog();
					}
					catch( AccessDeniedException )
					{
					}
					catch( SqlException )
					{
						MessageBox.Show("SQL exception, try later", "):");
					}
				}
				else
					break;
			}
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
						 where user.LoginId == loginID && user.Password == Utility.GetEncryptedPassword( password )
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
		#endregion
	}
}