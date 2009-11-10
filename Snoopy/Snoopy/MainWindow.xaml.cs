using System;
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
		private const string c_URI = @"http://scarab/SoftServe.Silverlight.PhotoBrowser.Web/SoftServe.Silverlight.BrowserPage.aspx";
		private const string c_TYPE = @"application/x-silverlight-2";
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
			//var silver = (HTMLObjectElementClass)document.getElementById( "xSilverHost" );
			var silverObject = GetSilverlightObject( document );
			var sChild = silverObject.children;
		}

		private static HTMLObjectElementClass GetSilverlightObject( HTMLDocumentClass document )
		{
			HTMLObjectElementClass result = null;

			if ( !document.hasChildNodes() )
				return result;

			var child = document.firstChild;
			var lastChild = document.lastChild;
			result = GetSilverlightObject( child );

			if ( null == result && child != lastChild )
			{
				do
				{
					child = child.nextSibling;
					result = GetSilverlightObject( child );
				} while ( lastChild != child && null != result );
			}

			return result;
		}
		private static HTMLObjectElementClass GetSilverlightObject( IHTMLDOMNode node )
		{
			var item = node.firstChild;

			while ( null != item )
			{
				if ( item is HTMLObjectElementClass )
				{
					var obj = (HTMLObjectElementClass)item;

					if ( c_TYPE == obj.type )
					{
						return obj;
					}
				}

				var result = GetSilverlightObject( item );

				if ( null != result )
				{
					return result;
				}

				item = item.nextSibling;
			}

			return null;
		}
		#endregion
	}
}