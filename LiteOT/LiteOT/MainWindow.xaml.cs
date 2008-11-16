using System;
using System.Collections;
using System.Linq;
using System.Windows;
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
		#region Constants
		private const String DESCRIPTIONS_NAME = "Descriptions";
		private const String NOTES_NAME = "Notes";
        private const String REPLICATION_PROPCEDURES_NAME ="RepPropcedures";
		#endregion

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
		/// <param name="projectName"></param>
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
		private void OnInfoTabChanged( Object sender, EventArgs args )
		{
			Object selectedItem = DefectList.SelectedItem;

			if (null != selectedItem)
			{
				var item = Utility.Cast(selectedItem, new
              	{
              		DefectId = 0,
              		Name = "",
              		Priority = "",
              		Status = "",
              		ProjectName = ""
              	});

				Int32 id = item.DefectId;
				var tag = ( (FrameworkElement)TabInfo.SelectedItem ).Tag.ToString();

				var result = from defects in m_Data.Defects
				         where defects.DefectId == id
							 select Description( defects, tag );

				int count = result.Count();

				if( 1 == count )
				{
					foreach( String info in result )
					{
						SetInfo(info,tag);
					}
				}
				else if( 1 < count )
				{
					throw new ApplicationException( "Double row in data base" );
				}
			}
		}

		private void SetInfo(string info, String tag)
		{
			switch( tag )
			{
				case DESCRIPTIONS_NAME:
					DescriptionInfo.Text = info;
					break;
				case NOTES_NAME:
					NotesInfo.Text = info;
					break;
				case REPLICATION_PROPCEDURES_NAME:
					RepPropceduresInfo.Text = info;
					break;
				default:
					throw new NotImplementedException();
			}
		}
		private static string Description( Defect defects, String tag )
		{
			switch( tag )
			{
				case DESCRIPTIONS_NAME:
					return defects.Description;
				case NOTES_NAME:
					return defects.Notes;
				case REPLICATION_PROPCEDURES_NAME:
					return defects.ReplicationProcedures;
				default:
					throw new NotImplementedException();
			}
		}
		#endregion
	}
}