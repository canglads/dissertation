using System;

[Serializable]
public class SavedGamesData {

	public SavedGame[] Games { get; set; }

	public SavedGamesData (SavedGame[] games)
	{
		Games = games;
	}

}
