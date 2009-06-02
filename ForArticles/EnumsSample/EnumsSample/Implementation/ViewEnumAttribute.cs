using System;

namespace EnumSample
{
	[AttributeUsage( AttributeTargets.Field )]
	public class ViewEnumAttribute : Attribute
	{
		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewEnumAttribute"/> class.
		/// </summary>
		/// <param name="view">The view.</param>
		public ViewEnumAttribute( String view )
		{
			View = view;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		public String View
		{
			get;
			private set;
		}
		#endregion
	}
}