using UnityEngine;
using System.Collections;

public class StartUIController : MonoBehaviour {

	public static StartUIController instance;

	public GameObject tagline;
	public GameObject settingsPanel;
	public GameObject mainMenu;
	public GameObject leaderboardPanel;
	public GameObject newGamePanel;
	public GameObject loadPanel;
	public GameObject matchmakingPanel;
	public GameObject creditsPanel;
	public AudioClip buttonPressAudio;

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	public void PlayButtonPressAudio ()
	{
		SoundController.instance.PlaySingle (buttonPressAudio);
	}

	public void ShowMainMenu ()
	{
		mainMenu.SetActive (true);
	}

	public void ShowNewGamePanel ()
	{
		newGamePanel.SetActive (true);
	}

	public void ShowLoadPanel ()
	{
		loadPanel.SetActive (true);
	}

	public void ShowMatchmakingPanel ()
	{
		matchmakingPanel.SetActive (true);
	}

	public void ShowCredits ()
	{
		creditsPanel.SetActive (true);
	}

}
