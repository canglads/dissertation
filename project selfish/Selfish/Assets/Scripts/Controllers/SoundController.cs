using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
	public static SoundController instance;
	
	public AudioSource fxSource;
	public AudioSource musicSourceA;// two of these
	public AudioSource musicSourceB;// for crossfade
	public float fadeIncrement = 0.1f;
	public AudioClip bounceAudio;
	public AudioClip eatAudio;
	public AudioClip infectedAudio;
	public AudioClip reproduceAudio;

	public float SoundFxVolume {
		get {
			return soundFxVolume;
		}
		set {
			soundFxVolume = value;
			fxSource.volume = value;
		}
	}
	
	public float MusicVolume {
		get {
			return musicVolume;
		}
		set {
			musicVolume = value;
			musicSourceA.volume = value;
			musicSourceB.volume = value;
		}
	}

	float soundFxVolume;
	float musicVolume;
	bool musicSourceAPlaying;
	
	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}
	
	
	//Used to play single sound clips.
	public void PlaySingle (AudioClip clip)
	{
		fxSource.clip = clip;
		fxSource.Play ();
	}
	
	public void ChangeMusic (AudioClip clip) {
		if (musicSourceAPlaying) {
			musicSourceB.clip = clip;
			// let's make sure, don't rely on the toggle
			if (musicSourceA.isPlaying)
				StartCoroutine (FadeOut (musicSourceA));
			StartCoroutine (FadeIn (musicSourceB));
		} else {
			musicSourceA.clip = clip;
			if (musicSourceB.isPlaying)
				StartCoroutine (FadeOut (musicSourceB));
			StartCoroutine (FadeIn (musicSourceA));
		}
		musicSourceAPlaying = !musicSourceAPlaying;
	}
	
	IEnumerator FadeOut (AudioSource source)
	{
		while (source.volume > 0) {
			source.volume -= fadeIncrement * Time.deltaTime;
			yield return null;
		}
		source.Stop ();
	}

	IEnumerator FadeIn (AudioSource source)
	{
		if (!source.isPlaying) {
			source.Play ();
		}
		while (source.volume < 1) {
			source.volume += fadeIncrement * Time.deltaTime;
			yield return null;
		}
	}
	
}

