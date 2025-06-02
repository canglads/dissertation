using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class LoadPanelRow {
	public Text level;
	public Text score;
	public Button button;
}

public class LoadPanelController : MonoBehaviour {

	public LoadPanelRow[] rows;  

	void OnEnable ()
	{
		int count = 0;
		foreach (SavedGame game in GameController.instance.SavedGames) {
			if (game != null) {
				LoadPanelRow row = rows[count];
				row.level.text = game.CurrentLevel.ToString();
				row.score.text = game.TotalScore.ToString();
				row.button.interactable = true;
			}
			count++;
		}
	}

	public void HandleBack ()
	{
		gameObject.SetActive (false);
	}

	public void HandleLoad (int game)
	{
		GameController.instance.LoadGame (game);
	}

}
