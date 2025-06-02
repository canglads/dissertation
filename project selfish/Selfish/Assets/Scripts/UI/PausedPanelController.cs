using UnityEngine;
using System.Collections;

public class PausedPanelController : MonoBehaviour {

	public void HandlePlay ()
	{
		UIController.instance.PlayButtonPressAudio ();
		(GameController.instance.sc as SinglePlayerSceneController).HidePauseScreenAndUnpauseAction ();
	}

	public void HandleRestart ()
	{
		UIController.instance.PlayButtonPressAudio ();
		Time.timeScale = 1f;
		Application.LoadLevel (GameController.instance.CurrentLevel);
	}

	public void HandleExit ()
	{
		UIController.instance.PlayButtonPressAudio ();
		GameController.instance.EndGame ();
	}

}
