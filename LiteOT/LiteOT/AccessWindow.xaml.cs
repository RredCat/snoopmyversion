using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;

namespace LiteOT
{
	/// <summary>
	/// Interaction logic for AccessWindow.xaml
	/// </summary>
	public partial class AccessWindow
	{
		#region Constants
		internal const string CONNECTION_PATH = @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
		#endregion

		#region Private members
		/// <summary>
		/// Presents store file name.
		/// </summary>
		private readonly string m_StoreFileName = AppDomain.CurrentDomain.FriendlyName + ".dat";
		private readonly string m_StoreUserFileName = AppDomain.CurrentDomain.FriendlyName + "User.dat";

		private String m_SQLServerName = String.Empty;
		private String m_UserName = String.Empty;
		private String m_Password = String.Empty;
		private String m_DatabaseName = String.Empty;
		private Boolean m_IsDBDataLoaded = false;
		private Boolean m_IsUserDataLoaded = false;
		#endregion

		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="AccessWindow"/> class.
		/// </summary>
		public AccessWindow()
		{
			InitializeComponent();
		}
		#endregion

		#region Properties
		public String Login
		{
			get
			{
				return LoginBox.Text;
			}
		}
		public String Password
		{
			get
			{
				return PasswordBox.Password;
			}
		}
		#endregion

