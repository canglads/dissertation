using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopRightPanelController : MonoBehaviour {

	public void HandlePause ()
	{
		UIController.instance.PlayButtonPressAudio ();
		(GameController.instance.sc as SinglePlayerSceneController).ShowPauseScreenAndPauseAction ();
	}

}
