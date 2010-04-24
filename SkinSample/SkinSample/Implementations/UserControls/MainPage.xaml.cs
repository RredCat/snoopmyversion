using System.Windows.Controls;
using Miracle.Silverlight.Skin;

namespace SkinSample
{
	public partial class MainPage
	{
		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="MainPage"/> class.
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
			xSwich.ItemsSource = new[] { "Default", "Moody" };
			xSwich.SelectedIndex = 0;
		}
		#endregion

		#region Implementations
		/// <summary>
		/// Called when [selection changed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var combo = (ComboBox)sender;
			SkinManager.Instance.CurrentSkinName = combo.SelectedItem.ToString();
		}
		#endregion
	}
}