using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	StartUIController suic;

	void Start ()
	{
		Time.timeScale = 1;//in case returning here from previously paused state
		suic = StartUIController.instance;
		#if UNITY_WEBPLAYER || UNITY_WEBGL
		GameObject loadButton = transform.Find ("Panel").Find ("LoadButton").gameObject;
		loadButton.SetActive(false);
		RectTransform newGameButton = transform.Find ("Panel").Find ("NewGameButton").GetComponent<RectTransform>();
		newGameButton.anchorMin = new Vector2 (0.25f, 0.6f);
		newGameButton.anchorMax = new Vector2 (0.75f, 0.75f);
		#endif
		#if UNITY_WEBPLAYER || UNITY_WEBGL
		GameObject exitButton = transform.Find ("Panel").Find("ExitButton").gameObject;
		exitButton.SetActive(false);
		#endif
	}

	public void HandleNewGame ()
	{
		suic.PlayButtonPressAudio ();
		suic.ShowNewGamePanel ();
	}

	public void HandleLoad ()
	{
		suic.PlayButtonPressAudio ();
		suic.ShowLoadPanel ();
	}

	public void HandleLeaderboard ()
	{
		suic.PlayButtonPressAudio ();
		suic.leaderboardPanel.SetActive (true);
	}

	public void HandleSettings ()
	{
		suic.PlayButtonPressAudio ();
		suic.settingsPanel.SetActive (true);
	}

	public void HandleExit ()
	{
		Application.Quit ();
	}

}
