// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved. Create by Ebbi Gebbi
//---
namespace EG.TaskWindow.Enums
{
	/// <summary>
	/// Typ zadania
	/// </summary>
	public enum TaskTypes
	{
		// Numeracja jest potrzebna do porównywania podczas filtrowania, na belce z filtrowaniem jest
		// dodatkowa opcja 'All' która nie jest dostępna podczas edycji Zadania.
		None = 0,
		Task = 1,
		Bug = 2,
		Balance = 3,
		Ticket = 4,
	}
}