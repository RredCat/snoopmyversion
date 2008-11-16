using System;
using System.Runtime.Serialization;

namespace LiteOT
{
	/// <summary>
	/// Presents DB data for save.
	/// </summary>
	[Serializable]
	//[SoapInclude( typeof( DBData ) )]
	//[XmlInclude( typeof( DBData ) )]
	//[SoapInclude( typeof( DBData ) )]
	//[XmlInclude( typeof( DBData ) )]
	public class DBData : ISerializable
	{
		#region Properties
		/// <summary>
		/// Gets or sets the name of the SQL server.
		/// </summary>
		/// <value>The name of the SQL server.</value>
		public String SQLServerName
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public String UserName
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public String Password
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the name of the database.
		/// </summary>
		/// <value>The name of the database.</value>
		public String DatabaseName
		{
			get;
			set;
		}
		#endregion

		#region ISerializable Members
		/// <summary>
		/// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
		/// <exception cref="T:System.Security.SecurityException">
		/// The caller does not have the required permission.
		/// </exception>
		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "SQLServerName", SQLServerName );
			info.AddValue( "UserName", UserName );
			info.AddValue( "Password", Password );
			info.AddValue( "DatabaseName", DatabaseName );
		}
		#endregion
	}
}
