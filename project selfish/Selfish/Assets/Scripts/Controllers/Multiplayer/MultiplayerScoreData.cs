using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Photon;

[Serializable]
public class MultiplayerScore {
	public int PlayerId { get; private set; }
	public int Score { get; set; }
	public MultiplayerScore (int playerId) {
		PlayerId = playerId;
	}
	public MultiplayerScore (int playerId, int score) {
		PlayerId = playerId;
		Score = score;
	}
}

public class MultiplayerScoreData : PunBehaviour {

	public static MultiplayerScoreData instance;
	
	public MultiplayerScore[] Scores { 
		get {
			return scores;
		}
		set {
			if (!disabled)
				scores = value;
		}
	}
	// no need to send player id's as well when syncing
	// PUN can't serialize complex objects without custom
	// methods, so this needs writing somewhere, may as well be here
	public int[] NetworkScores { 
		get {
			int[] networkScores = new int[Scores.Length];
			for (int i = 0; i < Scores.Length; i++)
				networkScores [i] = Scores[i].Score;
			return networkScores;
		}
		private set {
			for (int i = 0; i < Scores.Length; i++)
				Scores[i].Score = value[i];
		}
	}
	public int TotalFish { get; set; }

	MultiplayerScore[] scores;
	bool disabled;

	void Awake ()
	{
		instance = this;
	}

	public void Init (List<Multiplayer> players)
	{
		Scores = new MultiplayerScore[players.Count];
		for (int i = 0; i < players.Count; i++)
			Scores[i] = new MultiplayerScore(players[i].Id);
	}

	public void SendFinalScores ()
	{
		disabled = true;
		RpcController.instance.SendFinalScores (NetworkScores, TotalFish);
	}

	public void SetFinalScores (int[] finalScores, int totalFish)
	{
		disabled = true;
		NetworkScores = finalScores;
		TotalFish = totalFish;
	}

	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (disabled)
			return;
		if (stream.isWriting) {// masterclient
			stream.SendNext (NetworkScores);
			stream.SendNext (TotalFish);
		} else {// nonmasterclient
			if (Scores == null)
				return;// if not initialised yet, just ignore updates until we are
			NetworkScores = (int[]) stream.ReceiveNext();
			GameController.instance.sc.Score = Scores[MultiplayerController.instance.GetPlayerIndex()].Score;
			TotalFish = (int) stream.ReceiveNext ();
		}
		MultiplayerUIController.instance.UpdateScores (Scores);
	}

}
