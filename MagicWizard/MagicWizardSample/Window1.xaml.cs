using System.Windows;
using Miracle;

namespace MagicWizardSample
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1
	{
		#region Initialization
		public Window1()
		{
			InitializeComponent();
		}
		#endregion

		#region Implementation
		private void OnClick( object sender, RoutedEventArgs e )
		{
			MagicWizard wiz = new MagicWizard();
			wiz.Show();
		}
		#endregion
	}
}