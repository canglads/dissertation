using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardPanelController : MonoBehaviour {

	public Text[] names;
	public Text[] scores;
	public Text message;

	const string Dash = "-";

	public void HandleBack ()
	{
		StartUIController.instance.PlayButtonPressAudio ();
		gameObject.SetActive (false);
	}

	void OnEnable ()
	{
		message.text = "Loading...";
		GameController.instance.scoresWebServiceClient.Get (FillData);
	}
	
	void FillData (ScoresData scoresData)
	{
		if (scoresData == null) {
			message.text = "Unable to connect";
			return;
		}
		for (int i = 0; i < names.Length; i++) {
			if (scoresData.scores[i] == null) {
				names[i].text = Dash;
				scores[i].text = Dash;
			} else {
				names[i].text = scoresData.scores[i].name;
				scores[i].text = scoresData.scores[i].value.ToString ();
			}
		}
		message.text = "";
		// also update the lowest score in the game controller
		GameController.instance.LowestHighScore = GameController.GetLowestScore(scoresData);
	}

}
