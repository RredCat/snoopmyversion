using System;

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
		/// Gets or sets the name of the server.
		/// </summary>
		/// <value>The name of the server.</value>
		internal String ServerName
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the name of the DB user.
		/// </summary>
		/// <value>The name of the DB user.</value>
		internal String DBUserName
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the DB password.
		/// </summary>
		/// <value>The DB password.</value>
		internal String DBPassword
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the initial catalog.
		/// </summary>
		/// <value>The initial catalog.</value>
		internal String InitialCatalog
		{
			get;
			set;
		}
		#endregion
	}
}