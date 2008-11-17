using System;
using System.Linq;
using System.Windows;
using LiteOT.Implementation.Tools;

namespace LiteOT
{
	/// <summary>
	/// Interaction logic for DBSetupWindow.xaml
	/// </summary>
	public partial class DBSetupWindow
	{
		#region Initialize
		/// <summary>
		/// Initializes a new instance of the <see cref="DBSetupWindow"/> class.
		/// </summary>
		public DBSetupWindow()
		{
			InitializeComponent();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the server.
		/// </summary>
		/// <value>The name of the server.</value>
		public String ServerName
		{
			get
			{
				return ServerNameBox.Text;
			}
		}
		/// <summary>
		/// Gets the name of the database.
		/// </summary>
		/// <value>The name of the database.</value>
		public String DatabaseName
		{
			get
			{
				return DatabaseNameBox.Text;
			}
		}
		/// <summary>
		/// Gets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public String UserName
		{
			get
			{
				return UserNameBox.Text;
			}
		}
		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <value>The password.</value>
		public String Password
		{
			get
			{
				return PasswordBox.Password;
			}
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Called when [click ok].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnClickOk( object sender, EventArgs args )
		{
			DialogResult = true;
		}
		/// <summary>
		/// Called when [click cansel].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnClickCansel( object sender, EventArgs args )
		{
			DialogResult = false;
		}
		/// <summary>
		/// Called when [test connenct].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnTestConnenct( object sender, EventArgs args )
		{
			String connenctString = string.Format(AccessWindow.CONNECTION_PATH, ServerNameBox.Text, DatabaseNameBox.Text,
			                                      UserNameBox.Text, PasswordBox.Password);
			Boolean isCorrect;

			try
			{
				OTDataDataContext data = new OTDataDataContext( connenctString );
				var result = ( from user in data.Users
							   select user.UserId ).Take( 1 );

				isCorrect = 1 == result.Count();
			}
			catch( Exception )
			{
				isCorrect = false;
			}

			if( isCorrect )
			{
				MessageBox.Show( "Yes!", "We do it!", MessageBoxButton.OK, MessageBoxImage.Information );
			}
			else
				MessageBox.Show( "Incorrect data!", "):", MessageBoxButton.OK, MessageBoxImage.Error );
		}
		#endregion
	}
}