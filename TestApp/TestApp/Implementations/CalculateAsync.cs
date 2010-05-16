using System;
using System.Collections.Generic;
using System.Threading;

namespace TestApp
{
	public class CalculateAsync
	{
		#region Private fileds
		private readonly KeyList<Calculate> _list;
		private readonly IEnumerable<List<int>> _input;
		private readonly IEnumerable<List<OperationMode>> _operation;
		private readonly int _target;
		private readonly ManualResetEvent _doneEvent;
		#endregion

		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="CalculateAsync"/> class.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="input">The input.</param>
		/// <param name="operation">The operation.</param>
		/// <param name="target">The target.</param>
		/// <param name="doneEvent">The done event.</param>
		public CalculateAsync( KeyList<Calculate> list
			, IEnumerable<List<int>> input
			, IEnumerable<List<OperationMode>> operation
			, int target
			, ManualResetEvent doneEvent )
		{
			_list = list;
			_input = input;
			_operation = operation;
			_target = target;
			_doneEvent = doneEvent;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the results.
		/// </summary>
		/// <value>The results.</value>
		public List<string> Results
		{
			get;
			private set;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Threads the pool callback.
		/// </summary>
		/// <param name="threadContext">The thread context.</param>
		public void ThreadPoolCallback( Object threadContext )
		{
			int threadIndex = (int)threadContext;
			Console.WriteLine( "thread {0} started...", threadIndex );
			Results = Calculate( _list, _input, _operation, _target );
			Console.WriteLine( "thread {0} result calculated...", threadIndex );
			_doneEvent.Set();
		}
		#endregion

		#region Implementations
		/// <summary>
		/// Calculates the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="input">The input.</param>
		/// <param name="operation">The operation.</param>
		/// <param name="target">The target.</param>
		/// <returns></returns>
		private static List<string> Calculate( KeyList<Calculate> list
			, IEnumerable<List<int>> input
			, IEnumerable<List<OperationMode>> operation
			, int target )
		{
			var result = new List<string>();

			foreach ( var calculate in list.List )
			{
				foreach ( var inp in input )
				{
					foreach ( var ope in operation )
					{
						var inpParam = new List<int>( inp );
						var opeParam = new List<OperationMode>( ope );
						calculate.CalculateValue( inpParam, opeParam );

						if ( calculate.IsCorrect
							&& calculate.Value == target )
						{
							result.Add( calculate.GetExspretion() );
						}
					}
				}
			}

			return result;
		}
		#endregion
	}
}