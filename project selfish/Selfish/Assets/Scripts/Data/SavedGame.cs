using System;

[Serializable]
public class SavedGame {

	public int CurrentLevel { get; set; }
	public int[] Scores { get; set; }
	public int TotalScore { get; set; }

	// the provided scores array is copied here, allowing it to be reused for another series of scores 
	public SavedGame (int currentLevel, int[] scores)
	{
		CurrentLevel = currentLevel;
		Scores = (int[])scores.Clone();
		// total score is only for the advanced levels
		for (int i = GameController.instance.advancedLevelToSceneOffset; i < Scores.Length; i++)
			TotalScore += Scores[i];
	}

}