		#region Public methods
		public string GetConnectString()
		{
			return string.Format( CONNECTION_PATH, m_SQLServerName, m_DatabaseName, m_UserName, m_Password );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
		/// </summary>
		/// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
		protected override void OnInitialized( EventArgs e )
		{
			base.OnInitialized( e );
			LoadDBData();

			if( !m_IsDBDataLoaded )
			{
				SetDBInfo();
			}
			else
			{
				LoadUserData();

				if( m_IsUserDataLoaded )
				{
					Dispatcher.BeginInvoke( DispatcherPriority.SystemIdle, new ThreadStart( LoadDispatherLoaded ) );
				}
			}
		}

		/// <summary>
		/// Called when [click ok].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnClickOk( Object sender, EventArgs args )
		{
			SaveUserData();
			DialogResult = true;
		}
		/// <summary>
		/// Called when [click cansel].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnClickCansel( Object sender, EventArgs args )
		{
			DialogResult = false;
		}
		/// <summary>
		/// Called when [set DB info].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnSetDBInfo( Object sender, EventArgs args )
		{
			SetDBInfo();
		}
		private void SetDBInfo()
		{
			DBSetupWindow window = new DBSetupWindow();
			window.ShowDialog();

			if( true == window.DialogResult )
			{
				m_SQLServerName = window.ServerName;
				m_DatabaseName = window.DatabaseName;
				m_UserName = window.UserName;
				m_Password = window.Password;

				CahngeEnableState( true );
				SaveDBData();
			}
		}
		/// <summary>
		/// Cahnges the state of the enable.
		/// </summary>
		/// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
		private void CahngeEnableState( bool isEnabled )
		{
			LoginBox.IsEnabled = isEnabled;
			PasswordBox.IsEnabled = isEnabled;
			SavePassCheck.IsEnabled = isEnabled;
			OkButton.IsEnabled = isEnabled;
		}
		private void LoadDBData()
		{
			IsolatedStorageFile isoStorage = IsolatedStorageFile.GetStore( IsolatedStorageScope.User
				| IsolatedStorageScope.Assembly, null, null );

			LoadDBData( isoStorage, m_StoreFileName );
		}
		private void LoadDBData( IsolatedStorageFile isoStorage, string storeFileName )
		{
			if( null != isoStorage && !string.IsNullOrEmpty( storeFileName )
				&& 0 < isoStorage.GetFileNames( storeFileName ).Length )
			{
				Stream stream = new IsolatedStorageFileStream( storeFileName, FileMode.Open, isoStorage );

				try
				{
					XmlTextReader xmlTextReader = new XmlTextReader( stream );
					LoadDBData( xmlTextReader );
				}
				finally
				{
					stream.Close();
				}
			}
		}
		private void LoadDBData( XmlReader reader )
		{
			XmlSerializer serializer = new XmlSerializer( typeof( DBData ) );

			if( serializer.CanDeserialize( reader ) )
			{
				try
				{
					DBData dbParam = (DBData)serializer.Deserialize( reader );
					m_SQLServerName = dbParam.SQLServerName;
					m_UserName = dbParam.UserName;
					m_Password = dbParam.Password;
					m_DatabaseName = dbParam.DatabaseName;
					m_IsDBDataLoaded = true;
				}
				catch( InvalidOperationException ex )
				{
					if( ex.InnerException is XmlException )
					{
						throw ex.InnerException;
					}
					else
						throw;
				}
			}
			else
				Debug.Print( "XmlReader can't deserialize." );
		}
		private void SaveDBData()
		{
			IsolatedStorageFile isoStorage = IsolatedStorageFile.GetStore( IsolatedStorageScope.User
				| IsolatedStorageScope.Assembly, null, null );

			SaveDBData( isoStorage );
		}
		private void SaveDBData( IsolatedStorageFile isoStorage )
		{
			if( null != isoStorage && !string.IsNullOrEmpty( m_StoreFileName ) )
			{
				Stream stream = new IsolatedStorageFileStream( m_StoreFileName, FileMode.Create, isoStorage );
				try
				{
					XmlTextWriter writer = new XmlTextWriter( stream, Encoding.UTF8 );
					WriteDBDataToWriter( writer );
					writer.Flush();
				}
				finally
				{
					stream.Close();
				}
			}
		}
		private void WriteDBDataToWriter( XmlWriter writer )
		{
			DBData dbParam = new DBData
			{
				SQLServerName = m_SQLServerName,
				UserName = m_UserName,
				Password = m_Password,
				DatabaseName = m_DatabaseName
			};

			XmlSerializer serializer = new XmlSerializer( typeof( DBData ) );
			serializer.Serialize( writer, dbParam );
		}
		private void LoadUserData()
		{
			IsolatedStorageFile isoStorage = IsolatedStorageFile.GetStore( IsolatedStorageScope.User
				| IsolatedStorageScope.Assembly, null, null );

			LoadUserData( isoStorage, m_StoreUserFileName );
		}
		private void LoadUserData( IsolatedStorageFile isoStorage, string storeFileName )
		{
			if( null != isoStorage && !string.IsNullOrEmpty( storeFileName )
				&& 0 < isoStorage.GetFileNames( storeFileName ).Length )
			{
				Stream stream = new IsolatedStorageFileStream( storeFileName, FileMode.Open, isoStorage );

				try
				{
					XmlTextReader xmlTextReader = new XmlTextReader( stream );
					LoadUserData( xmlTextReader );
				}
				finally
				{
					stream.Close();
				}
			}
		}
		private void LoadUserData( XmlReader reader )
		{
			XmlSerializer serializer = new XmlSerializer( typeof( UserData ) );

			if( serializer.CanDeserialize( reader ) )
			{
				try
				{
					UserData dbParam = (UserData)serializer.Deserialize( reader );

					if( dbParam.IsSaved )
					{
						LoginBox.Text = dbParam.UserName;
						PasswordBox.Password = dbParam.Password;
						SavePassCheck.IsChecked = true;
						m_IsUserDataLoaded = true;
					}
				}
				catch( InvalidOperationException ex )
				{
					if( ex.InnerException is XmlException )
					{
						throw ex.InnerException;
					}
					else
						throw;
				}
			}
			else
				Debug.Print( "XmlReader can't deserialize." );
		}
		private void SaveUserData()
		{
			IsolatedStorageFile isoStorage = IsolatedStorageFile.GetStore( IsolatedStorageScope.User
				| IsolatedStorageScope.Assembly, null, null );

			SaveUserData( isoStorage );
		}
		private void SaveUserData( IsolatedStorageFile isoStorage )
		{
			if( null != isoStorage && !string.IsNullOrEmpty( m_StoreFileName ) )
			{
				Stream stream = new IsolatedStorageFileStream( m_StoreUserFileName, FileMode.Create, isoStorage );
				try
				{
					XmlTextWriter writer = new XmlTextWriter( stream, Encoding.UTF8 );
					WriteUserDataToWriter( writer );
					writer.Flush();
				}
				finally
				{
					stream.Close();
				}
			}
		}
		private void WriteUserDataToWriter( XmlWriter writer )
		{
			UserData dbParam = new UserData
			{
				UserName = GetLogin(),
				Password = GetPAssword(),
				IsSaved = SavePassCheck.IsChecked.Value
			};
			XmlSerializer serializer = new XmlSerializer( typeof( UserData ) );
			serializer.Serialize( writer, dbParam );
		}
		private string GetPAssword()
		{
			return SavePassCheck.IsChecked.Value ? PasswordBox.Password : String.Empty;
		}
		private string GetLogin()
		{
			return SavePassCheck.IsChecked.Value ? LoginBox.Text : String.Empty;
		}
		private void LoadDispatherLoaded()
		{
			RoutedEventArgs args = new RoutedEventArgs( Button.ClickEvent );
			OkButton.RaiseEvent( args );
		}
		#endregion
	}
}