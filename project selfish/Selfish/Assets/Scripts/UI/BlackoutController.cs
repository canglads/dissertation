using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//actually a whiteout
public class BlackoutController : MonoBehaviour {

	Image blackout;
	float fadeSpeed = 10f;
	// if we have started fading, ignore new requests
	// otherwise, the multiple coroutines will conflict
	// and never complete
	bool fading;

	void Awake ()
	{
		blackout = GetComponent<Image> ();
	}

	public void Blackout ()
	{
		if (fading)
			return;
		fading = true;
		StartCoroutine (FadeOut());
	}

	IEnumerator FadeOut ()
	{
		while (blackout.color.a < 0.95f) {
			blackout.color = new Color(1f, 1f, 1f, Mathf.Lerp(blackout.color.a, 1f, Time.deltaTime * fadeSpeed));
			yield return null;
		}
		StartCoroutine (FadeIn());
	}
	
	IEnumerator FadeIn ()
	{
		while (blackout.color.a > 0.05f) {
			blackout.color = new Color(1f, 1f, 1f, Mathf.Lerp(blackout.color.a, 0f, Time.deltaTime * fadeSpeed));
			yield return null;
		}
		blackout.color = Color.clear;
		fading = false;
	}

}
