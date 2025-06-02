using UnityEngine;
using System.Collections;

public class PredatorHealth : OrganismHealth {
	
	protected override void Update () {
		if (timeToDeath < 0f) {
			AiSceneController sc = GameController.instance.sc as AiSceneController;
			sc.Predators.Remove (gameObject);
			sc.DestroyOrganism (gameObject);
		}
		timeToDeath -= Time.deltaTime;
	}
	
}
