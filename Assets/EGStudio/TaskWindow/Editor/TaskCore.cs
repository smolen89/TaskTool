// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
/*
	History:
	12-06-2018 v1.0.0 - Release
	17-07-2018 v1.0.1 - Fixes:
						- Ukryto zakładkę Settings, nie jest narazie potrzebna a nie ma opcji tam.
						- Skasowano komentarze które nic nie wnoszą.
*/
//---
using System;
using System.IO;
using EG.TaskWindow.Enums;
using UnityEngine;

//---
namespace EG.TaskWindow
{
	/// <summary>
	/// Głowna klasa Task Window.
	/// </summary>
	public class TaskCore
	{
		public TaskCore()
		{
			hasProLicense = Application.HasProLicense();
			filePath = Path.Combine( Application.persistentDataPath, "Tasks.json" );

			Database = new TaskDatabase();
		}

		/// <summary>
		/// Nazwa Okna.
		/// </summary>
		internal const string AppName = "Task Tool";

		/// <summary>
		/// Wersja aplikacji.
		/// </summary>
		internal const string AppVersion = "v1.0.1";

		/// <summary>
		/// Ścieżka do menu.
		/// </summary>
		internal const string MenuItem = "Tools/Task Tool";

		/// <summary>
		/// Kontener zadań.
		/// </summary>
		internal TaskDatabase Database;

		/// <summary>
		/// Przełącznik do zmiany stylu GUI między Jasnym a Ciemnym stylem Unity.
		/// </summary>
		internal bool hasProLicense = false;

		/// <summary>
		/// Ścieżka pliku zawartości zadań.
		/// </summary>
		internal string filePath = string.Empty;

		/// <summary>
		/// Czy ma pokazywać zadania zarchiwizowane na liście zadań.
		/// </summary>
		internal bool showArchivedTasks = false;

		/// <summary>
		/// Czy pokazać belkę filtrowania w oknie listy zadań.
		/// </summary>
		internal bool showFilterToolbar = false;

		/// <summary>
		/// Określa co ma wyświetlać w ministatusie na liście zadań.
		/// </summary>
		internal ElementDescritions elementDescription = ElementDescritions.State;

		/// <summary>
		/// Określa czy w obecnej chwili jest wyświetlana lista, podgląd zadania bądź w późniejszych
		/// wersjach okno ustawień
		/// </summary>
		internal WindowStates windowState = WindowStates.List;

		// Popupy filtrowania
		internal FilterStates filterState = FilterStates.All;
		internal FilterTypes filterType = FilterTypes.All;
		internal FilterActivities filterActivity = FilterActivities.All;
		internal FilterPriorities filterPriority = FilterPriorities.All;
		internal string filterTags = "";

		/// <summary>
		/// wybrany element do wyświetlenia w podglądzie
		/// </summary>
		internal int currentIndex;

		/// <summary>
		/// Ładowanie Zadań z pliku.
		/// </summary>
		internal void LoadContent()
		{
			string serializedText = string.Empty;

			if (File.Exists( filePath ) == false)
			{
				Debug.LogWarning( $"Task Tool -- Nie odnaleziono pliku zawierającego zadania. -> {filePath}" );
			}
			else
			{
				try
				{
					serializedText = File.ReadAllText( filePath );
				}
				catch
				{
					Debug.LogError( $"Task Tool -- Plik z zadaniami jest nie czytelny. -> {filePath}" );
				}
			}
			if (serializedText != string.Empty)
			{
				Database = JsonUtility.FromJson<TaskDatabase>( serializedText );
			}
		}

		/// <summary>
		/// Zapisanie zadań do pliku.
		/// </summary>
		internal void SaveContent()
		{
			string jsonString = JsonUtility.ToJson( Database, true );
			try
			{
				using (StreamWriter writer = new StreamWriter( filePath, false ))
					writer.WriteLine( jsonString );
			}
			catch (Exception e)
			{
				Debug.LogError( $"Task Tool -- Nie mogę zapisać zadań do pliku. Powód: " + e.Message );
			}
		}

