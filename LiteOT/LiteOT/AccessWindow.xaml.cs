using System;

namespace AlternativeOT
{
	/// <summary>
	/// Interaction logic for AccessWindow.xaml
	/// </summary>
	public partial class AccessWindow
	{
		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="AccessWindow"/> class.
		/// </summary>
		public AccessWindow()
		{
			InitializeComponent();
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Called when [click ok].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnClickOk( object sender, EventArgs args )
		{
			DialogResult = true;
		}
		/// <summary>
		/// Called when [click cansel].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnClickCansel( object sender, EventArgs args )
		{
			DialogResult = false;
		}
		#endregion
	}
}
