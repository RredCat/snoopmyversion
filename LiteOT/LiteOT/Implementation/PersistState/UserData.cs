using System;
using System.Runtime.Serialization;

namespace LiteOT
{
	/// <summary>
	/// Presents user data for save.
	/// </summary>
	public class UserData : ISerializable
	{
		#region Properties
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public String UserName
		{
			get;
			set;
		}
		public String Password
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this instance is saved.
		/// </summary>
		/// <value><c>true</c> if this instance is saved; otherwise, <c>false</c>.</value>
		public Boolean IsSaved
		{
			get; set;
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
		}
		#endregion
	}
}