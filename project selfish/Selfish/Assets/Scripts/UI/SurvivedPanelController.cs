using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SurvivedPanelController : MonoBehaviour {

	public enum Level {Intro, Advanced, Last, Custom};

	public string scoreString = "TOTAL SCORE: ";

	GameObject survivedText;
	GameObject introLevelSurvivedText;
	GameObject masteredText;
	GameObject nextLevelButton;
	GameObject saveButton;
	GameObject exitButton;
	GameObject scoreText;
	GameObject playAgainButton;
	UIController uic;

	void Awake ()
	{
		Transform panel = transform.Find ("Panel");
		survivedText = panel.Find("SurvivedText").gameObject;
		introLevelSurvivedText = panel.Find("IntroLevelSurvivedText").gameObject;
		masteredText = panel.Find("MasteredText").gameObject;
		nextLevelButton = panel.Find ("NextLevelButton").gameObject;
		saveButton = panel.Find ("SaveButton").gameObject;
		exitButton = panel.Find ("ExitButton").gameObject;
		scoreText = panel.Find ("ScoreText").gameObject;
		playAgainButton = panel.Find ("PlayAgainButton").gameObject;
		#if UNITY_WEBPLAYER || UNITY_WEBGL
		saveButton.SetActive (false);
		#endif
	}

	void Start () 
	{
		uic = UIController.instance;
	}

	public void HandleSave ()
	{
		uic.PlayButtonPressAudio ();
		uic.ShowSaveGamePanel ();
	}

	public void HandleNextLevel ()
	{
		uic.PlayButtonPressAudio ();
		GameController.instance.NextLevel ();
	}

	public void HandleStats ()
	{
		uic.PlayButtonPressAudio ();
		uic.ShowStatsPanel (GameController.instance.sc.GetStats());
	}

	public void HandleExit ()
	{
		uic.PlayButtonPressAudio ();
		GameController.instance.EndGame ();
	}

	public void HandlePlayAgain ()
	{
		uic.PlayButtonPressAudio ();
		Time.timeScale = 1;
		Application.LoadLevel (GameController.instance.CurrentLevel);
	}

	public void SetDisplay (Level level)
	{
		if (level == Level.Intro)
			ModifyForIntroLevels ();
		else if (level == Level.Advanced)
			ModifyForAdvancedLevels ();
		else if (level == Level.Custom)
			ModifyForCustomLevel ();
		else
			ModifyForLastLevel ();
	}

	void ModifyForIntroLevels ()
	{
		survivedText.SetActive (false);
		introLevelSurvivedText.SetActive (true);
		saveButton.SetActive (false);
		nextLevelButton.SetActive (true);
		masteredText.SetActive (false);
		scoreText.SetActive (false);
		exitButton.SetActive (true);
		playAgainButton.SetActive (false);
	}

	void ModifyForAdvancedLevels ()
	{
		survivedText.SetActive (true);
		introLevelSurvivedText.SetActive (false);
		saveButton.SetActive (true);
		nextLevelButton.SetActive (true);
		masteredText.SetActive (false);
		scoreText.SetActive (false);
		exitButton.SetActive (true);
		playAgainButton.SetActive (false);
	}

	void ModifyForLastLevel ()
	{
		survivedText.SetActive (true);
		introLevelSurvivedText.SetActive (false);
		saveButton.SetActive (false);
		nextLevelButton.SetActive (false);
		exitButton.SetActive (true);
		masteredText.SetActive (true);
		scoreText.SetActive (true);
		scoreText.GetComponent<Text>().text = scoreString + GameController.instance.TotalScore;
		playAgainButton.SetActive (false);
	}

	void ModifyForCustomLevel ()
	{
		survivedText.SetActive (false);
		introLevelSurvivedText.SetActive (true);
		saveButton.SetActive (false);
		nextLevelButton.SetActive (false);
		masteredText.SetActive (false);
		scoreText.SetActive (false);
		exitButton.SetActive (true);
		playAgainButton.SetActive (true);
	}

}
