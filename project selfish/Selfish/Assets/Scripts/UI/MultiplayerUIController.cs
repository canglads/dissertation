using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class ScorePanel {
	public Text name;
	public Text score;
	public void Clear ()
	{
		name.text = "";
		score.text = "";
	}
}

public class MultiplayerUIController : MonoBehaviour {

	public static MultiplayerUIController instance;

	public ScorePanel[] scorePanels; 
	public GameObject confirmExitPanel;
	public GameObject gameOverPanel;

	MultiplayerController mc;

	void Awake ()
	{
		instance = this;
		mc = MultiplayerController.instance;
		FillNamesInScorePanel ();
	}

	void FillNamesInScorePanel ()
	{
		int count = 0;
		foreach (ScorePanel scorePanel in scorePanels) {
			if (count >= mc.CurrentGame.Players.Count)
				scorePanel.Clear();
			else {
				scorePanel.name.text = mc.CurrentGame.Players[count].Name;
			}
			count++;
		}
	}

	public void HandleExit ()
	{
		UIController.instance.PlayButtonPressAudio ();
		confirmExitPanel.SetActive (true);
	}

	public void UpdateScores (MultiplayerScore[] scores)
	{
		int count = 0;
		foreach (MultiplayerScore score in scores) {
			scorePanels [count].score.text = score.Score.ToString ();
			count++;
		}
	}

	public void ShowGameOverPanel ()
	{
		gameOverPanel.SetActive (true);
	}

}
