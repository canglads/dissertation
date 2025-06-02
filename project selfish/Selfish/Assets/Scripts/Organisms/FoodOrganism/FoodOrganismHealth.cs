using UnityEngine;
using System.Collections;

public class FoodOrganismHealth : OrganismHealth {

	protected override void Update () {
		if (timeToDeath < 0f) {
			AiSceneController sc = GameController.instance.sc as AiSceneController;
			sc.FoodOrganisms.Remove (gameObject);
			sc.DestroyOrganism (gameObject);
		}
		timeToDeath -= Time.deltaTime;
	}

}
