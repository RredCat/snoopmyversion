using System;
using System.Globalization;
using System.Windows.Data;

namespace LiteOT
{
	[ValueConversion(typeof(Boolean),typeof(Boolean))]
	public class RevertBooleanConverter:IValueConverter
	{
		#region IValueConverter Members
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return !((bool) value);
		}
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
