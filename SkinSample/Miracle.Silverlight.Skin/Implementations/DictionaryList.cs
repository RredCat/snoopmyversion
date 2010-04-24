using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Markup;

namespace Miracle.Silverlight.Skin
{
	[ContentProperty( "Itmes" )]
	public class DictionaryList : Dictionary<string, object>
	{
		#region Private fileds
		private readonly ObservableCollection<PairKeyValue> m_list = new ObservableCollection<PairKeyValue>();
		#endregion

		#region Initializations
		/// <summary>
		/// Initializes a new instance of the <see cref="DictionaryList"/> class.
		/// </summary>
		public DictionaryList()
		{
			m_list.CollectionChanged += new NotifyCollectionChangedEventHandler( OnCollectionChanged );
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the key values.
		/// </summary>
		/// <value>The key values.</value>
		public ObservableCollection<PairKeyValue> Itmes
		{
			get
			{
				return m_list;
			}
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Called when [collection changed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( NotifyCollectionChangedAction.Add != e.Action )
				throw new NotSupportedException( e.Action.ToString() );

			foreach (PairKeyValue item in e.NewItems )
			{
				Add( item.Key, item.Value );
			}
		}
		#endregion
	}
}