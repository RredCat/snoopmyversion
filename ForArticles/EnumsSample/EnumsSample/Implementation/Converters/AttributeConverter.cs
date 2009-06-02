using System;
using System.Windows.Data;
using System.Globalization;

namespace EnumSample
{
	public sealed class AttributeConverter:IValueConverter
	{
		#region IValueConverter Members
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			MyEnum en = (MyEnum)value;
			Attribute view = Attribute.GetCustomAttribute( value.GetType(), typeof( TestAttribute ), false );
			return value;
		}
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}