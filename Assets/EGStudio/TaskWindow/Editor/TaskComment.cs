// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
namespace EG.TaskWindow
{
	/// <summary>
	/// Element zawierający dane o komentarzu
	/// </summary>
	[System.Serializable]
	public class TaskComment
	{
		public TaskComment()
		{
			Date = string.Empty;
			Comment = string.Empty;
		}

		/// <summary>
		/// Sformatowana data komentarza
		/// </summary>
		public string Date;

		/// <summary>
		/// Treść komentarza
		/// </summary>
		public string Comment;
	}
}