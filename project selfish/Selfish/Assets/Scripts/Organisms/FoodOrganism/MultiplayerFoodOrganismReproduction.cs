using UnityEngine;
using System.Collections;

public class MultiplayerFoodOrganismReproduction : OrganismReproduction {

	public string prefabName = "MultiplayerCopepod";
	
	protected override void Reproduce ()
	{
		int numberOfOffspring = Random.Range (minOffspring, maxOffspring);
		for (int i = 0; i < numberOfOffspring; i++) {
			GameObject newOrganism = PhotonNetwork.Instantiate (prefabName, 
			                                                    transform.position, GetRandomRotation(), 0) as GameObject;
			MasterClientSceneController.instance.FoodOrganisms.Add (newOrganism);
		}
	}

}
