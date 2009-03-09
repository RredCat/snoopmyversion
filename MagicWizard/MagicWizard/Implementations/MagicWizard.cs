using System.Windows;
using System.Windows.Input;

namespace Miracle
{
	public class MagicWizard : Window
	{
		#region Initializaition
		/// <summary>
		/// Initializes the <see cref="MagicWizard"/> class.
		/// </summary>
		static MagicWizard()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof( MagicWizard )
				, new FrameworkPropertyMetadata( typeof( MagicWizard ) ) );
		}
		public MagicWizard()
		{
			AllowsTransparency = true;
			WindowStyle = WindowStyle.None;
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			MouseLeftButtonDown += new MouseButtonEventHandler( OnMouseLeftButtonDown );
		}
		#endregion

		#region Implementation
		private void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
		{
			DragMove();
		}
		#endregion
	}
}