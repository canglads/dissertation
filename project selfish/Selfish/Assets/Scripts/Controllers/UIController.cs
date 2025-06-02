using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIController : MonoBehaviour
{
	// one instance at a time
	public static UIController instance;
	//public ParameterController param;

	public AudioClip buttonPressAudio;
	public Button selfButton;
	public Button crossButton;
	public TouchPanelController touchPanelController;

	public bool DisplayImmPanels { get; set; }

	GameObject messagePanel;
	Text messageText;
	GameObject survivedPanel;
	GameObject deadPanel;
	GameObject pausedPanel;
	Text scoreText;
	Text remainingLevelTimeText;
	Text remainingLifetimeText;
	Text currentTempText;
	GameObject statsPanel;
	DamageImageController damageImageController;
	GameObject warningPanel;
	Text warningText;
	protected GameObject ownImmPanel;
	GameObject maleImmPanel;
	GameObject eatPanel;
	GameObject preventInteractionPanel;
	GameObject highScorePanel;
	StatsPanelController statsPanelController;
	BlackoutController blackoutController;
	GameObject saveGamePanel;
	
	void Awake ()
	{
		instance = this;
		messagePanel = transform.Find ("MessagePanel").gameObject;
		messageText = messagePanel.transform.Find ("Inset").Find ("MessageText").GetComponent<Text> ();
		survivedPanel = transform.Find ("SurvivedPanel").gameObject;
		deadPanel = transform.Find ("DeadPanel").gameObject;
		pausedPanel = transform.Find ("PausedPanel").gameObject;
		scoreText = transform.Find ("TopRightPanel").Find ("ScoreText").GetComponent<Text> ();
		GameObject timePanel = transform.Find ("TimePanel").gameObject;
		remainingLevelTimeText = timePanel.transform.Find ("LevelPanel").Find ("LevelText").GetComponent<Text> ();
		remainingLifetimeText = timePanel.transform.Find ("LifePanel").Find ("LifeText").GetComponent<Text> ();
		statsPanel = transform.Find ("StatsPanel").gameObject;
		warningPanel = transform.Find ("WarningPanel").gameObject;
		warningText = warningPanel.transform.Find ("WarningText").GetComponent<Text> ();
		selfButton = transform.Find ("SelfButton").GetComponent<Button> ();
		crossButton = transform.Find ("CrossButton").GetComponent<Button> ();
		ownImmPanel = transform.Find ("OwnImmPanel").gameObject;
		maleImmPanel = transform.Find ("MaleImmPanel").gameObject;
		eatPanel = transform.Find ("EatPanel").gameObject;
		preventInteractionPanel = transform.Find ("PreventInteractionPanel").gameObject;
		highScorePanel = transform.Find ("HighScorePanel").gameObject;
		blackoutController = transform.Find ("Blackout").GetComponent<BlackoutController> ();
		statsPanelController = statsPanel.GetComponent<StatsPanelController> ();
		touchPanelController = transform.Find ("TouchPanel").GetComponent<TouchPanelController> ();
		damageImageController = transform.Find ("DamageImage").GetComponent<DamageImageController> ();
		saveGamePanel = transform.Find ("SaveGamePanel").gameObject;
		//param = ParameterController.instance;
	}

	public virtual void ShowOwnImmPanel ()
	{
		ownImmPanel.SetActive (true);
		ownImmPanel.GetComponent<ImmPanelController> ().ShowImmunities (
			GameController.instance.sc.CurrentPlayer.GetComponent<ImmuneSystem>().Immunities
			);
	}

	public void ShowMessage (string text)
	{
		messageText.text = text;
		messagePanel.SetActive (true);
	}

	public void HideMessage ()
	{
		messageText.text = "";
		messagePanel.SetActive (false);
	}

	public void ShowSurvivedPanel (SurvivedPanelController.Level level)
	{
		survivedPanel.SetActive (true);
		survivedPanel.GetComponent<SurvivedPanelController>().SetDisplay(level);
	}

	public void HideSurvivedPanel ()
	{
		survivedPanel.SetActive (false);
	}

	public void ShowDeadPanel ()
	{
		deadPanel.SetActive (true);
	}

	public void HideDeadPanel ()
	{
		deadPanel.SetActive (false);
	}

	public void ShowPausedPanel ()
	{
		pausedPanel.SetActive (true);
	}

	public void HidePausedPanel ()
	{
		pausedPanel.SetActive (false);
	}

	public void ShowStatsPanel (Stats stats)
	{
		statsPanelController.SetStats (stats);
		statsPanel.SetActive (true);
	}
	
	public void HideStatsPanel ()
	{
		statsPanel.SetActive (false);
	}

	public void ShowHighScorePanel (int score)
	{
		highScorePanel.GetComponent<HighScorePanelController> ().Show (score);
	}

	public void ShowSaveGamePanel ()
	{
		saveGamePanel.SetActive (true);
	}

	public void DisplayScore (int score)
	{
		scoreText.text = "SCORE " + score;
	}

	public void DisplayLevelRemainingTime (float time)
	{
		remainingLevelTimeText.text = FormatTime(time);
	}

	public void DisplayRemainingLifetime (float time)
	{
		remainingLifetimeText.text = time < 0 ? FormatTime(0) : FormatTime(time);
	}

	public void EnableTempPanel ()
	{
		GameObject timePanel = transform.Find ("TimePanel").gameObject;
		currentTempText = timePanel.transform.Find ("TempPanel").Find ("TempText").GetComponent<Text> ();
	}
	public void DisplayCurrentTemp (int temp)
	{
		currentTempText.text = temp + " °C";
	}

	public void HandlePlayerMature ()
	{
		selfButton.gameObject.SetActive (true);
		crossButton.gameObject.SetActive (true);
	}

	public void ShowMaleImm (GameObject male)
	{
		if (!DisplayImmPanels || male == null)
			return;
		List<PathogenType> immunities = male.GetComponent<ImmuneSystem> ().Immunities;
		maleImmPanel.SetActive (true);
		maleImmPanel.GetComponent<ImmPanelController> ().ShowImmunities (immunities);
	}

	string FormatTime (float time)
	{
		int minutes = (int)(time / 60);
		string seconds = ((int)(time) % 60).ToString ("D2");
		return minutes + ":" + seconds;
	}

	public void UpdateFood (int value)
	{
		if (value == 0) {
			eatPanel.SetActive (false);
		} else {
			eatPanel.GetComponent<EatPanelController>().UpdateFood(value);
		}
	}

	public void PlayButtonPressAudio ()
	{
		SoundController.instance.PlaySingle (buttonPressAudio);
	}

	public void PreventInteraction()
	{
		preventInteractionPanel.SetActive(true);
	}

	public void EnableInteraction()
	{
		preventInteractionPanel.SetActive(false);
	}

	public void ShowWarningText (string text) 
	{
		warningText.text = text;
		warningPanel.SetActive (true);
	}

	public void HideMaleImmPanel ()
	{
		maleImmPanel.SetActive (false);
	}

	public void Blackout ()
	{
		blackoutController.Blackout ();
	}

	public void StartDamageImageFlashing ()
	{
		damageImageController.Flashing = true;
	}

	public void StopDamageImageFlashing ()
	{
		damageImageController.Flashing = false;
	}

	public void PauseFlashingDamageImage (bool value)
	{
		damageImageController.Paused = value;
	}

	public void HandleSwitchCurrentPlayer ()
	{
		GameObject currentPlayer = GameController.instance.sc.CurrentPlayer;
		if (currentPlayer.GetComponent<FishReproduction>().mature)
			HandlePlayerMature ();
		else {
			HideMatureComponents ();
			eatPanel.SetActive(true);
		}
		warningPanel.SetActive (false);
		StopDamageImageFlashing();
		Blackout();
		UpdateFood (currentPlayer.GetComponent<FishGrowth> ().FoodCounter);
	}

	void HideMatureComponents ()
	{
		selfButton.interactable = true;//default state true
		crossButton.interactable = false;//default state false
		selfButton.gameObject.SetActive (false);
		crossButton.gameObject.SetActive (false);
		ownImmPanel.SetActive (false);
		maleImmPanel.SetActive (false);
	}
	
}
