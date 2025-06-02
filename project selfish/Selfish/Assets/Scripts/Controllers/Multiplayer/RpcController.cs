using UnityEngine;
using System.Collections;
using Photon;
using System.Collections.Generic;

public class RpcController : PunBehaviour {

	public static RpcController instance;

	void Awake ()
	{
		instance = this;
	}

	public void PropagateInitMale (int id, List<int> playerIndexes, List<PathogenType> immunities)
	{
		photonView.RPC ("InitMale", PhotonTargets.Others, id, playerIndexes.ToArray(), immunities.ToArray());
	}

	public void PropagateMakeMaleMature (int id)
	{
		photonView.RPC ("MakeMaleMature", PhotonTargets.Others, id);
	}

	public void SendFinalScores (int[] scores, int totalFish)
	{
		photonView.RPC ("ReceiveFinalScores", PhotonTargets.Others, scores, totalFish);
	}

	// only to be called on non-master
	[PunRPC]
	public void ReceiveFinalScores (int[] scores, int totalFish)
	{
		NonMasterClientSceneController sc = GameController.instance.sc as NonMasterClientSceneController;
		if (sc == null) {
			Debug.Log ("RPC: FinalScoresSent called on master client");
			return;
		}
		MultiplayerScoreData.instance.SetFinalScores (scores, totalFish);
		sc.HandleFinalScoresReceived ();
	}

	// only to be called on non-master
	[PunRPC]
	public void InitMale (int id, int[] playerIndexes, int[] immunities)
	{
		NonMasterClientSceneController sc = GameController.instance.sc as NonMasterClientSceneController;
		if (sc == null) {
			Debug.Log ("RPC: InitMale called on master client");
			return;
		}
		sc.InitMale (id,  ToList(playerIndexes), ToImmunityList(immunities));
	}

	// only to be called on non-master
	[PunRPC]
	public void MakeMaleMature (int id)
	{
		NonMasterClientSceneController sc = GameController.instance.sc as NonMasterClientSceneController;
		if (sc == null) {
			Debug.Log ("RPC: MakeMaleMature called on master client");
			return;
		}
		sc.MakeMaleMature (id);
	}
	
	// only to be called on master
	[PunRPC]
	void CreateOffspring (int number, int[] playerIndexes, int[] immunities, bool self, Vector3 position)
	{
		MasterClientSceneController sc = GameController.instance.sc as MasterClientSceneController;
		if (sc == null) {
			Debug.Log ("RPC: CreateOffspring called on non-master client");
			return;
		}
		sc.CreateOffspring (number, ToList(playerIndexes), ToImmunityList(immunities), self, position);
	}

	// only to be called on master
	[PunRPC]
	void HandleOtherPlayerDeath (int deadFishId, int playerIndex, PhotonMessageInfo info)
	{
		MasterClientSceneController sc = GameController.instance.sc as MasterClientSceneController;
		if (sc == null) {
			Debug.Log ("RPC: HandleOtherPlayerDeath called on non-master client");
			return;
		}
		GameObject fish = sc.GetNewFishForOtherPlayer (deadFishId, playerIndex, info.sender);
		if (fish == null)
			photonView.RPC ("ReceiveNewPlayerFish", info.sender, -1, null, null, false, -1, -1f);
		else
			photonView.RPC ("ReceiveNewPlayerFish", info.sender, 
		                fish.GetComponent<PhotonView>().viewID,
		                fish.GetComponent<Fish>().PlayerIndexes.ToArray(),
		                fish.GetComponent<ImmuneSystem>().Immunities.ToArray(),
			            fish.GetComponent<FishReproduction>().mature,
			            fish.GetComponent<FishGrowth>().FoodCounter,
			            fish.GetComponent<FishHealth>().timeToDeath);
	}

	// only to be called on non-master
	[PunRPC]
	void ReceiveNewPlayerFish (int id, int[] playerIndexes, int[] immunities, bool mature,
	                           int foodCounter, float timeToDeath)
	{
		NonMasterClientSceneController sc = GameController.instance.sc as NonMasterClientSceneController;
		if (sc == null) {
			Debug.Log ("RPC: ReceiveNewPlayerFish called on master client");
			return;
		}
		sc.SwitchToNewFish (id, ToList(playerIndexes), ToImmunityList(immunities), mature,
		                    foodCounter, timeToDeath);
	}

	public void AskMasterToCreateOffspring (int number, List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position)
	{
		photonView.RPC ("CreateOffspring", PhotonNetwork.masterClient, number, playerIndexes.ToArray(), immunities.ToArray(), self, position);
	}

	public void InformMasterOfDeath (int deadFishId, int playerIndex)
	{
		photonView.RPC ("HandleOtherPlayerDeath", PhotonNetwork.masterClient, deadFishId, playerIndex);
	}

	List<int> ToList(int[] array)
	{
		List<int> list = new List<int> ();
		if (array != null)
			list.AddRange (array);
		return list;
	}
	
	List<PathogenType> ToImmunityList(int[] array)
	{
		List<PathogenType> list = new List<PathogenType> ();
		if (array != null)
			foreach (int pathogenTypeAsInt in array)
				list.Add ((PathogenType)pathogenTypeAsInt);
		return list;
	}

}
