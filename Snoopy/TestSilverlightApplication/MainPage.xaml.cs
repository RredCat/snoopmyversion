using System.Windows;
using System.Windows.Media;

namespace TestSilverlightApplication
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
		private void OnClick( object sender, RoutedEventArgs e )
		{
			LayoutRoot.Background = new SolidColorBrush( Colors.Blue );
		}
	}
}