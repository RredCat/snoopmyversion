using System;

namespace EnumSample
{
	[AttributeUsage( AttributeTargets.Field )]
	public class ViewEnumAttribute : Attribute
	{
		public ViewEnumAttribute( String text )
		{
			Text = text;
		}
		public String Text
		{
			get;
			private set;
		}
	}
	[AttributeUsage( AttributeTargets.Enum )]
	public class TestAttribute : Attribute
	{
	}
}