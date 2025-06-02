using UnityEngine;
using System.Collections;

public class FoodOrganism : MonoBehaviour {
	
	public int value;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "SelfPlayerFish" || 
		    	other.gameObject.tag == "NpcHerm" || 
		    	other.gameObject.tag == "Male") {
			if (other.gameObject.tag.Equals("SelfPlayerFish"))
				other.gameObject.GetComponent<PlayerGrowth>().Eat(value);
			(GameController.instance.sc as AiSceneController).FoodOrganisms.Remove(gameObject);
			Destroy (gameObject);
		}
	}

	protected virtual void Awake(){
		AiOrganismMovement movement = gameObject.AddComponent<AiOrganismMovement>();
	}

}
