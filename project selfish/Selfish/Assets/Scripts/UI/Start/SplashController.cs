using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {

	public float duration = 2f;

	float timer;

	void Awake ()
	{
		timer = duration;
	}

	void Start ()
	{
		if (GameController.instance.FirstLoad) {
			GameController.instance.FirstLoad = false;
		} else {
			// only show splash & tagline on first load
			StartUIController.instance.ShowMainMenu();
		}
	}

	void Update ()
	{
		if (timer < 0f)
			ShowTagline ();
		timer -= Time.deltaTime;
	}

	void ShowTagline ()
	{
		StartUIController.instance.tagline.SetActive (true);
		Destroy (gameObject);
	}

}
