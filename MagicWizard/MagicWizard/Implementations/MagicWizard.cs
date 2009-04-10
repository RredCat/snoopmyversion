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
		}
		#endregion

		#region Implementation
		protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
		{
			base.OnMouseLeftButtonDown( e );
			DragMove();
		}
		#endregion
	}
}