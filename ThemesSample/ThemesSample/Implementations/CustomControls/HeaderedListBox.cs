using System.Windows;
using System.Windows.Controls;

namespace ThemesSample
{
	public class HeaderedListBox :ListBox
	{
		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="HeaderedListBox"/> class.
		/// </summary>
		public HeaderedListBox()
		{
			DefaultStyleKey = typeof( HeaderedListBox );
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the header.
		/// </summary>
		/// <value>The header.</value>
		public string Header
		{
			get
			{
				return (string)GetValue( HeaderProperty );
			}
			set
			{
				SetValue( HeaderProperty, value );
			}
		}
		#endregion

		#region Dependency properties
		public static readonly DependencyProperty HeaderProperty
			= DependencyProperty.Register( "Header"
			, typeof( string )
			, typeof( HeaderedListBox )
			, new PropertyMetadata( string.Empty ) );
		#endregion
	}
}