using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Data.Linq;
using System.IO;
using Microsoft.Win32;

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
		private const Int32 DEFECT_TAG = 0;
		private const Int32 FEATURE_TAG = 1;
		private const String HTML_FORMAT = "<html><body>{0}</body></html>";
		private const String TEMP_FILE = "temp.html";
		private const String SEPARATOR = @"\";
		#endregion

		#region Private members
		private readonly OTDataDataContext m_Data = null;
		private readonly Int32 m_UserId;
		private readonly String m_CurrentDirrectory = Directory.GetCurrentDirectory();

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
			RefreshIssueList();
			ProjectBox.ItemsSource = GetProjects( m_Data, m_UserId );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="tag">The tag.</param>
		private void SetInfo( Object info, String tag )
		{
			switch( tag )
			{
				case DESCRIPTIONS_NAME:
					UpdateTextFrame( DescriptionInfo, info );
					break;
				case NOTES_NAME:
					UpdateTextFrame( NotesInfo, info );
					break;
				case REPLICATION_PROPCEDURES_NAME:
					UpdateTextFrame( RepPropceduresInfo, info );
					break;
				case ATACHMENTS_NAME:
					AttachmentList.ItemsSource = GetAttachments( info );
					break;
				default:
					throw new NotImplementedException();
			}
		}
		private IEnumerable GetAttachments( Object issueID )
		{
			Int32 id = Int32.Parse( issueID.ToString() );

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
		/// Gets the text info.
		/// </summary>
		/// <param name="textFrame">The text frame.</param>
		/// <param name="text">The text.</param>
		private void UpdateTextFrame(Frame textFrame, Object text )
		{
			String str = String.Format( HTML_FORMAT, text );
			File.WriteAllText( TEMP_FILE, str );
			textFrame.Source = new Uri( m_CurrentDirrectory + SEPARATOR + TEMP_FILE );
			textFrame.Refresh();
		}
		/// <summary>
		/// Refreshes the issue list.
		/// </summary>
		private void RefreshIssueList()
		{
			IssueList.ItemsSource = GetIssues( m_Data, m_UserId );
		}
		/// <summary>
		/// Selects the current.
		/// </summary>
		private void SelectCurrent()
		{
			if (false == ProjectCheck.IsChecked)
			{
				String projectName = ProjectBox.SelectedItem.ToString();
				IssueList.ItemsSource = GetIssues( m_Data, m_UserId, projectName );
			}
		}
		/// <summary>
		/// Gets the defects.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		private IEnumerable GetIssues( OTDataDataContext data, Int32 userId )
		{
			return (IssueType.Defect == m_IssueType) 
				? GetDefects(data, userId) 
				: GetFeatures(data, userId);
		}
		/// <summary>
		/// Gets the tab info.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		private IQueryable<Object> GetTabInfo( Int32 id, String tag )
		{
			return IssueType.Defect == m_IssueType
				? GetDefectsTabInfo( id, tag )
				: GetFeaturesTabInfo( id, tag );
		}
		/// <summary>
		/// Gets the defects tab info.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		private IQueryable<Object> GetDefectsTabInfo( Int32 id, String tag )
		{
			return from defects in m_Data.Defects
				   where defects.DefectId == id
				   select GetDefectDescription( defects, tag );
		}
		/// <summary>
		/// Gets the features tab info.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		private IQueryable<Object> GetFeaturesTabInfo( Int32 id, String tag )
		{
			return from features in m_Data.Features
				   where features.FeatureId == id
				   select GetFeatureDescription( features, tag );
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
								 IssueId = defects.DefectId,
								 defects.Name,
								 Priority = priorities.Name,
								 Status = statuses.Name,
								 ProjectName = projects.Name
				
			 } );
		}
		/// <summary>
		/// Gets the features.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		private static IEnumerable GetFeatures( OTDataDataContext data, Int32 userId )
		{
			return ( from features in data.Features
					 join projects in data.Projects
						 on features.ProjectId equals projects.ProjectId
					 join priorities in data.PriorityTypes
						 on features.PriorityTypeId equals priorities.PriorityTypeId
					 join statuses in data.StatusTypes
						 on features.StatusTypeId equals statuses.StatusTypeId
					 where features.AssignedToId == userId
					 select new
					 {
						 IssueId = features.FeatureId,
						 features.Name,
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
		private static IEnumerable GetIssues( OTDataDataContext data, Int32 userId, String projectName )
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
		/// Gets the defect description.
		/// </summary>
		/// <param name="defects">The defects.</param>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		private static Object GetDefectDescription(Defect defects, String tag)
		{
			switch (tag)
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
		/// <summary>
		/// Gets the feature description.
		/// </summary>
		/// <param name="feature">The feature.</param>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		private static Object GetFeatureDescription( Feature feature, String tag )
		{
			switch( tag )
			{
				case DESCRIPTIONS_NAME:
					return feature.Description;
				case NOTES_NAME:
					return feature.Notes;
				case ATACHMENTS_NAME:
					return feature.FeatureId;
				default:
					throw new NotImplementedException();
			}
		}
		#endregion

		#region Event handlers
		private void OnSelectAll( Object sender, EventArgs args )
		{
			if( null != IssueList )
			{
				IssueList.ItemsSource = GetIssues( m_Data, m_UserId );
			}
		}
		private void OnSelectCurrent( Object sender, EventArgs args )
		{
			SelectCurrent();
		}
		private void OnInfoTabChanged( Object sender, SelectionChangedEventArgs args )
		{
			ListView view = args.Source as ListView;

			if( null != view && "AttachmentList" == view.Name )
				return;

			Object selectedItem = IssueList.SelectedItem;

			if( null != selectedItem )
			{
				var item = Utility.Cast( selectedItem, new
				{
					IssueId = 0,
					Name = String.Empty,
					Priority = String.Empty,
					Status = String.Empty,
					ProjectName = String.Empty
				} );

				String tag = ( (FrameworkElement)TabInfo.SelectedItem ).Tag.ToString();
				var result = GetTabInfo( item.IssueId, tag );

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
			if( IsInitialized )
			{
				RadioButton radio = (RadioButton)sender;

				if( radio.IsInitialized )
				{
					Int32 tag = Int32.Parse( radio.Tag.ToString() );

					switch( tag )
					{
						case DEFECT_TAG:
							m_IssueType = IssueType.Defect;
							RepTabItem.Visibility = Visibility.Visible;
							break;
						case FEATURE_TAG:
							m_IssueType = IssueType.Feature;
							RepTabItem.Visibility = Visibility.Collapsed;
							break;
						default:
							throw new NotImplementedException();
					}
				}

				RefreshIssueList();
			}
		}
		private void OnGetAttachment(Object sender, MouseButtonEventArgs arg)
		{
			Object selectedItem = AttachmentList.SelectedItem;

			if (null != AttachmentList.SelectedItem)
			{
				var item = Utility.Cast(selectedItem, new
				{
					AttachmentId = 0,
					FileName = String.Empty,
					AttachDate = DateTime.Now,
					Description = String.Empty
				});

				Int32 id = item.AttachmentId;

				var result = from attachments in m_Data.Attachments
				             where attachments.AttachmentId == id
				             select new
				             {
								attachments.FileData,
				                attachments.FileName
				             };

				int count = result.Count();

				if( 1 == count )
				{
					var res = result.ToList();
					Binary binary = res[0].FileData;
					String fileName = res[0].FileName;
					SaveFileDialog saveFileDialog = new SaveFileDialog
					{
						Filter = "All files (*.*)|*.*",
						RestoreDirectory = true,
						FileName = fileName
					};

					if (true == saveFileDialog.ShowDialog())
					{
						Stream stream;

						if ((stream = saveFileDialog.OpenFile()) != null)
						{
							BinaryWriter binaryWriter = new BinaryWriter(stream);
							byte[] b = binary.ToArray();

							for (int i = 0, cnt = b.Count(); i < cnt; ++i)
							{
								binaryWriter.Write(b[ i ]);
							}

							binaryWriter.Close();
							stream.Close();
						}
					}

				}
				else if( 1 < count )
				{
					throw new ApplicationException( "Double row in data base" );
				}
			}
		}
		private void OnRefreshIssueList(Object sender, EventArgs args)
		{
			SelectCurrent();
		}
		private void OnLogout(Object sender, EventArgs args)
		{
			DialogResult = true;
		}
		#endregion
	}
}