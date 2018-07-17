// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
namespace EG.TaskWindow.Enums
{
	/// <summary>
	/// Stan zadania
	/// </summary>
	public enum TaskStates : int
	{
		// Numeracja jest potrzebna do porównywania podczas filtrowania, na belce z filtrowaniem jest
		// dodatkowa opcja 'All' która nie jest dostępna podczas edycji Zadania.
		None = 0,
		New = 1,
		InProgress = 2,
		Archived = 3,
		Done = 4,
		Organize = 5,
	}
}