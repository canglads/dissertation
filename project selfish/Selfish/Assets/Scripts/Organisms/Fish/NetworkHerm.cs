using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkHerm : NetworkFish {

	public void ConvertToSelfPlayer (List<int> playerIndexes, List<PathogenType> immunities)
	{
		// add new scripts
		PlayerMovement newMovement = gameObject.AddComponent<PlayerMovement> ();
		gameObject.AddComponent<PlayerGrowth> ();
		gameObject.AddComponent<PlayerHealth> ();
		gameObject.AddComponent<PlayerReproduction> ();
		gameObject.AddComponent<ImmuneSystem> ();
		SelfPlayer newFish = gameObject.AddComponent<SelfPlayer> ();
		// set some values
		newMovement.speed = Movement.speed;
		newFish.PlayerIndexes = playerIndexes;
		newFish.ImmuneSystem.Immunities = immunities;
		// remove old script (but not this)
		Destroy (Movement);
		// other changes
		gameObject.tag = "SelfPlayerFish";
		CameraController.instance.Target = transform;
	}

}
