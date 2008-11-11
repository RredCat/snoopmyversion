using System;
using System.Collections;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace LiteOT
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Private members
		private readonly OTDataDataContext m_Data = null;
		private readonly Int32 m_UserId;
		#endregion

		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		public MainWindow(OTDataDataContext data, Int32 userId )
		{
			m_Data = data;
			m_UserId = userId;
			InitializeComponent();
			DefectList.ItemsSource = GetDefects( m_Data, m_UserId );
			ProjectBox.ItemsSource = GetProjects( m_Data, m_UserId );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Gets the defects.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		private static IEnumerable GetDefects( OTDataDataContext data, Int32 userId )
		{
			return (from defects in data.Defects
			        join projects in data.Projects
			        	on defects.ProjectId equals projects.ProjectId
			        join priorities in data.PriorityTypes
			        	on defects.PriorityTypeId equals priorities.PriorityTypeId
			        join statuses in data.StatusTypes
			        	on defects.StatusTypeId equals statuses.StatusTypeId
			        where defects.AssignedToId == userId
			        select new
			               	{
			               		defects.DefectId,
			               		defects.Name,
			               		Priority = priorities.Name,
			               		Status = statuses.Name,
			               		ProjectName = projects.Name
			               	});
		}
		/// <summary>
		/// Gets the defects.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		private static IEnumerable GetDefects( OTDataDataContext data, Int32 userId , String projectName)
		{
			return ( from defects in data.Defects
					 join projects in data.Projects
						 on defects.ProjectId equals projects.ProjectId
					 join priorities in data.PriorityTypes
						 on defects.PriorityTypeId equals priorities.PriorityTypeId
					 join statuses in data.StatusTypes
						 on defects.StatusTypeId equals statuses.StatusTypeId
					 where ( defects.AssignedToId == userId && projects.Name == projectName )
					 select new
					 {
						 defects.DefectId,
						 defects.Name,
						 Priority = priorities.Name,
						 Status = statuses.Name,
						 ProjectName = projects.Name
					 } );
		}
		/// <summary>
		/// Gets the projects.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		private static IEnumerable GetProjects( OTDataDataContext data, Int32 userId )
		{
			return (from projects in data.Projects
			        join defects in data.Defects
			        	on projects.ProjectId equals defects.ProjectId
			        where defects.AssignedToId == userId
			        select projects.Name).Distinct();
		}
		#endregion

		#region Event handlers
		private void OnSelectAll(Object sender, EventArgs args)
		{
			if( null != DefectList )
			{
				DefectList.ItemsSource = GetDefects( m_Data, m_UserId );
			}
		}
		private void OnSelectCurrent(Object sender, EventArgs args)
		{
			if( false == ProjectCheck.IsChecked )
			{
				String projectName = ProjectBox.SelectedItem.ToString();
				DefectList.ItemsSource = GetDefects( m_Data, m_UserId, projectName );
			}
		}
		#endregion
	}
}