using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Multiplayer {
	public int Id { get; set; }
	public string Name { get; set; }
	public Multiplayer(int id, string name) {
		Id = id;
		Name = name;
	}
}
// implemented largely independently on Photon Unity Networking
// in an attempt to provide a level of abstraction
// i.e. should be able to replace PunController with an instance 
// of a class with a similar interface if want to use a different 
// networking library
public class MultiplayerController : MonoBehaviour {

	public static MultiplayerController instance;

	public MatchmakingPanelController matchmakingPanelController;
	public int maxPlayersPerGame = 4;

	public CallbackWithId CreateGameCallback { get; private set; }
	public CallbackWithPlayers JoinRoomCallback { get; private set; }
	public Callback LeaveGameCallback { get; private set; }
	public MultiplayerGame CurrentGame { get; private set; }
	public Multiplayer Player { get; set; }

	public delegate void Callback (bool error);
	public delegate void CallbackWithId (string id, bool error);
	public delegate void CallbackWithPlayers (List<Multiplayer> players, bool error);
	public delegate void CallbackWithGame (MultiplayerGame game, bool error);

	PunController pc;
	List<MultiplayerGame> games;
	public bool Playing { get; private set; }//this player is currently playing the multiplayer level not in the start UI

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);	
		}
		DontDestroyOnLoad (gameObject);
		pc = GetComponent<PunController> ();
		games = new List<MultiplayerGame> ();
		Player = new Multiplayer (0, SystemInfo.deviceUniqueIdentifier);
	}

	// called on master client having left
	public void EndGame ()
	{
		NonMasterClientSceneController nmcc = GameController.instance.sc as NonMasterClientSceneController;
		if (nmcc == null) // if this has backed up and we are no longer in the right scene
			return;
		nmcc.EndGame ();
	}

	public void HandleConnected ()
	{
		if (matchmakingPanelController != null)
			matchmakingPanelController.HandleConnected ();
	}

	public void HandleNotConnected ()
	{
		matchmakingPanelController.HandleNotConnected ();
	}

	public void HandleNewPlayer (Multiplayer newPlayer, string gameId)
	{
		MultiplayerGame game = GetGame (gameId);
		game.AddPlayer (newPlayer);
		matchmakingPanelController.UpdateGameInList (game);
	}

	public void Connect ()
	{
		pc.Connect ();
	}

	public void UpdateGame (string name, int playerCount)
	{
		MultiplayerGame game = GetGame (name);
		game.PlayerCount = playerCount;
		matchmakingPanelController.UpdateGameInList (game);
	}

	public void UpdateGame (string name, List<Multiplayer> players)
	{
		MultiplayerGame game = GetGame (name);
		game.AddPlayers (players);
		matchmakingPanelController.UpdateGameInList (game);
	}

	MultiplayerGame GetGame (string id)
	{
		foreach (MultiplayerGame game in games) {
			if (game.Id == id)
				return game;
		};
		return null;
	}

	public void HandleGameDeleted (string id)
	{
		foreach (MultiplayerGame game in games) {
			if (game.Id == id) {
				matchmakingPanelController.DeleteGameInList (game);
				return;
			}
		}	
	}

	public void HandleDeleteGame (MultiplayerGame game, CallbackWithGame callback)
	{
		pc.DeleteRoom (game.Id);
		games.Remove (game);
		if (callback != null) 
			callback (game, false);
		Destroy (game);
		CurrentGame = null;
	}

	public void HandleThisPlayerLeftGame ()
	{
		if (Playing)
			HandleKickedDuringGame ();
		else if (matchmakingPanelController != null)
			matchmakingPanelController.HandleThisPlayerLeftGame ();
		// else normal exit at end of game, take no action
	}

	// called by incoming network message - no need to inform network
	public void CreateGameOwnedByOtherPlayer (string id, int playerCount)
	{
		MultiplayerGame game = gameObject.AddComponent<MultiplayerGame> ();
		game.Init (id, playerCount);
		games.Add (game);
		matchmakingPanelController.CreateGameInList (game, false);
	}

	// called by UI interaction - need to inform network
	public void CreateGameOwnedByThisPlayer (CallbackWithGame callback)
	{
		pc.CreateRoom ();
		CreateGameCallback = delegate (string id, bool error) {
			if (error) {
				callback(null, true);
				return;
			}
			PhotonNetwork.player.name = Player.Name;
			MultiplayerGame game = gameObject.AddComponent<MultiplayerGame> ();
			game.Init (Player, id);
			CurrentGame = game;
			games.Add (game);
			callback (game, false);
			CreateGameCallback = null;
		};
	}

	public void LoadMultiplayerLevel ()
	{
		Playing = true;
		pc.LoadMultiplayerLevel ();
	}

	public void HandleJoinGame (Multiplayer joiningPlayer, MultiplayerGame game, Callback callback)
	{
		pc.JoinRoom (game.Id, joiningPlayer.Name);
		JoinRoomCallback = delegate (List<Multiplayer> players, bool error) {
			if (error) {
				callback (true);
				return;
			}
			CurrentGame = game;
			game.AddPlayers(players);
			callback (false);
			JoinRoomCallback = null;
		};
	}

	public void HandleLeaveGame (Multiplayer leavingPlayer, MultiplayerGame game, Callback callback)
	{
		pc.HandleLeaveRoom ();
		LeaveGameCallback = delegate (bool error) {
			if (error) {
				callback (true);
				return;
			}
			CurrentGame = null;
			game.Players.Remove (leavingPlayer);
			callback (false);
			LeaveGameCallback = null;
		};
	}

	public void HandleServerFull ()
	{
		matchmakingPanelController.HandleServerFull ();
	}

	public void HandleExitDuringGame ()
	{
		pc.HandleLeaveRoom ();
		LeaveGameCallback = delegate (bool error) {
			if (error) 
				return;// just return, player will retry
			Playing = false;
			Application.LoadLevel(0);
		};
	}

	public void HandleKickedDuringGame ()
	{
		Playing = false;
		// ideally notify player as well, but is an unlikely event
		Application.LoadLevel(0);
	}

	public int GetPlayerIndex ()
	{
		return GetPlayerIndexFromId (Player.Id);
	}

	public int GetPlayerIndexFromId (int id)
	{
		int count = 0;
		foreach (Multiplayer multiplayer in CurrentGame.Players) {
			if (multiplayer.Id == id)
				return count;
			count++;
		};
		Debug.Log ("Failed to identify player in player list.");
		return -1;
	}

	public void UpdateRoomList ()
	{
		pc.OnReceivedRoomListUpdate ();
	}

	public void Disconnect ()
	{
		pc.Disconnect ();
	}
}
