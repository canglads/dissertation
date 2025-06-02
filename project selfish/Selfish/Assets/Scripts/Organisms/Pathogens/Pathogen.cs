using UnityEngine;
using System.Collections;

public enum PathogenType {
	Nematode,
	Bacillus,
	Acanthocephalan,
	Virus,
	Trichodinid
};

public class Pathogen : MonoBehaviour {

	public PathogenType type;

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "SelfPlayerFish" || other.tag == "NpcHerm" || other.tag == "Male") {
			if(other.GetComponent<FishHealth>().MakeInfected(type)) {
				// if successfully infected destroy the pathogen
				(GameController.instance.sc as AiSceneController).Pathogens[type].Remove(gameObject);
				Destroy (gameObject);
			}
		}
	}

	protected virtual void Awake(){
		AiPathogensMovement movement = gameObject.AddComponent<AiPathogensMovement>();
	}
	
}
