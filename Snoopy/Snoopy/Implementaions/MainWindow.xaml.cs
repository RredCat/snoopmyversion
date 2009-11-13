using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Navigation;
using mshtml;

namespace Snoopy
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Constants
		private const string c_URI = @"http://localhost/TestSilverlightApplication.Web/TestSilverlightApplicationTestPage.aspx";
		#endregion

		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			xBrowser.LoadCompleted += new LoadCompletedEventHandler( OnBrowserLoadCompleted );
			xBrowser.Source = new Uri( c_URI, UriKind.Absolute );
		}
		#endregion

		#region Implementation
		private void OnBrowserLoadCompleted( object sender, NavigationEventArgs e )
		{
			var document = (HTMLDocumentClass )xBrowser.Document;
			var silverObject =ToolsHelper.GetSilverlightObject( document );
			//var sChild = silverObject.children;

			Type t = silverObject.GetType();
				//Type.GetType( "PrimarySharedLameServer.HelloClass" );
			Debug.Print( "Base type {0}", t.BaseType );
			Debug.Print( "GUID {0}", t.GUID );
			Debug.Print( "COM Object? {0}", t.IsCOMObject );
			Debug.Print( "Defining Namespace {0}", t.Namespace );
		}
		#endregion
	}
}