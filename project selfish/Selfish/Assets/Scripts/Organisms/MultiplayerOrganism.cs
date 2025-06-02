using UnityEngine;
using System.Collections;

public class MultiplayerOrganism : MonoBehaviour {

	protected virtual void Awake ()
	{
		if (MultiplayerController.instance.CurrentGame.IsMasterClient) {
			AiOrganismMovement movement = gameObject.AddComponent<AiOrganismMovement>();
			NetworkMovement multiplayerMovement = GetComponent<NetworkMovement>();
			movement.speed = multiplayerMovement.speed;
			Destroy (multiplayerMovement);
		}
	}

}
