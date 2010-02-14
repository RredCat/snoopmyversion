using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WpfApplication1
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1
	{
		public Window1()
		{
			InitializeComponent();
		}

		protected override void OnInitialized( EventArgs e )
		{
			base.OnInitialized( e );
			OnClick(null, EventArgs.Empty);
		}

		private void OnClick(object sender, EventArgs args)
		{
			FileStream streamO = new FileStream( @"d:\1.CSV", FileMode.Open );
			StreamReader sReader = new StreamReader( streamO );

			string line;
			string pattern =@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)";
			Regex reg = new Regex(pattern);
			List<string> list= new List<string>();
			string oldLine = null;

			while( ( line = sReader.ReadLine() ) != null )
			{
				//if( null == oldLine )
				//{
				//    list.Add( line );
				//    oldLine = line;
				//    continue;
				//}

				//if(line != oldLine)
				//{
				//    list.Add( line );
				//}

				//oldLine = line;

				MatchCollection matchCol = reg.Matches( line );

				foreach( Match m in matchCol )
				{
					list.Add( m.Value );
				}
			}

			streamO.Close();
			FileStream streamC = new FileStream( @"d:\1.CSV", FileMode.Create );
			StreamWriter sWriter = new StreamWriter(streamC);
			
			foreach(string st in list)
			{
				sWriter.Write(st + ";");
			}

			sWriter.Close();
			streamC.Close();

			Debug.Print("____________________________\n");
		}
	}
}
