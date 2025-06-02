using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultiplayerGameListItem : MonoBehaviour {
	
	public GameObject namesListItemPrefab;
	public string joinFailedMessage = "Join failed";
	public string leaveFailedMessage = "Leave failed";
	public string startFailedMessage = "Start failed";
	public string awaitingStartMessage = "Waiting for other player to start the game";
	public string startOrWaitMessage = "Start game now or wait for more players";
	public string maxReachedMessage = "Maximum players reached, please start game now";

	public MultiplayerGame Game { get; private set; }

	// all buttons should be set to inactive in editor
	// init methods will determine which are activated
	GameObject deleteButton;
	GameObject startButton; 
	GameObject joinButton;
	GameObject leaveButton;
	Text countText;
	Transform namesListContentPanel;
	MultiplayerController mc;
	MatchmakingPanelController mpc;
	bool ownedByThisPlayer;

	void Awake ()
	{
		mc = MultiplayerController.instance;
		mpc = mc.matchmakingPanelController;
		deleteButton = transform.Find ("DeleteButton").gameObject;
		startButton  = transform.Find ("StartButton").gameObject;
		joinButton = transform.Find ("JoinButton").gameObject;
		leaveButton = transform.Find ("LeaveButton").gameObject;
		countText = transform.Find ("CountText").GetComponent<Text> ();
		namesListContentPanel = transform.Find ("NamesListPanel").Find ("ScrollView").Find ("ContentPanel");
	}

	public void HandleDeleteGame ()
	{
		mpc.HandleDeleteGame (Game);
	}

	public void HandleStartGame ()
	{
		mc.LoadMultiplayerLevel ();
	}

	public void HandleJoinGame ()
	{
		mc.HandleJoinGame (mc.Player, Game, JoinGameCallback);
	}

	public void JoinGameCallback (bool error)
	{
		if (error) {
			mpc.ShowMessage(joinFailedMessage);
			return;
		}
		leaveButton.SetActive (true);
		joinButton.SetActive (false);
		UpdateNamesAndCount ();
		mpc.SetEnableNameInput (false);
		mpc.SetEnableCreateButton (false);
		mpc.ShowMessage (awaitingStartMessage);
	}

	public void HandleLeaveGame ()
	{
		mc.HandleLeaveGame (mc.Player, Game, LeaveGameCallback);
	}

	public void LeaveGameCallback (bool error)
	{
		if (error) {
			mpc.ShowMessage(leaveFailedMessage);
			return;
		}
		leaveButton.SetActive (false);
		joinButton.SetActive (true);
		UpdateNamesAndCount ();
		mpc.SetEnableNameInput (true);
		mpc.SetEnableCreateButton (true);
		mpc.ShowMessage ("");
	}

	public void SetEnableJoinButton (bool value)
	{
		joinButton.GetComponent<Button>().interactable = value && 
			Game.Players.Count < mc.maxPlayersPerGame;
	}

	void EnableOrDisableJoin ()
	{
		bool enabled = Game.Players.Count < mc.maxPlayersPerGame;
		joinButton.GetComponent<Button>().interactable = enabled;
	}

	public void UpdateNamesAndCount ()
	{
		countText.text = Game.PlayerCount.ToString ();
		RefreshNamesList ();
		if (ownedByThisPlayer)
			EnableOrDisableStart ();
		else
			EnableOrDisableJoin ();
	}

	void EnableOrDisableStart ()
	{
		startButton.GetComponent<Button> ().interactable = Game.Players.Count > 1;
		if (Game.Players.Count == mc.maxPlayersPerGame)
			mpc.ShowMessage (maxReachedMessage);
		else if (Game.Players.Count > 1)
			mpc.ShowMessage (startOrWaitMessage);
	}

	public void InitOwnedByThisPlayer (MultiplayerGame game)
	{
		Game = game;
		ownedByThisPlayer = true;
		RefreshNamesList ();
		deleteButton.SetActive (true);
		startButton.SetActive (true);
	}

	public void InitOwnedByOtherPlayer (MultiplayerGame game)
	{
		Game = game;
		RefreshNamesList ();
		joinButton.SetActive (true);
	}

	void RefreshNamesList ()
	{
		TransformUtil.DestroyAllChildren (namesListContentPanel);
		foreach (Multiplayer player in Game.Players) {
			GameObject item = Instantiate(namesListItemPrefab);
			item.GetComponent<Text>().text = player.Name;
			item.transform.SetParent(namesListContentPanel);
			item.transform.localScale = Vector3.one;
		}
	}

}
