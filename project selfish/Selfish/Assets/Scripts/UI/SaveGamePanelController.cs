using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public struct SaveGameRow {
	public Text level;
	public Text score;
	public Button button;
}

public class SaveGamePanelController : MonoBehaviour {

	public Text messageText;
	public SaveGameRow[] rows;
	public string savedText = "Saved";
	public string errorText = "Error";

	void OnEnable ()
	{
		int count = 0;
		foreach (SavedGame game in GameController.instance.SavedGames) {
			if (game != null) {
				rows[count].level.text = game.CurrentLevel.ToString();
				rows[count].score.text = game.TotalScore.ToString();
			}
			count++;
		}
	}

	public void HandleBack ()
	{
		gameObject.SetActive (false);
	}

	public void HandleSave (int slot)
	{
		if (GameController.instance.SaveGame (slot)) {
			messageText.text = savedText;
			rows[slot].level.text = (GameController.instance.CurrentLevel + 1).ToString();
			rows[slot].score.text = GameController.instance.TotalScore.ToString();
		} else {
			messageText.text = errorText;
		}
		foreach (SaveGameRow row in rows)
			row.button.interactable = false;

	}

}
