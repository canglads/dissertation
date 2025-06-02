using UnityEngine;
using System.Collections;

public class FoodOrganismReproduction : OrganismReproduction
{

	protected override void Reproduce ()
	{
		int numberOfOffspring = Random.Range (minOffspring, maxOffspring);
		AiSceneController sc = (GameController.instance.sc as AiSceneController);
		for (int i = 0; i < numberOfOffspring; i++) {
			GameObject newOrganism = Instantiate (
				sc.foodOrganismTemplate, 
			    transform.position, GetRandomRotation()) as GameObject;
			sc.FoodOrganisms.Add (newOrganism);
		}
	}

}
