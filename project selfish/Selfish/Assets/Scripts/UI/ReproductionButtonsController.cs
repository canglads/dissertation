using UnityEngine;
using System.Collections;

public class ReproductionButtonsController : MonoBehaviour {

	public void HandleSelf ()
	{
		(GameController.instance.sc as AiSceneController).CurrentPlayer.GetComponent<PlayerReproduction> ().Self ();
	}

	public void HandleCross ()
	{
		(GameController.instance.sc as AiSceneController).CurrentPlayer.GetComponent<PlayerReproduction> ().Cross ();
	}

}
