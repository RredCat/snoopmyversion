using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

//Dictionary<string, object>
using SkinDictionary = System.Collections.Generic.Dictionary<string, object>;

namespace Miracle.Silverlight.Skin
{
	public class SkinManager : FrameworkElement
	{
		#region Constants
		private const string c_DEFAULT = "Default";
		#endregion

		#region Private fileds
		private static SkinManager m_Instance;

		private readonly List<WeakReference> m_skinObjects = new List<WeakReference>();
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the skins.
		/// </summary>
		/// <value>The skins.</value>
		public DictionaryList Skins
		{
			get
			{
				return (DictionaryList)GetValue( SkinsProperty );
			}
			set
			{
				SetValue( SkinsProperty, value );
			}
		}
		/// <summary>
		/// Gets or sets the name of the current skin.
		/// </summary>
		/// <value>The name of the current skin.</value>
		public string CurrentSkinName
		{
			get
			{
				return (string)GetValue( CurrentSkinNameProperty );
			}
			set
			{
				SetValue( CurrentSkinNameProperty, value );
			}
		}
		/// <summary>
		/// Gets or sets the value of the CurrentSkin dependency property.
		/// </summary>

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static SkinManager Instance
		{
			get
			{
				return m_Instance ?? ( m_Instance = new SkinManager() );
			}
		}
		#endregion

		#region Public methods
		public static SkinStateMode GetSkinState(DependencyObject obj)
		{
			return (SkinStateMode)obj.GetValue( SkinStateProperty );
		}
		public static void SetSkinState(DependencyObject obj, SkinStateMode value)
		{
			obj.SetValue( SkinStateProperty, value );
		}
		#endregion

		#region Implementations
		/// <summary>
		/// Sets the current Skin.
		/// </summary>
		private void SetCurrentSkin()
		{
			string key = CurrentSkinName;
			var skin = Skins.ContainsKey( key )
				? Skins[ key ]
				: null;

			for (int i = m_skinObjects.Count-1; i > -1; --i)
			{
				var reference = m_skinObjects[ i ];

				if ( reference.IsAlive )
				{
					var skinObject = (DependencyObject)reference.Target;
					skinObject.SetValue( SkinProperty, skin );
				}
				else
					m_skinObjects.RemoveAt(i);
			}
		}
		/// <summary>
		/// Registers the element.
		/// </summary>
		/// <param name="dpObject">The dp object.</param>
		private void RegisterElement( DependencyObject dpObject )
		{
			string key = CurrentSkinName;
			var skin = Skins.ContainsKey( key )
					? Skins[ key ]
					: null;
			dpObject.SetValue( SkinProperty, skin );
			m_skinObjects.Add( new WeakReference( dpObject ) );
		}
		/// <summary>
		/// Uns the register element.
		/// </summary>
		/// <param name="dpObject">The dp object.</param>
		private void UnRegisterElement(DependencyObject dpObject)
		{
			WeakReference reference = null;

			foreach (var item in m_skinObjects)
			{
				if(dpObject == item.Target)
				{
					reference = item;
					break;
				}
			}

			Debug.Assert( null == reference, "I realy don't now how you do it!" );
			dpObject.ClearValue( SkinProperty );
			m_skinObjects.Remove( reference );
		}

		/// <summary>
		/// Called when [current skin name changed].
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnCurrentSkinNameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var instance = (SkinManager)d;
			instance.SetCurrentSkin();
		}
		/// <summary>
		/// Called when [skin state changed].
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnSkinStateChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var mode = (SkinStateMode) e.NewValue;

			if ( SkinStateMode.Hold == mode )
			{
				m_Instance.RegisterElement( d );
			}
			else
				m_Instance.UnRegisterElement(d);
		}
		#endregion

		#region Dependency properties
		public static readonly DependencyProperty SkinsProperty
			= DependencyProperty.Register( "Skins"
				, typeof( DictionaryList )
				, typeof( SkinManager )
				, new PropertyMetadata( null ) );
		public static readonly DependencyProperty CurrentSkinNameProperty
			= DependencyProperty.Register( "CurrentSkinName"
				, typeof( string )
				, typeof( SkinManager )
				, new PropertyMetadata( c_DEFAULT
					, new PropertyChangedCallback( OnCurrentSkinNameChanged ) ) );

		public static readonly DependencyProperty SkinProperty
			= DependencyProperty.RegisterAttached( "Skin"
				, typeof( SkinDictionary )
				, typeof( SkinManager )
				, new PropertyMetadata( null ) );
		public static readonly DependencyProperty SkinStateProperty
			= DependencyProperty.RegisterAttached( "SkinState"
				, typeof( SkinStateMode )
				, typeof( SkinManager )
				, new PropertyMetadata( SkinStateMode.Default
					, new PropertyChangedCallback( OnSkinStateChanged ) ) );
		#endregion
	}
}