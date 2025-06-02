using UnityEngine;
using System.Collections;

public class MultiplayerFoodOrganismNutrition : MonoBehaviour {

	public int value;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "SelfPlayerFish" ||
		    other.gameObject.tag == "Male" ||
		    other.gameObject.tag == "NpcHerm" ||
		    other.gameObject.tag == "OtherPlayerFish") {
			if (other.gameObject.tag.Equals("SelfPlayerFish"))
				other.gameObject.GetComponent<PlayerGrowth>().Eat(value);
			if (PhotonNetwork.isMasterClient) {
				MasterClientSceneController.instance.FoodOrganisms.Remove(gameObject);
				PhotonNetwork.Destroy (gameObject);
			}
		}
	}
	
}

