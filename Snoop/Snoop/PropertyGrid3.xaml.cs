namespace Snoop
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Threading;

	public partial class PropertyGrid3 : INotifyPropertyChanged
	{
		#region RoutedCommands
		public static readonly RoutedCommand RemoveIt = new RoutedCommand();
		public static readonly RoutedCommand RemoveAll = new RoutedCommand();
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		#region Private members
		private object target;
		private PropertyFilter propertyFilter = new PropertyFilter( string.Empty, true );
		private ObservableCollection<PropertyInformation> properties = new ObservableCollection<PropertyInformation>();
		private List<object> inspectStack = new List<object>();
		private PropertyInformation selection;
		private IEnumerator<PropertyInformation> propertiesToAdd;
		private GridViewColumnHeader lastHeaderClicked = null;
		private ListSortDirection lastDirection = ListSortDirection.Ascending;
		private DelayedCall processIncrementalCall;
		private DelayedCall filterCall;
		#endregion

		#region Initialization
		public PropertyGrid3()
		{

			processIncrementalCall = new DelayedCall( ProcessIncrementalPropertyAdd, DispatcherPriority.Background );
			filterCall = new DelayedCall( ProcessFilter, DispatcherPriority.Background );

			InitializeComponent();

			Unloaded += HandleUnloaded;

			CommandBindings.Add( new CommandBinding( PropertyGrid3.RemoveIt, HandleRemoveIt ) );
			CommandBindings.Add( new CommandBinding( PropertyGrid3.RemoveAll, HandleRemoveAll ) );
		} 
		#endregion

		#region Properties
		public PropertyInformation Selection
		{
			get
			{
				return selection;
			}
			set
			{
				selection = value;
				OnPropertyChanged( "Selection" );
			}
		}
		public Type Type
		{
			get
			{
				if( target != null )
					return target.GetType();
				return null;
			}
		}
		public ObservableCollection<PropertyInformation> Properties
		{
			get
			{
				return properties;
			}
		}
		public string StringFilter
		{
			get
			{
				return propertyFilter.FilterString;
			}
			set
			{
				propertyFilter.FilterString = value;

				filterCall.Enqueue();

				OnPropertyChanged( "StringFilter" );
			}
		}
		#endregion

		#region Public methods
			public void PushTarget( object target )
		{
			inspectStack.Add( target );
			ChangeTarget( target );
		}
		public void SetTarget( object target )
		{
			inspectStack.Clear();
			ChangeTarget( target );
		}
		#endregion

		#region Implementations
		private void ChangeTarget( object newTarget )
		{

			if( target != newTarget )
			{
				target = newTarget;

				foreach( PropertyInformation property in properties )
					property.Teardown();
				properties.Clear();

				propertiesToAdd = null;
				processIncrementalCall.Enqueue();

				OnPropertyChanged( "Type" );
			}
		}
		/// <summary>
		/// Delayed loading of the property inspector to avoid creating the entire list of property
		/// editors immediately after selection. Keeps that app running smooth.
		/// </summary>
		/// <param name="performInitialization"></param>
		/// <returns></returns>
		private void ProcessIncrementalPropertyAdd()
		{
			int numberToAdd = 10;

			if( propertiesToAdd == null )
			{
				propertiesToAdd = PropertyInformation.GetProperties( target ).GetEnumerator();

				numberToAdd = 0;
			}
			int i = 0;
			for( ; i < numberToAdd && propertiesToAdd.MoveNext(); ++i )
			{

				PropertyInformation property = propertiesToAdd.Current;
				property.Filter = propertyFilter;
				properties.Add( property );
			}

			if( i == numberToAdd )
				processIncrementalCall.Enqueue();
			else
				propertiesToAdd = null;
		}
		private void HandleRemoveIt( object sender, ExecutedRoutedEventArgs e )
		{
			FrameworkElement element = (FrameworkElement)e.OriginalSource;
			PropertyInformation information = (PropertyInformation)element.DataContext;
			Properties.Remove( information );
		}
		private void HandleRemoveAll( object sender, ExecutedRoutedEventArgs e )
		{
			Properties.Clear();
		}

		private void ProcessFilter()
		{
			foreach( PropertyInformation property in properties )
				property.Filter = propertyFilter;
		}
		public bool ShowDefaults
		{
			get
			{
				return propertyFilter.ShowDefaults;
			}
			set
			{
				propertyFilter.ShowDefaults = value;

				foreach( PropertyInformation property in properties )
					property.Filter = propertyFilter;

				OnPropertyChanged( "ShowDefaults" );
			}
		}
		private void HandleUnloaded( object sender, EventArgs e )
		{
			foreach( PropertyInformation property in properties )
				property.Teardown();
		}
		protected void OnPropertyChanged( string propertyName )
		{
			Debug.Assert( GetType().GetProperty( propertyName ) != null );
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
		#endregion
	}
}
