using UnityEngine;
using System.Collections;

public class PathogenReproduction : OrganismReproduction {

	public GameObject template;

	protected override void Reproduce ()
	{
		int numberOfOffspring = Random.Range (minOffspring, maxOffspring);
		for (int i = 0; i < numberOfOffspring; i++) {
			GameObject newOrganism = Instantiate (template, 
			                                      transform.position, GetRandomRotation()) as GameObject;
			(GameController.instance.sc as AiSceneController).Pathogens[GetComponent<Pathogen>().type].Add (newOrganism);
		}
	}

}
