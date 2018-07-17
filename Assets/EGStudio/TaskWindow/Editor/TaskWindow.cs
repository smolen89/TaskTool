// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
using UnityEditor;
using UnityEngine;

//---
namespace EG.TaskWindow
{
	public class TaskWindow : EditorWindow
	{
		private TaskCore Core;
		private TaskGUI CoreGUI;

		private Vector2 scrollBarPosition;
		private Vector2 minimumWindowSize = new Vector2( 500, 200 );

		[MenuItem( TaskCore.MenuItem )]
		private static void Init()
		{
			TaskWindow window = GetWindow( typeof( TaskWindow ) ) as TaskWindow;

			window.titleContent.text = TaskCore.AppName;
			window.minSize = window.minimumWindowSize;

			window.Show();
		}

		private void Awake()
		{
			Validation();
		}

		public void OnDestroy()
		{
			if (Core != null)
			{
				Core.SaveContent();
			}
		}

		private void OnGUI()
		{
			if (CoreGUI == null || Core == null)
			{
				GUILayout.Label( "Core or CoreGUI is not loaded!!" );
				Validation();
				return;
			}

			CoreGUI.DrawToolbar();

			scrollBarPosition = EditorGUILayout.BeginScrollView(
				scrollBarPosition,
				GUILayout.Width( position.width ),
				GUILayout.Height( Core.GetScrolledAreaHeight( position.height ) ) );
			{
				if (Core.Database != null)
				{
					CoreGUI.DrawContent();
				}
			}
			GUILayout.EndScrollView();
		}

		private void Validation()
		{
			if (Core == null)
			{
				Core = new TaskCore();
				Core.LoadContent();
			}
			if (CoreGUI == null)
			{
				CoreGUI = new TaskGUI( Core );
			}
		}
	}
}