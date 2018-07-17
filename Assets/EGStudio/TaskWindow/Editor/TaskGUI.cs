// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
using EG.TaskWindow.Enums;
using UnityEditor;
using UnityEngine;

//---
namespace EG.TaskWindow
{
	public class TaskGUI
	{
		public TaskGUI( TaskCore core )
		{
			this.Core = core;
		}

		private TaskCore Core;

		public void DrawToolbar()
		{
			switch (Core.windowState)
			{
				case WindowStates.List:
					DrawToolbarForList();
					break;

				case WindowStates.Element:
					DrawToolbarForElement();
					break;

				case WindowStates.Settings:
					DrawToolbarForSettings();
					break;

				default:
					break;
			}
		}

		public void DrawContent()
		{
			switch (Core.windowState)
			{
				case WindowStates.List:
					DrawContentForList();
					break;

				case WindowStates.Element:
					DrawContentForElement();
					break;

				case WindowStates.Settings:
					DrawContentForSettings();
					break;

				default:
					break;
			}
		}

		#region Task List

		private void DrawToolbarForList()
		{
			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
			{
				if (GUILayout.Button( "Add Task", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ))
				{
					Core.Database.Items.Add( new Task() );
				}

				Core.showFilterToolbar = GUILayout.Toggle(
					Core.showFilterToolbar, "Filters",
					EditorStyles.toolbarButton,
					GUILayout.ExpandWidth( false ) );

				Core.elementDescription = (ElementDescritions)EditorGUILayout.EnumPopup(
					Core.elementDescription,
					EditorStyles.toolbarPopup,
					GUILayout.Width( 100 ) );

				Core.showArchivedTasks = GUILayout.Toggle(
					Core.showArchivedTasks, "Show Archived Task",
					EditorStyles.toolbarButton,
					GUILayout.ExpandWidth( false ) );

				GUILayout.FlexibleSpace();
				if (GUILayout.Button( "Settings", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ))
				{
					Core.windowState = WindowStates.Settings;
				}
				GUILayout.Label( TaskCore.AppVersion );
			}
			EditorGUILayout.EndHorizontal();

			#region filter window

			if (Core.showFilterToolbar)
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.BeginVertical();
					{
						EditorGUILayout.LabelField( "Priority:",
							EditorStyles.miniLabel,
							GUILayout.Width( 50f ) );

						Core.filterPriority = (FilterPriorities)EditorGUILayout.EnumPopup(
							Core.filterPriority,
							GUILayout.ExpandWidth( false ) );
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical();
					{
						EditorGUILayout.LabelField( "Type:",
							EditorStyles.miniLabel,
							GUILayout.Width( 50f ) );

						Core.filterType = (FilterTypes)EditorGUILayout.EnumPopup(
							Core.filterType,
							GUILayout.ExpandWidth( false ) );
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical();
					{
						EditorGUILayout.LabelField( "Activity:",
							EditorStyles.miniLabel,
							GUILayout.Width( 50f ) );

						Core.filterActivity = (FilterActivities)EditorGUILayout.EnumPopup(
							Core.filterActivity,
							GUILayout.ExpandWidth( false ) );
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical();
					{
						EditorGUILayout.LabelField( "State:",
							EditorStyles.miniLabel,
							GUILayout.Width( 50f ) );

						Core.filterState = (FilterStates)EditorGUILayout.EnumPopup(
							Core.filterState,
							GUILayout.ExpandWidth( false ) );
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical();
					{
						EditorGUILayout.LabelField( "Tag:",
							EditorStyles.miniLabel,
							GUILayout.Width( 50f ) );

						Core.filterTags = EditorGUILayout.TextField( Core.filterTags );
					}
					EditorGUILayout.EndVertical();

					GUILayout.FlexibleSpace();
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Separator();
			}

			#endregion filter window
		}

		private void DrawContentForList()
		{
			if (Core.Database.Count < 1)
			{
				return;
			}

			foreach (Task tempElement in Core.Database.Items)
			{
				if (Core.checkFilters( tempElement ))
				{
					GUI.backgroundColor = Core.colorPriority( tempElement );
					EditorGUILayout.BeginVertical( "Box" );
					{
						EditorGUILayout.LabelField( tempElement.Name,
							Core.hasProLicense ? EditorStyles.whiteBoldLabel : EditorStyles.boldLabel,
							GUILayout.ExpandWidth( true ),
							GUILayout.Height( 20f ) );

						EditorGUILayout.BeginHorizontal();
						{
							if (GUILayout.Button( "X", GUILayout.ExpandWidth( false ), GUILayout.Height( 14f ) ))
							{
								Core.Database.Remove( tempElement );
								Core.SaveContent();
								return;
							}

							if (GUILayout.Button( "Edit", GUILayout.Width( 100f ), GUILayout.Height( 14f ) ))
							{
								Core.currentIndex = Core.Database.IndexOf( tempElement );
								Core.windowState = WindowStates.Element;
							}

							EditorGUILayout.LabelField(
								tempElement.Progress + "%",
								EditorStyles.miniLabel,
								GUILayout.Width( 35f ) );

							if (Core.GetElementDescription( tempElement ) != "")
							{
								EditorGUILayout.LabelField(
									"| " + Core.GetElementDescription( tempElement ),
									EditorStyles.miniLabel,
									GUILayout.Width( 88f ) );
							}
							else
							{
								EditorGUILayout.LabelField(
									"  ",
									EditorStyles.miniLabel,
									GUILayout.Width( 88f ) );
							}

							if (tempElement.Tag != "")
							{
								EditorGUILayout.LabelField(
									"| Tags: " + tempElement.Tag.Replace( ";", ", " ),
									EditorStyles.wordWrappedMiniLabel );
							}
						}
						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.EndVertical();
				}
			}
		}

		#endregion Task List

		#region Task Element

		private void DrawToolbarForElement()
		{
			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
			{
				if (GUILayout.Button( "Add comment", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ))
				{
					TaskComment tempComment = new TaskComment();
					tempComment.Date = ( string.Format(
						"{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}",
						System.DateTime.Now.Year,
						System.DateTime.Now.Month,
						System.DateTime.Now.Day,
						System.DateTime.Now.Hour,
						System.DateTime.Now.Minute,
						System.DateTime.Now.Second ) );
					tempComment.Comment = "";
					Core.Database.Items[Core.currentIndex].Comments.Add( tempComment );
				}

				if (GUILayout.Button( "Back", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ))
				{
					Core.SaveContent();
					Core.windowState = WindowStates.List;
				}
				GUILayout.FlexibleSpace();
				GUILayout.Label( TaskCore.AppVersion );
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawContentForElement()
		{
			EditorGUILayout.LabelField( "Title:", EditorStyles.miniLabel );
			Core.Database[Core.currentIndex].Name = EditorGUILayout.TextField( Core.Database[Core.currentIndex].Name );
			EditorGUILayout.Separator();

			#region Belka z ustawieniami

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField( "Progress:",
						EditorStyles.miniLabel,
						GUILayout.Width( 55f ) );

					Core.Database[Core.currentIndex].Progress =
						EditorGUILayout.IntSlider( Core.Database[Core.currentIndex].Progress, 0, 100 );
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField( "Priority:",
						EditorStyles.miniLabel,
						GUILayout.Width( 50f ) );

					Core.Database[Core.currentIndex].Priority =
						(TaskPriorities)EditorGUILayout.EnumPopup( Core.Database[Core.currentIndex].Priority );
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField( "Type:",
						EditorStyles.miniLabel,
						GUILayout.Width( 50f ) );

					Core.Database[Core.currentIndex].Type =
						(TaskTypes)EditorGUILayout.EnumPopup( Core.Database[Core.currentIndex].Type );
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField( "Activity:",
						EditorStyles.miniLabel,
						GUILayout.Width( 50f ) );

					Core.Database[Core.currentIndex].Activity =
						(TaskActivities)EditorGUILayout.EnumPopup( Core.Database[Core.currentIndex].Activity );
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField( "State:",
						EditorStyles.miniLabel,
						GUILayout.Width( 50f ) );

					Core.Database[Core.currentIndex].State =
						(TaskStates)EditorGUILayout.EnumPopup( Core.Database[Core.currentIndex].State );
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				{
					EditorGUILayout.LabelField( "Tags:",
						EditorStyles.miniLabel,
						GUILayout.Width( 50f ) );

					Core.Database[Core.currentIndex].Tag =
						EditorGUILayout.TextField( Core.Database[Core.currentIndex].Tag );
				}
				EditorGUILayout.EndVertical();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			#endregion Belka z ustawieniami

			EditorGUILayout.Separator();

			foreach (TaskComment tempComment in Core.Database[Core.currentIndex].Comments)
			{
				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button( "X", EditorStyles.miniButton, GUILayout.ExpandWidth( false ) ))
				{
					Core.Database[Core.currentIndex].Comments.Remove( tempComment );
					Core.SaveContent();
					return;
				}
				EditorGUILayout.LabelField( tempComment.Date, EditorStyles.miniLabel, GUILayout.Width( 125f ) );
				tempComment.Comment = EditorGUILayout.TextArea( tempComment.Comment );
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
			}
		}

		#endregion Task Element

		#region Task Settings

		private void DrawToolbarForSettings()
		{
			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
			{
				GUILayout.FlexibleSpace();
				if (GUILayout.Button( "Back", EditorStyles.toolbarButton, GUILayout.ExpandWidth( false ) ))
				{
					Core.windowState = WindowStates.List;
				}
				GUILayout.Label( TaskCore.AppVersion );
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawContentForSettings()
		{
			EditorGUILayout.LabelField( "Not Implemented yet." );

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.BeginVertical( "Box" );
				{
					SettingLeftPanel();
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical( "Box" );
				{
					SettingRightPanel();
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		private void SettingLeftPanel()
		{
			if (GUILayout.Button( "Fill test tasks" ))
			{
				Core.FillTemporaryTasks();
			}
		}

		private void SettingRightPanel()
		{
			if (GUILayout.Button( "Remove test tasks" ))
			{
				Core.RemoveAllTasks();
			}
		}

		#endregion Task Settings
	}
}