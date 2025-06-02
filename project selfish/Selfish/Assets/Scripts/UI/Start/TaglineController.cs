using UnityEngine;
using System.Collections;

public class TaglineController : MonoBehaviour {
	
	public float duration = 2f;
	
	float timer;
	
	void Awake ()
	{
		timer = duration;
	}

	void Update ()
	{
		if (timer < 0f)
			ShowMainMenu ();
		timer -= Time.deltaTime;
	}
	
	void ShowMainMenu ()
	{
		StartUIController.instance.ShowMainMenu ();
		Destroy (gameObject);
	}
	
}