		internal bool checkFilters( Task task )
		{
			if (task.State == TaskStates.Archived && !showArchivedTasks)
			{
				return false;
			}

			// jeśli jest schowany pasek z filtrem to nie filtruje zawartości
			if (!showFilterToolbar)
			{
				return true;
			}

			if (filterState == FilterStates.All || (int)task.State == (int)filterState)
			{
				if (filterType == FilterTypes.All || (int)task.Type == (int)filterType)
				{
					if (filterActivity == FilterActivities.All || (int)task.Activity == (int)filterActivity)
					{
						if (filterPriority == FilterPriorities.All || (int)task.Priority == (int)filterPriority)
						{
							if (filterTags == "" || checkTags( task.Tag, filterTags ))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private bool checkTags( string elementTag, string filter )
		{
			string[] tempString = elementTag.ToString().Split( char.Parse( ";" ) );
			foreach (string tmp in tempString)
			{
				foreach (string filterTemp in filter.ToString().Split( char.Parse( ";" ) ))
				{
					if (filterTemp.ToLower() == tmp.ToLower())
					{
						return true;
					}
				}
			}
			return false;
		}

		internal Color colorPriority( Task _Element )
		{
			// task - blue
			if (_Element.Type == TaskTypes.Task)
			{
				switch (_Element.Priority)
				{
					case TaskPriorities.None:
						return new Color( 0.80f, 0.80f, 0.86f );

					case TaskPriorities.Low:
						return new Color( 0.70f, 0.78f, 0.86f );

					case TaskPriorities.Mid:
						return new Color( 0.70f, 0.95f, 1.00f );

					case TaskPriorities.Hight:
						return new Color( 0.40f, 0.85f, 1.00f );

					case TaskPriorities.VeryHight:
						return new Color( 0.00f, 0.75f, 1.00f );
				}
			}

			// red - bug
			if (_Element.Type == TaskTypes.Bug)
			{
				switch (_Element.Priority)
				{
					case TaskPriorities.None:
						return new Color( 0.86f, 0.80f, 0.80f );

					case TaskPriorities.Low:
						return new Color( 0.85f, 0.70f, 0.70f );

					case TaskPriorities.Mid:
						return new Color( 1.00f, 0.70f, 0.70f );

					case TaskPriorities.Hight:
						return new Color( 1.00f, 0.40f, 0.40f );

					case TaskPriorities.VeryHight:
						return new Color( 1.00f, 0.00f, 0.00f );
				}
			}

			// Tickets - yellow
			if (_Element.Type == TaskTypes.Ticket)
			{
				switch (_Element.Priority)
				{
					case TaskPriorities.None:
						return new Color( 0.86f, 0.86f, 0.80f );

					case TaskPriorities.Low:
						return new Color( 0.86f, 0.86f, 0.70f );

					case TaskPriorities.Mid:
						return new Color( 1.00f, 1.00f, 0.70f );

					case TaskPriorities.Hight:
						return new Color( 1.00f, 1.00f, 0.40f );

					case TaskPriorities.VeryHight:
						return new Color( 1.00f, 1.00f, 0.00f );
				}
			}

			// Balance - green
			if (_Element.Type == TaskTypes.Balance)
			{
				switch (_Element.Priority)
				{
					case TaskPriorities.None:
						return new Color( 0.80f, 0.86f, 0.80f );

					case TaskPriorities.Low:
						return new Color( 0.70f, 0.86f, 0.70f );

					case TaskPriorities.Mid:
						return new Color( 0.70f, 1.00f, 0.70f );

					case TaskPriorities.Hight:
						return new Color( 0.40f, 1.00f, 0.40f );

					case TaskPriorities.VeryHight:
						return new Color( 0.00f, 1.00f, 0.00f );
				}
			}

			return new Color( 0.7f, 0.7f, 0.7f );
		}

		internal void RemoveAllTasks()
		{
			Database.Items.Clear();
		}

		internal string GetElementDescription( Task task )
		{
			switch (elementDescription)
			{
				case ( ElementDescritions.Type ):
					return task.Type.ToString();

				case ( ElementDescritions.State ):
					return task.State.ToString();

				case ( ElementDescritions.Priority ):
					return task.Priority.ToString();

				case ( ElementDescritions.Activity ):
					return task.Activity.ToString();

				case ( ElementDescritions.Comments ):
					return "Com.: " + task.Comments.Count.ToString();
			}
			return "";
		}

		internal float GetScrolledAreaHeight( float height )
		{
			return height - 18f + ( showFilterToolbar && windowState == WindowStates.List ? -45f : 0f );
		}

		internal void FillTemporaryTasks()
		{
			for (int types = 0; types < System.Enum.GetNames( typeof( TaskTypes ) ).Length; types++)
			{
				for (int priorities = 0; priorities < System.Enum.GetNames( typeof( TaskPriorities ) ).Length; priorities++)
				{
					Task task = new Task()
					{
						Name = $"Type: {types}, Prio: {priorities}.",
						Priority = (TaskPriorities)priorities,
						Type = (TaskTypes)types
					};
					Database.Items.Add( task );
				}
			}
		}
	}
}