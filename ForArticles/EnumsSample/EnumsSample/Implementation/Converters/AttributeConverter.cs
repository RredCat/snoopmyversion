using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace EnumSample
{
	public sealed class AttributeConverter : IValueConverter
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
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return GetEnumView( value.GetType(), value.ToString() );
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

		#region Implementation
		/// <summary>
		/// Gets the enum view.
		/// </summary>
		/// <param name="fieldType">Type of the field.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns></returns>
		private static object GetEnumView( Type fieldType, string fieldName )
		{
			FieldInfo info = fieldType.GetField( fieldName );

			if( null != info )
			{
				foreach( ViewEnumAttribute attribute in info.GetCustomAttributes( typeof( ViewEnumAttribute ), false ) )
				{
					return attribute.View;
				}
			}

			return fieldName;
		}
		#endregion
	}
}