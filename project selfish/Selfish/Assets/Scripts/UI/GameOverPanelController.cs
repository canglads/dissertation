using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class GameOverScorePanel {
	public GameObject panel;
	public Text name;
	public Text score;
}

public class GameOverPanelController : MonoBehaviour {

	public Text wonLostText;
	public GameOverScorePanel[] scorePanels;
	public string wonMessage = "YOU WON!";
	public string lostMessage = "YOU LOST!";
	public string drawMessage = "DRAW!";

	UIController uic;

	void Start ()
	{
		uic = UIController.instance;
	}

	void OnEnable ()
	{
		int result = MultiplayerLevelController.instance.GetResult ();
		if (result == 1)
			wonLostText.text = wonMessage;
		else if (result == 0)
			wonLostText.text = drawMessage;
		else
			wonLostText.text = lostMessage;
		MultiplayerScore[] scores = MultiplayerScoreData.instance.Scores;
		int count = 0;
		foreach (GameOverScorePanel panel in scorePanels) {
			if (count >= scores.Length)
				panel.panel.SetActive (false);
			else {
				panel.panel.SetActive (true);
				panel.name.text = MultiplayerController.instance.CurrentGame.Players[count].Name;
				panel.score.text = scores[count].Score.ToString();
			}
			count++;
		}
	}

	public void HandleExit ()
	{
		uic.PlayButtonPressAudio ();
		MultiplayerController.instance.HandleExitDuringGame ();
	}

	public void HandleStats ()
	{
		uic.PlayButtonPressAudio ();
		uic.ShowStatsPanel (MultiplayerLevelController.instance.GetStats());
	}

}
