using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApp.Helper
{
	public class Calculate
	{
		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="Calculate"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Calculate( int value )
		{
			Value = value;
			IsSingle = true;
			IsCorrect = true;
			Range = 1;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Calculate"/> class.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		public Calculate( Calculate first, Calculate second )
		{
			FirstCalc = first;
			SecondCalc = second;
			Range = first.Range + second.Range;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether this instance is single.
		/// </summary>
		/// <value><c>true</c> if this instance is single; otherwise, <c>false</c>.</value>
		public bool IsSingle
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this instance is correct.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is correct; otherwise, <c>false</c>.
		/// </value>
		public bool IsCorrect
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the first calc.
		/// </summary>
		/// <value>The first calc.</value>
		public Calculate FirstCalc
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the second calc.
		/// </summary>
		/// <value>The second calc.</value>
		public Calculate SecondCalc
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public int Value
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the range.
		/// </summary>
		/// <value>The range.</value>
		public int Range
		{
			get;
			private set;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Calculates the value.
		/// </summary>
		/// <param name="modeList">The mode list.</param>
		public void CalculateValue( List<OperationMode> modeList )
		{
			if ( IsSingle )
				return;

			var childMode = modeList.Take( modeList.Count ).ToList();

			FirstCalc.CalculateValue( childMode );
			SecondCalc.CalculateValue( childMode );

			if ( FirstCalc.IsCorrect
				&& SecondCalc.IsCorrect )
			{
				switch ( modeList.Last() )
				{
					case OperationMode.Add:
						Value = FirstCalc.Value + SecondCalc.Value;
						IsCorrect = true;
						break;
					case OperationMode.Sub:
						Value = FirstCalc.Value - SecondCalc.Value;
						IsCorrect = 0 < Value;
						break;
					case OperationMode.Mul:
						Value = FirstCalc.Value * SecondCalc.Value;
						IsCorrect = true;
						break;
					case OperationMode.Div:
						var first = FirstCalc.Value;
						var second = SecondCalc.Value;
						Value = first / second;
						IsCorrect = first % second == 0;
						break;
					default:
						throw new NotImplementedException( modeList.Last().ToString() );
				}
			}
			else
				IsCorrect = false;
		}
		#endregion
	}
}