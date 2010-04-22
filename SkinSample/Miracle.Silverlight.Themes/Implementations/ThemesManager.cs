using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

//Dictionary<string, object>
using ThemeDictionary = System.Collections.Generic.Dictionary<string, object>;

namespace Miracle.Silverlight.Themes
{
	public class ThemesManager : FrameworkElement
	{
		#region Constants
		private const string c_DEFAULT = "Default";
		#endregion

		#region Private fileds
		private static ThemesManager m_Instance;

		private readonly List<WeakReference> m_themedObjects = new List<WeakReference>();
		#endregion

		#region Properties
		public DictionaryList Themes
		{
			get
			{
				return (DictionaryList)GetValue( ThemesProperty );
			}
			set
			{
				SetValue( ThemesProperty, value );
			}
		}
		public string CurrentThemeName
		{
			get
			{
				return (string)GetValue( CurrentThemeNameProperty );
			}
			set
			{
				SetValue( CurrentThemeNameProperty, value );
			}
		}
		/// <summary>
		/// Gets or sets the value of the CurrentTheme dependency property.
		/// </summary>

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static ThemesManager Instance
		{
			get
			{
				return m_Instance ?? ( m_Instance = new ThemesManager() );
			}
		}
		#endregion

		#region Public methods
		public static ThemeStateMode GetThemeState(DependencyObject obj)
		{
			return (ThemeStateMode)obj.GetValue( ThemeStateProperty );
		}
		public static void SetThemeState(DependencyObject obj, ThemeStateMode value)
		{
			obj.SetValue( ThemeStateProperty, value );
		}
		#endregion

		#region Implementations
		/// <summary>
		/// Sets the current theme.
		/// </summary>
		private void SetCurrentTheme()
		{
			string key = CurrentThemeName;
			var theme = Themes.ContainsKey( key )
				? Themes[ key ]
				: null;

			for (int i = m_themedObjects.Count-1; i > -1; --i)
			{
				var reference = m_themedObjects[ i ];

				if ( reference.IsAlive )
				{
					var themedObject = (DependencyObject)reference.Target;
					themedObject.SetValue( ThemeProperty, theme );
				}
				else
					m_themedObjects.RemoveAt(i);
			}
		}
		/// <summary>
		/// Registers the element.
		/// </summary>
		/// <param name="dpObject">The dp object.</param>
		private void RegisterElement(DependencyObject dpObject)
		{
				string key = CurrentThemeName;
				var theme = Themes.ContainsKey( key )
					? Themes[ key ]
					: null;
				dpObject.SetValue( ThemeProperty, theme );
				m_themedObjects.Add( new WeakReference( dpObject ) );
		}
		/// <summary>
		/// Uns the register element.
		/// </summary>
		/// <param name="dpObject">The dp object.</param>
		private void UnRegisterElement(DependencyObject dpObject)
		{
			WeakReference reference = null;

			foreach (var item in m_themedObjects)
			{
				if(dpObject == item.Target)
				{
					reference = item;
					break;
				}
			}

			Debug.Assert( null == reference, "I realy don't now how you do it!" );
			dpObject.ClearValue( ThemeProperty );
			m_themedObjects.Remove( reference );
		}

		/// <summary>
		/// Called when [current theme name changed].
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCurrentThemeNameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var instance = (ThemesManager)d;
			instance.SetCurrentTheme();
		}
		private static void OnThemeStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var mode = (ThemeStateMode) e.NewValue;

			if ( ThemeStateMode.Hold == mode )
			{
				m_Instance.RegisterElement( d );
			}
			else
				m_Instance.UnRegisterElement(d);
		}
		#endregion

		#region Dependency properties
		public static readonly DependencyProperty ThemesProperty
			= DependencyProperty.Register( "Themes"
				, typeof( DictionaryList )
				, typeof( ThemesManager )
				, new PropertyMetadata( null ) );
		public static readonly DependencyProperty CurrentThemeNameProperty
			= DependencyProperty.Register( "CurrentThemeName"
				, typeof( string )
				, typeof( ThemesManager )
				, new PropertyMetadata( c_DEFAULT
					, new PropertyChangedCallback( OnCurrentThemeNameChanged ) ) );
		
		public static readonly DependencyProperty ThemeProperty
			= DependencyProperty.RegisterAttached( "Theme"
				, typeof( ThemeDictionary )
				, typeof( ThemesManager )
				, new PropertyMetadata( null ) );
		public static readonly DependencyProperty ThemeStateProperty
			= DependencyProperty.RegisterAttached( "ThemeState"
				, typeof( ThemeStateMode )
				, typeof( ThemesManager )
				, new PropertyMetadata( ThemeStateMode.Default
					, new PropertyChangedCallback( OnThemeStateChanged ) ) );
		#endregion
	}
}