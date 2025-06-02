using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NonMasterClientSceneController : SceneController {

	MultiplayerController mc;

	public override void PropagateMakeMaleMature (GameObject male)
	{
		// no action - should not be called on non-master in any case
	}

	protected override void Awake ()
	{
		base.Awake ();
		IsMultiplayer = true;
	}

	protected override void Start ()
	{
		base.Start ();
		mc = MultiplayerController.instance;
		CreateNewPlayerFish ();
		uiController.UpdateFood (param.foodToMature);
		uiController.ShowOwnImmPanel ();
		StartCoroutine(WaitForScoresThenSetNames());
	}

	IEnumerator WaitForScoresThenSetNames ()
	{
		while (MultiplayerScoreData.instance == null)
			yield return null;
		MultiplayerScoreData.instance.Init (mc.CurrentGame.Players);
	}

	void CreateNewPlayerFish ()
	{
		CurrentPlayer = PhotonNetwork.Instantiate (playerTemplate.name, startPos.GetRandomStartPosition (false), Quaternion.identity, 0);
		CurrentPlayer.GetComponent<NonNetworkFish>().Init(new List<int>(){mc.GetPlayerIndex ()}, GetRandomImmmunities());
		uiController.UpdateFood (CurrentPlayer.GetComponent<SelfPlayer> ().Growth.FoodCounter);
		CameraController.instance.Target = CurrentPlayer.transform;
	}

	public override void CreateOffspring (int numberOfOffspring, List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position)
	{
		RpcController.instance.AskMasterToCreateOffspring (numberOfOffspring, playerIndexes, immunities, self, position);
	}
	
	public override void HandleCurrentPlayerFishDead ()
	{
		RpcController.instance.InformMasterOfDeath (CurrentPlayer.GetPhotonView().viewID, mc.GetPlayerIndex());
	}

	public void SwitchToNewFish (int id, List<int> playerIndexes, List<PathogenType> immunities,
	                             bool mature, int foodCounter, float timeToDeath)
	{
		SoundController.instance.PlaySingle (switchAudio);
		GameObject oldFish = CurrentPlayer;
		if (id == -1) //no fish to switch to so make a new one
			CreateNewPlayerFish ();
		else
			FindFishAndConvertToPlayer (id, playerIndexes, immunities, mature, foodCounter, timeToDeath);
		uiController.HandleSwitchCurrentPlayer ();
		uiController.ShowOwnImmPanel ();
		PhotonNetwork.Destroy (oldFish);
	}

	public void InitMale (int id, List<int> playerIndexes, List<PathogenType> immunities)
	{
		GameObject fish = PhotonView.Find (id).gameObject;
		if (fish == null) {
			Debug.Log("Unable to init male - id not found: " + id);
			return;
		}
		fish.GetComponent<Fish> ().PlayerIndexes = playerIndexes;
		fish.GetComponent<ImmuneSystem> ().Immunities = immunities;
	}

	public void MakeMaleMature (int id)
	{
		GameObject fish = PhotonView.Find (id).gameObject;
		if (fish == null) {
			Debug.Log("Unable to make male mature - id not found: " + id);
			return;
		}
		fish.GetComponent<NetworkMaleReproduction> ().mature = true;
	}

	void FindFishAndConvertToPlayer (int id, List<int> playerIndexes, List<PathogenType> immunities,
	                                 bool mature, int foodCounter, float timeToDeath)
	{
		GameObject fish = PhotonView.Find (id).gameObject;
		NetworkHerm oldFish = fish.GetComponent<NetworkHerm> ();
		oldFish.ConvertToSelfPlayer (playerIndexes, immunities);
		Destroy(oldFish);
		fish.GetComponent<FishReproduction> ().mature = mature;
		fish.GetComponent<FishGrowth> ().FoodCounter = foodCounter;
		fish.GetComponent<FishHealth> ().timeToDeath = timeToDeath;
		CurrentPlayer = fish;
	}

	public override Stats GetStats ()
	{
		return new Stats (
			(Mathf.RoundToInt(Score * MultiplayerScoreData.instance.TotalFish / 100f)).ToString (),
			(MultiplayerScoreData.instance.TotalFish).ToString (),
			Score.ToString (),
			((int)timer) + " seconds",
			Format (Selfs, Crosses),
			Format (Crosses, Selfs)
		);
	}
	
	public void HandleFinalScoresReceived ()
	{
		MultiplayerUIController.instance.ShowGameOverPanel ();
	}

	public void EndGame ()
	{
		Time.timeScale = 0;
		Camera.main.GetComponent<CameraController> ().enabled = false;
		gameOver = true;
		SoundController.instance.PlaySingle (survivedAudio);
		MultiplayerUIController.instance.ShowGameOverPanel ();
	}
}
