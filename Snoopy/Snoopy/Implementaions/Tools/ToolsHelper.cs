using mshtml;

namespace Snoopy
{
	public static class ToolsHelper
	{
		#region Constants
		private const string c_TYPE = @"application/x-silverlight-2";
		#endregion

		#region Public methods
		public static HTMLObjectElementClass GetSilverlightObject( HTMLDocumentClass document )
		{
			HTMLObjectElementClass result = null;

			if ( !document.hasChildNodes() )
				return result;

			var child = document.firstChild;
			var lastChild = document.lastChild;
			result = GetSilverlightObject( child );

			if ( null == result && child != lastChild )
			{
				do
				{
					child = child.nextSibling;
					result = GetSilverlightObject( child );
				} while ( lastChild != child && null != result );
			}

			return result;
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Gets the silverlight object.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns></returns>
		private static HTMLObjectElementClass GetSilverlightObject( IHTMLDOMNode node )
		{
			var item = node.firstChild;

			while ( null != item )
			{
				if ( item is HTMLObjectElementClass )
				{
					var obj = (HTMLObjectElementClass)item;

					if ( c_TYPE == obj.type )
					{
						return obj;
					}
				}

				var result = GetSilverlightObject( item );

				if ( null != result )
				{
					return result;
				}

				item = item.nextSibling;
			}

			return null;
		}
		#endregion
	}
}