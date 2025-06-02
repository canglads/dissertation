using UnityEngine;
using System.Collections;

public class MultiplayerPathogenReproduction : OrganismReproduction {
	
	public string prefabName;

	protected override void Reproduce ()
	{
		int numberOfOffspring = Random.Range (minOffspring, maxOffspring);
		for (int i = 0; i < numberOfOffspring; i++) {
			GameObject newOrganism = PhotonNetwork.Instantiate (prefabName, 
			                                      transform.position, GetRandomRotation(), 0) as GameObject;
			MasterClientSceneController.instance.Pathogens[GetComponent<Pathogen>().type].Add (newOrganism);
		}
	}
	
}
