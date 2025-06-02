using UnityEngine;
using System.Collections;
using Photon;
using System.Collections.Generic;

public class PunController : PunBehaviour {

	public static PunController instance;

	MultiplayerController mc;
	RoomInfo[] rooms;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);	
		}
		DontDestroyOnLoad (gameObject);
		mc = GetComponent<MultiplayerController> ();
		PhotonNetwork.automaticallySyncScene = true;
	}

	public void LoadMultiplayerLevel ()
	{
		PhotonNetwork.room.open = false;
		PhotonNetwork.room.visible = false;
		PhotonNetwork.LoadLevel ("Multiplayer");
	}

	public void Connect ()
	{
		if (!PhotonNetwork.connected)
			PhotonNetwork.ConnectUsingSettings (GameController.instance.version);
		else
			OnReceivedRoomListUpdate ();
	}

	public void CreateRoom ()
	{
		RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 4 };
		PhotonNetwork.CreateRoom (null, roomOptions, null);
	}

	public override void OnPhotonCreateRoomFailed (object[] codeAndMsg)
	{
		if (mc.CreateGameCallback != null)
			mc.CreateGameCallback (null, true);
	}

	public override void OnJoinedRoom ()
	{
		// will be called on client who created room as well
		// so we can do this here...
		mc.Player.Id = PhotonNetwork.player.ID;
		// ...but they don't want to do anything else here
		if (PhotonNetwork.isMasterClient)
			return;
		// this probably covers it anyway but belt and braces
		if (mc.JoinRoomCallback != null) {
			mc.JoinRoomCallback (ToMultiplayerList(PhotonNetwork.playerList), false);
		}
	}

	public override void OnPhotonJoinRoomFailed (object[] codeAndMsg)
	{
		if (mc.JoinRoomCallback != null)
			mc.JoinRoomCallback (null, true);
	}

	public override void OnCreatedRoom ()
	{
		if (mc.CreateGameCallback != null) {
			mc.CreateGameCallback (PhotonNetwork.room.name, false);
		}
	}

	public override void OnPhotonPlayerConnected (PhotonPlayer player)
	{
		mc.HandleNewPlayer (new Multiplayer (player.ID, player.name), PhotonNetwork.room.name);
	}

	public override void OnLeftRoom ()
	{
		// if forced out by delete game call this:
		mc.HandleThisPlayerLeftGame ();
		// only if invoked as leaving someone else's game, does this get called as well
		if (mc.LeaveGameCallback != null)
			mc.LeaveGameCallback (false);
	}

	public void DeleteRoom (string id)
	{
		foreach (PhotonPlayer player in PhotonNetwork.otherPlayers)
			PhotonNetwork.CloseConnection (player);
		PhotonNetwork.CloseConnection (PhotonNetwork.player);
	}

	public void JoinRoom (string roomName, string playerName)
	{
		PhotonNetwork.player.name = playerName;
		PhotonNetwork.JoinRoom (roomName);
	}

	public void HandleLeaveRoom ()
	{
		PhotonNetwork.LeaveRoom ();
	}

	public override void OnJoinedLobby ()
	{
		mc.HandleConnected ();
		OnReceivedRoomListUpdate ();
	}

	public override void OnDisconnectedFromPhoton ()
	{
		mc.HandleNotConnected ();
	}

	public override void OnReceivedRoomListUpdate ()
	{
		// ignore during or at end of game
		if (mc.Playing || mc.matchmakingPanelController == null)
			return;
		RoomInfo[] newRooms = PhotonNetwork.GetRoomList ();
		if (rooms == null) {//first time called after join lobby
			rooms = newRooms;
			return;
		}
		foreach (RoomInfo newRoom in newRooms) {
			if (ContainsRoom(rooms, newRoom))
				UpdateRoomMembers (newRoom);
			else
				mc.CreateGameOwnedByOtherPlayer (newRoom.name, newRoom.playerCount);
		}
		foreach (RoomInfo oldRoom in rooms) {
			if (!ContainsRoom(newRooms, oldRoom))
			    mc.HandleGameDeleted (oldRoom.name);
		}
		rooms = newRooms;
	}

	public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{ 
		// whoever the new master client is they will not have the correct data
		// to continue controlling the game, so we just gameover for all remaining
		// players as soon as the master client leaves (the former master will not
		// receive this callback as they will already have left the room)
		mc.EndGame ();
	}

	void UpdateRoomMembers (RoomInfo room)
	{
		if (room.isLocalClientInside)
			mc.UpdateGame (room.name, ToMultiplayerList (PhotonNetwork.playerList));
		else
			mc.UpdateGame (room.name, room.playerCount);
	}

	List<Multiplayer> ToMultiplayerList (PhotonPlayer[] photonPlayers)
	{
		List<Multiplayer> multiplayers = new List<Multiplayer>();
		foreach (PhotonPlayer photonPlayer in photonPlayers) {
			multiplayers.Add (new Multiplayer(photonPlayer.ID, photonPlayer.name));
		}
		return multiplayers;
	}

	bool ContainsRoom(RoomInfo[] roomsToCheck, RoomInfo roomToFind)
	{
		foreach (RoomInfo roomToCheck in roomsToCheck)
			if (roomToCheck.name == roomToFind.name)
				return true;
		return false;
	}

	public override void OnPhotonMaxCccuReached ()
	{
		mc.HandleServerFull ();
	}

	public void Disconnect ()
	{
		PhotonNetwork.Disconnect ();
	}

}
