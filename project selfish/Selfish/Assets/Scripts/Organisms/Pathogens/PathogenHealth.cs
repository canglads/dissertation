using UnityEngine;
using System.Collections;

public class PathogenHealth : OrganismHealth {
		
	protected override void Update () {
		if (timeToDeath < 0f) {
			AiSceneController sc = (AiSceneController)GameController.instance.sc;
			sc.Pathogens[GetComponent<Pathogen>().type].Remove (gameObject);
			sc.DestroyOrganism (gameObject);
		}
		timeToDeath -= Time.deltaTime;
	}

}
