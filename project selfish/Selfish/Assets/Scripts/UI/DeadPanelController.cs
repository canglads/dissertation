using UnityEngine;
using System.Collections;

public class DeadPanelController : MonoBehaviour {

	public void HandleRestart ()
	{
		UIController.instance.PlayButtonPressAudio ();
		Time.timeScale = 1f;
		Application.LoadLevel (GameController.instance.CurrentLevel);
	}

	public void HandleStats ()
	{
		UIController.instance.PlayButtonPressAudio ();
		UIController.instance.ShowStatsPanel (GameController.instance.sc.GetStats());
	}

	public void HandleExit ()
	{
		UIController.instance.PlayButtonPressAudio ();
		GameController.instance.EndGame ();
	}

}
