using UnityEngine;
using System.Collections;

public class AiMaleReproduction : MaleReproduction {

	public override void MakeMature () {
		mature = true;
		newlyMature = true;
		GameController.instance.sc.PropagateMakeMaleMature (gameObject);
	}

	protected override void OnTriggerEnter2D (Collider2D other) {
		if (mature && (other.tag.Equals("NpcHerm") || other.tag.Equals("SelfPlayerFish"))) {
			other.gameObject.GetComponent<HermReproduction>().HandleNearMale(gameObject);
			HandleNearHerm (other.gameObject);
			newlyMature = false;
		}
	}
	
	protected override void OnTriggerStay2D (Collider2D other) {
		if (newlyMature && (other.tag.Equals("NpcHerm") || other.tag.Equals("SelfPlayerFish"))) {
			other.gameObject.GetComponent<HermReproduction>().HandleNearMale(gameObject);
			HandleNearHerm (other.gameObject);
			newlyMature = false;
		}
	}
	
	protected override void OnTriggerExit2D (Collider2D other) {
		if (mature && (other.tag.Equals("NpcHerm")) || other.tag.Equals("SelfPlayerFish")) {
			other.gameObject.GetComponent<HermReproduction>().HandleNotNearMale(gameObject);
			HandleNotNearHerm (other.gameObject);
		}
	}

}