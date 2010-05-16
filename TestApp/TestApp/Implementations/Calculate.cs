using System;
using System.Collections.Generic;

namespace TestApp
{
	public class Calculate
	{
		#region Private fields
		private readonly bool _isSingle;
		private readonly Calculate _firstCalc;
		private readonly Calculate _secondCalc;
		
		private OperationMode _mode;
		#endregion

		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="Calculate"/> class.
		/// </summary>
		public Calculate()
		{
			_isSingle = true;
			IsCorrect = true;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Calculate"/> class.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		public Calculate( Calculate first, Calculate second )
		{
			_firstCalc = first;
			_secondCalc = second;
		}
		#endregion

		#region Properties
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
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public int Value
		{
			get;
			private set;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Calculates the value.
		/// </summary>
		/// <param name="inputList">The input list.</param>
		/// <param name="modeList">The mode list.</param>
		public void CalculateValue( List<int> inputList, List<OperationMode> modeList )
		{
			if ( _isSingle )
			{
				Value = inputList[ 0 ];
				inputList.RemoveAt( 0 );
				return;
			}

			_firstCalc.CalculateValue( inputList, modeList );
			_secondCalc.CalculateValue( inputList, modeList );
			_mode = modeList[ 0 ];
			modeList.RemoveAt( 0 );

			if ( _firstCalc.IsCorrect
				&& _secondCalc.IsCorrect )
			{
				switch ( _mode )
				{
					case OperationMode.Add:
						Value = _firstCalc.Value + _secondCalc.Value;
						IsCorrect = true;
						break;
					case OperationMode.Sub:
						Value = _firstCalc.Value - _secondCalc.Value;
						IsCorrect = 0 < Value;
						break;
					case OperationMode.Mul:
						Value = _firstCalc.Value * _secondCalc.Value;
						IsCorrect = true;
						break;
					case OperationMode.Div:
						var first = _firstCalc.Value;
						var second = _secondCalc.Value;
						Value = first / second;
						IsCorrect = first % second == 0;
						break;
					default:
						throw new NotImplementedException( _mode.ToString() );
				}
			}
			else
				IsCorrect = false;
		}
		/// <summary>
		/// Gets the exspretion.
		/// </summary>
		/// <returns></returns>
		public string GetExspretion()
		{
			if ( _isSingle )
				return Value.ToString();

			var mode = GetReadableMode();
			var firstExp = _firstCalc.GetExspretion();
			var secondExp = _secondCalc.GetExspretion();
			return string.Format( "({0}{1}{2})", firstExp, mode, secondExp );
		}
		#endregion

		#region Implementations
		/// <summary>
		/// Gets the readable mode.
		/// </summary>
		/// <returns></returns>
		private string GetReadableMode()
		{
			switch ( _mode )
			{
				case OperationMode.Add:
					return "+";
				case OperationMode.Sub:
					return "-";
				case OperationMode.Mul:
					return "*";
				case OperationMode.Div:
					return "/";
				default:
					throw new NotImplementedException( _mode.ToString() );
			}
		}
		#endregion
	}
}