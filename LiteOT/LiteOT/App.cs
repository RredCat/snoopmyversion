using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Net.Mail;

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
			try
			{
				bool isUnAauthorized = true;

				while (isUnAauthorized)
				{
					AccessWindow window = new AccessWindow();
					window.ShowDialog();

					if (true == window.DialogResult)
					{
						try
						{
							OTDataDataContext data = new OTDataDataContext( window.GetConnectString() );
							Int32 userId = GetUserId( data, window.Login, window.Password );
							isUnAauthorized = false;

							MainWindow mainWindow = new MainWindow( data, userId );
							mainWindow.ShowDialog();
						}
						catch (AccessDeniedException)
						{
						}
						catch (SqlException)
						{
							MessageBox.Show( "SQL exception, try later", "):" );
						}
					}
					else
						break;
				}
			}
			catch (Exception ex)
			{
			}
			catch
			{
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

			if (1 == count)
			{
				List<Int32> userIds = result.ToList();
				return userIds[ 0 ];
			}
			else if (1 < count)
			{
				throw new ApplicationException( "Double row in data base" );
			}

			throw new AccessDeniedException( "Incorrect loggin or password" );
		}
		/// <summary>
		/// Sends the mail.
		/// </summary>
		/// <param name="body">The body of mail.</param>
		private static void SendMail( String body )
		{
			MailAddress toAddress = new MailAddress( "rredcat@gmail.com" );
			MailAddress fromAddress = new MailAddress( "bug.informer@rredcat.org" );
			MailAddress senderAddress = new MailAddress( "bug.informers@rredcat.org" );

			MailMessage mail = new MailMessage();
			mail.To.Add( toAddress );
			mail.From = fromAddress;
			mail.IsBodyHtml = false;
			mail.Priority = MailPriority.Low;
			mail.Sender = senderAddress;
			mail.Subject = "Raport about bug in LightOT";
			mail.Body = body;
		}
		#endregion
	}
}