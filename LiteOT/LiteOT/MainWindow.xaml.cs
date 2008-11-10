using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LiteOT
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Private members
		private readonly OTDataDataContext m_Data = null;
		private Int32 m_UserId;
		#endregion

		#region Initialization
		public MainWindow(OTDataDataContext data, Int32 userId )
		{
			m_Data = data;
			m_UserId = userId;
			InitializeComponent();
			DefectList.ItemsSource = GetDefects(m_Data,  m_UserId);
			ProjectBox.ItemsSource = GetProjects( m_Data, m_UserId );
		}
		#endregion

		#region Implementation
		private static IEnumerable GetDefects( OTDataDataContext data, Int32 userId )
		{
			return from defects in data.Defects
			              where defects.AssignedToId == userId
				   select new
				   {
					   defects.DefectId,
					   defects.Name,
					   defects.PriorityTypeId,
					   defects.StatusTypeId,
					   defects.ProjectId
				   };

		}
		private static IEnumerable GetProjects( OTDataDataContext data, Int32 userId )
		{
			return ( from projects in data.Projects
					 join defects in data.Defects
					 on projects.ProjectId equals defects.ProjectId
					 where defects.AssignedToId == userId
					 select projects.Name ).Distinct();
		}
		#endregion
	}
}
