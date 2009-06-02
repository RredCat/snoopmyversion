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
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			MyEnum enumValue = (MyEnum)value;

			switch ( enumValue )
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
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}