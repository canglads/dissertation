using UnityEngine;
using System.Collections;

public class Predator : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "SelfPlayerFish" || 
			other.gameObject.tag == "NpcHerm" || 
			other.gameObject.tag == "Male")
			other.GetComponent<FishHealth> ().GetEaten ();
	}

}
