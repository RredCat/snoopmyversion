using System.Windows.Controls;
using Miracle.Silverlight.Themes;

namespace ThemesSample
{
	public partial class MainPage
	{
		#region Initialization
		public MainPage()
		{
			InitializeComponent();
			xSwich.ItemsSource = new[] { "Default", "Moody" };
			xSwich.SelectedIndex = 0;
		}
		#endregion

		#region Implementations
		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var combo = (ComboBox)sender;
			ThemesManager.Instance.CurrentThemeName = combo.SelectedItem.ToString();
		}
		#endregion
	}
}