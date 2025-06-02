using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsPanelController : MonoBehaviour {

	public Text yourFishText;
	public Text totalFishText;
	public Text scoreText;
	public Text timeLeftText;
	public Text selfsText;
	public Text crossesText;

	public void SetStats (Stats stats)
	{
		yourFishText.text = stats.YourFish;
		totalFishText.text = stats.TotalFish;
		scoreText.text = stats.Score;
		timeLeftText.text = stats.TimeLeft;
		selfsText.text = stats.Selfs;
		crossesText.text = stats.Crosses;
	}

	public Stats GetStats(){
		return new Stats (yourFishText.text, totalFishText.text, scoreText.text, timeLeftText.text, selfsText.text, crossesText.text);
	}

	public void HandleBack ()
	{
		UIController.instance.PlayButtonPressAudio ();
		gameObject.SetActive (false);
	}

}
