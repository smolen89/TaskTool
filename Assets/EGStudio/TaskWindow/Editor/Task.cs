// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
using System.Collections.Generic;
using EG.TaskWindow.Enums;

//---
namespace EG.TaskWindow
{
	/// <summary>
	/// Informacje o zadaniu
	/// </summary>
	[System.Serializable]
	public class Task
	{
		public Task()
		{
			Comments = new List<TaskComment>();
			Name = "(no name)";
			Tag = "";
			Progress = 0;
			Priority = TaskPriorities.None;
			State = TaskStates.None;
			Type = TaskTypes.None;
			Activity = TaskActivities.None;
		}

		/// <summary>
		/// Nazwa zadania
		/// </summary>
		public string Name;

		/// <summary>
		/// Tagi zadania
		/// </summary>
		public string Tag;

		/// <summary>
		/// Postęp zadania
		/// </summary>
		public int Progress;

		/// <summary>
		/// Priorytet zadania
		/// </summary>
		public TaskPriorities Priority;

		/// <summary>
		/// Stan zadania
		/// </summary>
		public TaskStates State;

		/// <summary>
		/// Aktywność zadania
		/// </summary>
		public TaskActivities Activity;

		/// <summary>
		/// Typ zadania
		/// </summary>
		public TaskTypes Type;

		/// <summary>
		/// Lista komentarzy.
		/// </summary>
		public List<TaskComment> Comments;
	}
}