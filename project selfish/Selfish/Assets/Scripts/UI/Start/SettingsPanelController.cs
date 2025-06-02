using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour {
		
	public Slider soundFxSlider;
	public Slider musicSlider;
		
	void Start ()
	{
		soundFxSlider.value = GameController.instance.SoundFxVolume;
		musicSlider.value = GameController.instance.MusicVolume;
	}
		
	public void HandleSoundFxChanged ()
	{
		GameController.instance.SoundFxVolume = soundFxSlider.value;
	}
		
	public void HandleMusicChanged ()
	{
		GameController.instance.MusicVolume = musicSlider.value;
	}
		
	public void HandleCredits ()
	{
		StartUIController.instance.ShowCredits ();	
		gameObject.SetActive (false);
	}

	public void HandleBack ()
	{
		StartUIController.instance.PlayButtonPressAudio ();
		gameObject.SetActive (false);
	}
		
}


