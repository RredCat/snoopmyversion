using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
	public class Operation
	{
		#region Properties
		public OperationMode Mode
		{
			get;
			set;
		}
		public int Value { get; set; }
		#endregion

		#region Public methods
		public bool Validate()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}