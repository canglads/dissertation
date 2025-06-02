using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageImageController : MonoBehaviour {

	public float flashSpeed = 1f;
	public Color flashColor = new Color (1f, 0.05f, 0.05f, 0.5f);

	public bool Flashing { 
		get {
			return flashing;
		}
		set {
			flashing = value;
			if (flashing)
				StartFlash ();
			else // there is no difference between paused and stopped here
				paused = true;
		}
	}
	public bool Paused { 
		get {
			damageImage.color = Color.clear;
			return paused;
		}
		set {
			paused = value;	
			if (!paused && flashing)
				StartFlash();
		}
	}

	bool paused;
	bool flashing;
	Image damageImage;
	bool toggle;
	float flashTimer;

	void Awake ()
	{
		damageImage = GetComponent<Image> ();
	}

	void StartFlash ()
	{
		flashTimer = flashSpeed;
		damageImage.color = flashColor;
		StartCoroutine (Flash ());
	}

	IEnumerator Flash ()
	{
		if (paused) {
			damageImage.color = Color.clear;
			yield break;
		}
		while (flashTimer > 0) {
			flashTimer -= Time.deltaTime;
			yield return null;
		}
		damageImage.color = Color.clear;
		Invoke ("StartFlash", flashSpeed);
	}

}
