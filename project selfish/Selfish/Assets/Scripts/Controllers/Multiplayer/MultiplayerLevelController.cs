using UnityEngine;
using System.Collections;

public class MultiplayerLevelController : MonoBehaviour {

	public static MultiplayerLevelController instance;

	public GameObject masterClientControllerPrefab;
	public GameObject nonMasterClientControllerPrefab;

	public GameObject CurrentPlayer { 
		get {
			return cc.CurrentPlayer;
		}
		set {
			StartCoroutine (WaitForCCInstantiation(value));
		}
	}

	IEnumerator WaitForCCInstantiation (GameObject player)
	{
		while (cc == null)
			yield return null;
		cc.CurrentPlayer = player;
	}


	MultiplayerController mc;

	SceneController cc;

	void Awake ()
	{
		instance = this;
		mc = MultiplayerController.instance;
		if (mc.CurrentGame.IsMasterClient)
			cc = Instantiate (masterClientControllerPrefab).GetComponent<MasterClientSceneController> ();
		else {
			cc = Instantiate (nonMasterClientControllerPrefab).GetComponent<NonMasterClientSceneController> ();
		}
	}

	public Stats GetStats ()
	{
		return cc.GetStats ();
	}

	// -1 loss; 0 draw; 1 win
	public int GetResult ()
	{
		return GetResult (MultiplayerScoreData.instance.Scores, mc.GetPlayerIndex());
	}

	public static int GetResult (MultiplayerScore[] scores, int playerIndex)
	{
		int playerScore = scores[playerIndex].Score;
		int equalScoreCount = 0;
		foreach (MultiplayerScore score in scores) {
			if (playerScore < score.Score)
				return -1;
			if (playerScore == score.Score)
				equalScoreCount++;
		};
		// will check player's score against itself, hence > 1 not > 0
		return equalScoreCount > 1 ? 0 : 1;
	}

}
