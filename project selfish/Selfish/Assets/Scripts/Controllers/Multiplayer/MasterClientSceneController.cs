using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterClientSceneController : AiSceneController {

	public static MasterClientSceneController instance;

	public GameObject scoresPrefab;

	MultiplayerController mc;
	MultiplayerScoreData scoreData;

	public override void DestroyOrganism (GameObject organism)
	{
		PhotonNetwork.Destroy (organism);
	}

	public override void PropagateMakeMaleMature (GameObject male)
	{
		RpcController.instance.PropagateMakeMaleMature (male.GetComponent<PhotonView>().viewID);
	}

	protected override void Awake ()
	{
		instance = this;
		base.Awake ();
		IsMultiplayer = true;
		mc = MultiplayerController.instance;
	}

	protected override void Start ()
	{
		base.Start ();
		scoreData = PhotonNetwork.Instantiate (scoresPrefab.name,
		                                       Vector3.zero,
		                                       Quaternion.identity,
		                                       0)//group 0
			.GetComponent<MultiplayerScoreData> ();
		MultiplayerScoreData.instance.Init (mc.CurrentGame.Players);
	}

	protected override int GetCountOfFishWithPlayerIndex ()
	{
		int playerIndex = MultiplayerController.instance.GetPlayerIndex ();
		return GetCountOfFishWithPlayerIndex (Herms, playerIndex) +
			GetCountOfFishWithPlayerIndex (Males, playerIndex);
	}

	public void HandleNewNetworkPlayer (SelfPlayer fish)
	{
		fish.ConvertToNetworkFish ();
		NetworkFish newFish = fish.GetComponent<NetworkFish>();
		Destroy (fish);
		newFish.PlayerIndexes.Add (mc.GetPlayerIndexFromId(newFish.GetComponent<PhotonView>().ownerId));
		StartCoroutine (WaitForDestroyNonMasterSelfPlayerFish(fish, newFish));
	}

	IEnumerator WaitForDestroyNonMasterSelfPlayerFish(SelfPlayer oldFish, NetworkFish newFish)
	{
		// if don't wait for destruction of old fish script ...
		while (oldFish != null)
			yield return null;
		// ... master may try to network destroy the gameobject in KillHerms ...
		Herms.Add (newFish.gameObject);
		// ... and RecalculateScores will not be correct
		RecalculateScores ();
	}

	public GameObject GetNewFishForOtherPlayer (int deadFishId, int playerIndex, PhotonPlayer otherPlayer)
	{
		//first remove the current other player fish from herms before it gets destroyed
		//by the non-master
		Herms.Remove(PhotonView.Find (deadFishId).gameObject);
		RecalculateScores ();
		// now get try to get a fish to switch to
		GameObject switchFish = GetSwitchFish (playerIndex);
		if (switchFish == null)
			return null;
		AiHerm oldFish = switchFish.GetComponent<AiHerm> ();
		oldFish.ConvertToOtherPlayerFish ();
		Destroy (oldFish);
		switchFish.GetComponent<PhotonView> ().TransferOwnership (otherPlayer);
		return switchFish;
	}

	public override void RecalculateScores ()
	{
		if (gameOver)
			return;
		//clean up Herms
		Herms.RemoveAll (item => item == null);
		// construct scores here first then copy over at once to ensure consistent syncing
		MultiplayerScore[] newScores = new MultiplayerScore[scoreData.Scores.Length];
		for (int i = 0; i < newScores.Length; i++)
			newScores [i] = new MultiplayerScore (scoreData.Scores [i].PlayerId);
		//increment scores to fish count 
		Herms.ForEach(herm => {
			if (herm.GetComponent<Fish>().PlayerIndexes != null) {
				herm.GetComponent<Fish>().PlayerIndexes.ForEach (index => 
					newScores[index].Score++
				);  
			}
		});
		//divide by total fish count
		foreach (MultiplayerScore s in newScores)
			s.Score = Mathf.RoundToInt(s.Score * 100f / (Herms.Count + Males.Count));
		// now we can copy over to the synced data object
		scoreData.Scores = newScores;
		scoreData.TotalFish = Herms.Count + Males.Count;
		// and set values for the master
		Score = scoreData.Scores[mc.GetPlayerIndex()].Score;
		uiController.DisplayScore (Score);
	}

	protected override void CreateOffspring (List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position)
	{
		float maleRatio = self ? param.hermReproductionParameters.maleRatioSelf : param.hermReproductionParameters.maleRatioCross;
		bool male = Random.value < maleRatio;
		GameObject template = male ? maleTemplate : hermTemplate;
		GameObject newNpc = PhotonNetwork.Instantiate (template.name, position, GetRandomRotation (), 0);
		if (male)
			RpcController.instance.PropagateInitMale (newNpc.GetComponent<PhotonView> ().viewID,
			                                 playerIndexes,
			                                 immunities);
		InitNpcFish (newNpc, male, playerIndexes, immunities);
		RecalculateScores ();
	}

	protected override void SpawnHerm (bool startOfLevel)
	{
		GameObject newHerm = PhotonNetwork.Instantiate (hermTemplate.name, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation(), 0);
		Herms.Add (newHerm);
		newHerm.GetComponent<AiHerm>().Init (null, GetRandomImmmunities());
	}
	
	protected override void SpawnMale (bool startOfLevel)
	{
		GameObject newMale = PhotonNetwork.Instantiate (maleTemplate.name, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation(), 0);
		Males.Add (newMale);
		newMale.GetComponent<AiMale>().Init(new List<int>(), GetRandomImmmunities());
		RpcController.instance.PropagateInitMale (
			newMale.GetComponent<PhotonView> ().viewID,
		    newMale.GetComponent<Fish>().PlayerIndexes,
			newMale.GetComponent<ImmuneSystem>().Immunities);
	}
	
	protected override void SpawnPathogen (GameObject template, bool startOfLevel)
	{
		GameObject newPathogen = PhotonNetwork.Instantiate (template.name, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation(), 0);
		Pathogens[template.GetComponent<Pathogen>().type].Add(newPathogen);
	}
	
	protected override void SpawnFoodOrganism (bool startOfLevel)
	{
		GameObject newFoodOrganism = PhotonNetwork.Instantiate (foodOrganismTemplate.name, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation (), 0);
		FoodOrganisms.Add (newFoodOrganism);
	}

	protected override void SpawnPredator (bool startOfLevel)
	{
		GameObject newPredator = PhotonNetwork.Instantiate (predatorTemplate.name, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation(), 0);
		Predators.Add (newPredator);
	}

	public override void HandleCurrentPlayerFishDead ()
	{
		PhotonNetwork.Destroy (CurrentPlayer);
		SoundController.instance.PlaySingle (switchAudio);
		GameObject switchFish = GetSwitchFish (mc.GetPlayerIndex());
		if (switchFish == null)//i.e. no relations to switch to
			CreateSelfPlayer (false);
		else {//switch
			CurrentPlayer = switchFish;
			AiHerm fish = switchFish.GetComponent<AiHerm>();
			fish.ConvertToSelfPlayer();
			Destroy(fish);
		}
		uiController.HandleSwitchCurrentPlayer ();
		if (param.displayImmPanels)
			uiController.ShowOwnImmPanel ();
		RecalculateScores ();
	}

	protected override void CreateSelfPlayer (bool startOfLevel)
	{
		CurrentPlayer = PhotonNetwork.Instantiate (playerTemplate.name, StartPositionsController.instance.GetRandomStartPosition (startOfLevel), Quaternion.identity, 0);
		// note, random immunities added in base call below
		CurrentPlayer.GetComponent<NonNetworkFish> ().Init (new List<int>(){mc.GetPlayerIndex()}, new List<PathogenType>());
		base.CreateSelfPlayer (startOfLevel);
	}

	protected override void HandleSurvived ()
	{
		Time.timeScale = 0;
		Camera.main.GetComponent<CameraController> ().enabled = false;
		gameOver = true;
		MultiplayerScoreData.instance.SendFinalScores ();
		MultiplayerUIController.instance.ShowGameOverPanel ();
	}

	protected override void KillHerms (int number)
	{
		for (int i = 0; i < number;) {
			GameObject herm = Herms[Random.Range(0, Herms.Count)];
			if (herm.GetComponent<SelfPlayer>() == null && herm.GetComponent<PhotonView>().isMine) {
				herm.GetComponent<AiHealth>().HandleDeath();
				i++;
			}
		}
	}

	protected override void KillOrganism (List<GameObject> organisms)
	{
		int index = Random.Range (0, organisms.Count);
		GameObject organism = organisms[index];
		organisms.RemoveAt(index);
		PhotonNetwork.Destroy (organism);
	}

	public override Stats GetStats ()
	{
		// definitive scores must be sourced directly from scoreData
		// to avoid race conditions
		int score = scoreData.Scores[mc.GetPlayerIndex()].Score;
		return new Stats (
			(Mathf.RoundToInt(score * scoreData.TotalFish / 100f)).ToString (),
		    scoreData.TotalFish.ToString (),
		    score.ToString (),
			((int)timer) + " seconds",
		    Format (Selfs, Crosses),
		    Format (Crosses, Selfs)
		);
	}

	void InitNpcFish (GameObject npcFish, bool male, List<int> playerIndexes, List<PathogenType> immunities)
	{
		npcFish.transform.localScale = new Vector3 (0.7f, 0.7f, 1f);
		npcFish.GetComponent<AiFish>().Init (playerIndexes, immunities);
		if (male)
			Males.Add (npcFish);
		else
			Herms.Add (npcFish);
	}

	int GetCountOfFishWithPlayerIndex (List<GameObject> fishList, int playerIndex)
	{
		int count = 0;
		fishList.ForEach (fish => {
			fish.GetComponent<Fish>().PlayerIndexes.ForEach(index => {
				if (index == playerIndex)
					count++;
			});
		});
		return count;
	}

}
