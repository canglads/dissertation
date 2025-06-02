using UnityEngine;
using System.Collections;

public class MultiplayerPathogen : MultiplayerOrganism {

	protected override void Awake ()
	{
		if (!MultiplayerController.instance.CurrentGame.IsMasterClient) {
			Destroy (gameObject.GetComponent<PathogenHealth> ());
			Destroy (gameObject.GetComponent<MultiplayerPathogenReproduction>());
		}
		base.Awake ();
	}

}
