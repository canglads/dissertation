using UnityEngine;
using System.Collections;

public class SelfPlayer : NonNetworkFish {

	public override void RemoveFromSceneController ()
	{
		GameController.instance.sc.RemoveHerm (gameObject);
	}

	public override void ConvertToNetworkFish ()
	{
		// add new scripts
		NetworkMovement newMovement = gameObject.AddComponent<NetworkMovement> ();
		gameObject.AddComponent<NetworkHerm> ();
		// set values
		newMovement.speed = Movement.speed;
		// remove old scripts (but not this)
		Destroy (Movement);
		Destroy (Growth);
		Destroy (Health);
		Destroy (Reproduction);
		Destroy (ImmuneSystem);
		// other changes
		gameObject.tag = "OtherPlayerFish";
	}

	protected override void Awake ()
	{
		base.Awake ();
		Movement = GetComponent<PlayerMovement> ();
		Growth = GetComponent<PlayerGrowth> ();
		Health = GetComponent<PlayerHealth> ();
		Reproduction = GetComponent<PlayerReproduction> ();
		if (GameController.instance.sc.IsMultiplayer) {
			if (!GetComponent<PhotonView> ().isMine) {
				if (PhotonNetwork.isMasterClient)
					MasterClientSceneController.instance.HandleNewNetworkPlayer (this);
				else {
					ConvertToNetworkFish ();
					Destroy (this);
				}
			}
		}
	}

}
