using System;
using System.Globalization;
using System.Windows.Data;

namespace EnumSample
{
	public sealed class SimpleConverter : IValueConverter
	{
		#region Constants
		private const string c_VIEW_1 = "My Value 1";
		private const string c_VIEW_2 = "My Value 2";
		private const string c_VIEW_3 = "My Value 3";
		#endregion

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
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			MyEnum enumValue = (MyEnum)value;

			switch( enumValue )
			{
				case MyEnum.MyEnumValue1:
					return c_VIEW_1;
				case MyEnum.MyEnumValue2:
					return c_VIEW_2;
				case MyEnum.MyEnumValue3:
					return c_VIEW_3;
				default:
					throw new ArgumentException();
			}
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}