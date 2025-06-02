using UnityEngine;
using System.Collections;

public class NetworkMaleReproduction : MaleReproduction {

	protected override void OnTriggerEnter2D (Collider2D other) {
		if (mature && other.tag.Equals("SelfPlayerFish")) {
			other.gameObject.GetComponent<HermReproduction>().HandleNearMale(gameObject);
			HandleNearHerm (other.gameObject);
			newlyMature = false;
		}
	}
	
	protected override void OnTriggerStay2D (Collider2D other) {
		if (newlyMature && other.tag.Equals("SelfPlayerFish")) {
			other.gameObject.GetComponent<HermReproduction>().HandleNearMale(gameObject);
			HandleNearHerm (other.gameObject);
			newlyMature = false;
		}
	}
	
	protected override void OnTriggerExit2D (Collider2D other) {
		if (mature && other.tag.Equals("SelfPlayerFish")) {
			other.gameObject.GetComponent<HermReproduction>().HandleNotNearMale(gameObject);
			HandleNotNearHerm (other.gameObject);
		}
	}

}
