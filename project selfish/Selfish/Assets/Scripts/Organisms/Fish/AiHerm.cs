using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiHerm : AiFish {

	public override void RemoveFromSceneController ()
	{
		AiSceneController sc = (AiSceneController)GameController.instance.sc;
		sc.RemoveHerm (gameObject);
	}

	public void ConvertToSelfPlayer ()
	{
		// add new scripts
		PlayerMovement newMovement = gameObject.AddComponent<PlayerMovement> ();
		PlayerGrowth newGrowth = gameObject.AddComponent<PlayerGrowth> ();
		PlayerHealth newHealth = gameObject.AddComponent<PlayerHealth> ();
		PlayerReproduction newRepro = gameObject.AddComponent<PlayerReproduction> ();
		SelfPlayer newFish = gameObject.AddComponent<SelfPlayer> ();// this adds another ImmuneSystem
		// copy over values
		//newMovement.speed = Movement.speed; //not working
		newFish.PlayerIndexes = PlayerIndexes;
		newGrowth.FoodCounter = Growth.FoodCounter;
		newHealth.timeToDeath = Health.timeToDeath;
		newRepro.mature = Reproduction.mature;
		newFish.ImmuneSystem.Immunities = ImmuneSystem.Immunities;
		// remove old scripts (except this)
		Destroy (Growth);
		Destroy (Health);
		Destroy (Reproduction);
		Destroy (Movement);
		// other changes
		gameObject.tag = "SelfPlayerFish";
		CameraController.instance.Target = transform;
		GameController.instance.sc.CurrentPlayer = gameObject;
		if (newRepro.mature)
			newRepro.ShowMatureUIComponents ();
	}

	public override void ConvertToNetworkFish ()
	{
		// add new scripts
		NetworkMovement newMovement = gameObject.AddComponent<NetworkMovement> ();
		NetworkHerm newFish = gameObject.AddComponent<NetworkHerm> ();
		// copy over values
		newMovement.speed = Movement.speed;
		// not needed initially but will be required for convert to other player
		newFish.PlayerIndexes = PlayerIndexes;
		// remove old scripts (except this)
		Destroy (Growth);
		Destroy (Health);
		Destroy (Reproduction);
		Destroy (Movement);
		Destroy (ImmuneSystem);
	}

	public void ConvertToOtherPlayerFish ()
	{
		ConvertToNetworkFish ();
		gameObject.tag = "OtherPlayerFish";
	}

	protected override void Awake ()
	{
		base.Awake ();
		Movement = gameObject.AddComponent<AiOrganismMovement> ();
		Reproduction = gameObject.GetComponent<AiHermReproduction> (); 
		if (GameController.instance.sc.IsMultiplayer && !PhotonNetwork.isMasterClient) {
			ConvertToNetworkFish ();
			Destroy (this);
		}
	}

}
