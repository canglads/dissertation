using UnityEngine;
using System.Collections;

public class MultiplayerPathogenInfection : Pathogen {

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (PhotonNetwork.isMasterClient) {
			if (other.tag == "SelfPlayerFish" || other.tag == "NpcHerm" || other.tag == "Male") {
				if (other.GetComponent<FishHealth> ().MakeInfected (type)) {
					// if successfully infected destroy the pathogen
					MasterClientSceneController.instance.Pathogens[type].Remove (gameObject);
					PhotonNetwork.Destroy (gameObject);
				}
			} else if (other.tag == "OtherPlayerFish") {
				// we don't know if will be infected or not - it is up to the owner to deal
				// with the infection of the fish - but here we destroy the pathogen in either case
				MasterClientSceneController.instance.Pathogens[type].Remove (gameObject);
				PhotonNetwork.Destroy (gameObject);
			}
		} else {
			if (other.tag == "SelfPlayerFish")
				other.GetComponent<FishHealth> ().MakeInfected (type);
		}
	}

}
