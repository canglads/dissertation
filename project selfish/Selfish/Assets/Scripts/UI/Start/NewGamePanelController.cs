using UnityEngine;
using System.Collections;

public class NewGamePanelController : MonoBehaviour {

	public void HandleIntro ()
	{
		StartUIController.instance.PlayButtonPressAudio ();
		GameController.instance.LoadFirstIntroLevel ();
	}

	public void HandleAdvanced ()
	{
		StartUIController.instance.PlayButtonPressAudio ();
		GameController.instance.LoadFirstAdvancedLevel ();
	}

	public void HandleMultiplayer ()
	{
		StartUIController.instance.PlayButtonPressAudio ();
		StartUIController.instance.ShowMatchmakingPanel ();
	}

	public void HandleCustom ()
	{
		StartUIController.instance.PlayButtonPressAudio ();
		GameController.instance.LoadCustomLevel ();
	}

	public void HandleBack ()
	{
		gameObject.SetActive (false);
	}

}
