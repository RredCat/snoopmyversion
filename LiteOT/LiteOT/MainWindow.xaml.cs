using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Documents;

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
		private const String REPLICATION_PROPCEDURES_NAME = "RepPropcedures";
		private const String ATACHMENTS_NAME = "Atachments";
		private const String DEFECT_TAG = "Defect";
		private const String FEATURE_TAG = "Feature";
		#endregion

		#region Private members
		private readonly OTDataDataContext m_Data = null;
		private readonly Int32 m_UserId;

		private IssueType m_IssueType = IssueType.Defect;
		#endregion

		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		public MainWindow( OTDataDataContext data, Int32 userId )
		{
			m_Data = data;
			m_UserId = userId;
			InitializeComponent();
			DefectList.ItemsSource = GetDefects( m_Data, m_UserId );
			ProjectBox.ItemsSource = GetProjects( m_Data, m_UserId );
		}
		#endregion

		#region Implementation
		private void SetInfo( Object info, String tag )
		{
			switch( tag )
			{
				case DESCRIPTIONS_NAME:
					DescriptionInfo.Document = GetTextInfo( info );
					break;
				case NOTES_NAME:
					NotesInfo.Document = GetTextInfo( info );
					break;
				case REPLICATION_PROPCEDURES_NAME:
					RepPropceduresInfo.Document = GetTextInfo( info );
					break;
				case ATACHMENTS_NAME:
					AttachmentList.ItemsSource = GetAttachments( info );
					break;
				default:
					throw new NotImplementedException();
			}
		}
		private IEnumerable GetAttachments( Object defectID )
		{
			Int32 id = Int32.Parse( defectID.ToString() );

			return from attachments in m_Data.Attachments
				   where ( attachments.SourceType == (int)m_IssueType && attachments.SourceId == id )
				   select new
				   {
					   attachments.AttachmentId,
					   attachments.FileName,
					   attachments.AttachDate,
					   attachments.Description
				   };
		}

		/// <summary>
		/// Gets the defects.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		private static IEnumerable GetDefects( OTDataDataContext data, Int32 userId )
		{
			return ( from defects in data.Defects
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
							 } );
		}
		/// <summary>
		/// Gets the defects.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="projectName"></param>
		/// <returns></returns>
		private static IEnumerable GetDefects( OTDataDataContext data, Int32 userId, String projectName )
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
			return ( from projects in data.Projects
					 join defects in data.Defects
						 on projects.ProjectId equals defects.ProjectId
					 where defects.AssignedToId == userId
					 select projects.Name ).Distinct();
		}
		/// <summary>
		/// Gets the text info.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		private static FlowDocument GetTextInfo( Object text )
		{
			Paragraph paragraph = new Paragraph();
			paragraph.Inlines.Add( new Run( text.ToString() ) );
			FlowDocument document = new FlowDocument();
			document.Blocks.Add( paragraph );
			return document;
		}
		#endregion

		#region Event handlers
		private void OnSelectAll( Object sender, EventArgs args )
		{
			if( null != DefectList )
			{
				DefectList.ItemsSource = GetDefects( m_Data, m_UserId );
			}
		}
		private void OnSelectCurrent( Object sender, EventArgs args )
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

			if( null != selectedItem )
			{
				var item = Utility.Cast( selectedItem, new
				{
					DefectId = 0,
					Name = "",
					Priority = "",
					Status = "",
					ProjectName = ""
				} );

				Int32 id = item.DefectId;
				String tag = ( (FrameworkElement)TabInfo.SelectedItem ).Tag.ToString();
				var result = from defects in m_Data.Defects
							 where defects.DefectId == id
							 select Description( defects, tag );

				int count = result.Count();

				if( 1 == count )
				{
					List<Object> res = result.ToList();
					SetInfo( res[ 0 ], tag );
				}
				else if( 1 < count )
				{
					throw new ApplicationException( "Double row in data base" );
				}
			}
		}
		private void OnIssueTypeChecked( Object sender, EventArgs args )
		{
			RadioButton radio = (RadioButton)sender;

			if( radio.IsInitialized )
			{
				String tag = radio.Tag.ToString();

				switch( tag )
				{
					case DEFECT_TAG:
						m_IssueType = IssueType.Defect;
						break;
					case FEATURE_TAG:
						m_IssueType = IssueType.Feature;
						break;
					default:
						throw new NotImplementedException();
				}
			}
		}

		private static Object Description( Defect defects, String tag )
		{
			switch( tag )
			{
				case DESCRIPTIONS_NAME:
					return defects.Description;
				case NOTES_NAME:
					return defects.Notes;
				case REPLICATION_PROPCEDURES_NAME:
					return defects.ReplicationProcedures;
				case ATACHMENTS_NAME:
					return defects.DefectId;
				default:
					throw new NotImplementedException();
			}
		}
		#endregion
	}
}