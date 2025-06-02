using UnityEngine;
using System.Collections;

public class MultiplayerReproductionButtonsController : MonoBehaviour {
	
	public void HandleSelf ()
	{
		MultiplayerLevelController.instance.CurrentPlayer.GetComponent<PlayerReproduction> ().Self ();
	}
	
	public void HandleCross ()
	{
		MultiplayerLevelController.instance.CurrentPlayer.GetComponent<PlayerReproduction> ().Cross ();
	}

}
