using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {

	public void HandlePress ()
	{
		UIController.instance.PlayButtonPressAudio ();
		(GameController.instance.sc as SinglePlayerSceneController).Pressed = true;
	}

}
