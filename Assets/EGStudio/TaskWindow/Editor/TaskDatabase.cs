// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
using System.Collections.Generic;

//---
namespace EG.TaskWindow
{
	[System.Serializable]
	public class TaskDatabase
	{
		public TaskDatabase()
		{
			Items = new List<Task>();
		}

		public List<Task> Items;

		public int Count => Items.Count;

		public Task this[int index]
		{
			get { return Items[index]; }
			set { Items[index] = value; }
		}

		internal void Remove( Task task )
		{
			Items.Remove( task );
		}

		internal int IndexOf( Task task )
		{
			return Items.IndexOf( task );
		}
	}
}