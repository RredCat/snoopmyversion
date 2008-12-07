using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace LiteOT
{
	/// <summary>
	/// Presents convertor that converts item index value to enabled state.
	/// </summary>
	[ValueConversion( typeof( Int32 ), typeof( Boolean ) )]
	public class SelectedToVisibilityConverter : IValueConverter
	{
		#region IValueConverter Members
		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public Object Convert( Object value, Type targetType, Object parameter, CultureInfo culture )
		{
			return -1 < ((Int32) value) ? Visibility.Visible : Visibility.Collapsed;
		}
		/// <summary>
		/// Nothing does.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public Object ConvertBack( Object value, Type targetType, Object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}