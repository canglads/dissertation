using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiMale : AiFish {

	public override void RemoveFromSceneController ()
	{
		AiSceneController sc = (AiSceneController)GameController.instance.sc;
		sc.RemoveMale (gameObject);
	}

	protected override void Awake ()
	{
		base.Awake ();
		Movement = gameObject.AddComponent<AiOrganismMovement> ();
		Reproduction = gameObject.GetComponent<AiMaleReproduction> (); 
		if (GameController.instance.sc.IsMultiplayer && !PhotonNetwork.isMasterClient) {
			ConvertToNetworkFish ();
			Destroy (this);
		}
	}

	public override void ConvertToNetworkFish ()
	{
		// add new scripts
		NetworkMovement newMovement = gameObject.AddComponent<NetworkMovement> ();
		gameObject.AddComponent<NetworkMaleReproduction>();
		gameObject.AddComponent<NetworkMale> ();
		// set values
		newMovement.speed = Movement.speed;
		// remove old scripts (but not this)
		Destroy (Movement);
		Destroy (Growth);
		Destroy (Health);
		Destroy (Reproduction);
	}

}
