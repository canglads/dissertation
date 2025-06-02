using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// only used in multiplayer level (exit in single player is via pause panel)
public class ConfirmExitPanelController : MonoBehaviour {

	public Text message;

	void OnEnable ()
	{
		message.gameObject.SetActive (PhotonNetwork.isMasterClient);
	}

	public void HandleYes ()
	{
		UIController.instance.PlayButtonPressAudio ();
		MultiplayerController.instance.HandleExitDuringGame ();
	}

	public void HandleNo ()
	{
		UIController.instance.PlayButtonPressAudio ();
		gameObject.SetActive (false);
	}

}
