using UnityEngine;
using System.Collections;

public class MultiplayerFoodOrganism : MultiplayerOrganism {

	protected override void Awake ()
	{
		if (!MultiplayerController.instance.CurrentGame.IsMasterClient) {
			Destroy (gameObject.GetComponent<FoodOrganismHealth>());
			Destroy (gameObject.GetComponent<MultiplayerFoodOrganismReproduction>());
		}
		base.Awake ();
	}
}
