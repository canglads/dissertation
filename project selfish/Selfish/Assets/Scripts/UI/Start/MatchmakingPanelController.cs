using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MatchmakingPanelController : MonoBehaviour {

	public InputField nameInputField;
	public Text messageText;
	public Button createGameButton;
	public Transform gameListContentPanel;
	public Text connectedText;
	public Image connectedBackground;
	public Color connectedColor;
	public Color notConnectedColor;
	public GameObject gameListItemPrefab;
	public string enterName = "Please enter a name";
	public string deleteFailedMessage = "Delete failed";
	public string createFailedMessage = "Create failed";
	public string serverFullMessage = "Server busy, please try again later";
	public string connectedMessage = "Connected";
	public string notConnectedMessage = "Not\nConnected";
	public string awaitingJoinMessage = "Waiting for other players to join the game";

	MultiplayerController mc;
	List<MultiplayerGameListItem> listItems;

	const string MultiplayerNamePrefsString = "MultiplayerName";

	public void HandleConnected ()
	{
		connectedBackground.color = connectedColor;
		connectedText.text = connectedMessage;
		SetEnableNameInput (true);
		if (nameInputField.text.Trim () == "")
			ShowMessage (enterName);
	}

	public void HandleNotConnected ()
	{
		connectedBackground.color = notConnectedColor;
		connectedText.text = notConnectedMessage;
		SetEnableNameInput (false);
		SetEnableForAllButtons (false);
		TryEnableButtons ();//in case connection lost and reacquired
	}
	// to be called when game is deleted by another player 
	// (as well as when player clicked leave)
	public void HandleThisPlayerLeftGame ()
	{
		SetEnableNameInput (true);
		SetEnableForAllButtons (true);
	}

	// OnEnable may be called before Start - so it is better to do the whole
	// initialisation here every time (given disconnecting on Back)
	void OnEnable ()
	{
		mc = MultiplayerController.instance;
		mc.matchmakingPanelController = this;
		listItems = new List<MultiplayerGameListItem> ();
		nameInputField.text = PlayerPrefs.GetString (MultiplayerNamePrefsString, "");
		if (PhotonNetwork.connected) {
			HandleConnected ();
			MultiplayerController.instance.UpdateRoomList ();
		} else
			MultiplayerController.instance.Connect ();
	}

	public void HandleServerFull ()
	{
		ShowMessage (serverFullMessage);
		SetEnableForAllButtons (false);
	}

	public void HandleBack ()
	{
		// this may be annoying for players but it is necessary to disconnect to minimise CCU's
		// if we have created any games, delete them
		if (PhotonNetwork.isMasterClient) {
			DeleteGameInList (mc.CurrentGame);
			mc.HandleDeleteGame(mc.CurrentGame, null);
		}
		mc.Disconnect ();
		gameObject.SetActive (false);
	}

	public void HandleCreateGame ()
	{
		mc.CreateGameOwnedByThisPlayer (CreateGameCallback);
	}

	public void CreateGameCallback (MultiplayerGame game, bool error)
	{
		if (error) {
			ShowMessage(createFailedMessage);
			return;
		}
		CreateGameInList (game, true);
		SetEnableNameInput (false);
		createGameButton.interactable = false;
		ShowMessage (awaitingJoinMessage);
	}

	public void SetEnableNameInput (bool value)
	{
		nameInputField.interactable = value;
	}

	public void HandleDeleteGame (MultiplayerGame game)
	{
		mc.HandleDeleteGame (game, DeleteGameCallback);
	}

	public void DeleteGameCallback (MultiplayerGame game, bool error)
	{
		if (error) {
			ShowMessage(deleteFailedMessage);
			return;
		}
		DeleteGameInList (game);
		SetEnableNameInput (true);
		createGameButton.interactable = true;
		ShowMessage ("");
	}

	public void HandleInputChanged ()
	{
		string input = nameInputField.text.Trim ();
		if (input != "")
			PlayerPrefs.SetString (MultiplayerNamePrefsString, input);
		TryEnableButtons ();
	}

	public void TryEnableButtons ()
	{
		string input = nameInputField.text.Trim();
		if (input == "") {
			ShowMessage (enterName);
			SetEnableForAllButtons (false);
		} else {
			ShowMessage ("");
			mc.Player.Name = input;
			SetEnableForAllButtons (true);
		}
	}

	void SetEnableForAllButtons (bool value)
	{
		createGameButton.interactable = value;
		foreach (MultiplayerGameListItem item in listItems)
			item.SetEnableJoinButton (value);
	}

	public void UpdateGameInList (MultiplayerGame game)
	{
		listItems.ForEach (item => {
			if (item.Game.Id == game.Id)
				item.UpdateNamesAndCount();
		});
	}

	public void DeleteGameInList (MultiplayerGame game)
	{
		nameInputField.interactable = true;
		MultiplayerGameListItem itemToRemove = null;
		listItems.ForEach (item => {
			if (item.Game == game) 
				itemToRemove = item;
		}); 
		if (itemToRemove != null) {
			listItems.Remove (itemToRemove);
			Destroy (itemToRemove.gameObject);
		}
	}

	public void CreateGameInList (MultiplayerGame game, bool ownedByThisPlayer)
	{
		GameObject go = Instantiate (gameListItemPrefab);
		MultiplayerGameListItem script = go.GetComponent<MultiplayerGameListItem> ();
		if (ownedByThisPlayer) {
			nameInputField.interactable = false;
			script.InitOwnedByThisPlayer (game);
		} else
			script.InitOwnedByOtherPlayer (game);
		listItems.Add (script);
		go.transform.SetParent(gameListContentPanel);
		go.transform.localScale = Vector3.one;
		TryEnableButtons ();
	}

	public void ShowMessage (string message)
	{
		messageText.text = message;
	}

	public void SetEnableCreateButton (bool value)
	{
		createGameButton.interactable = value;
	}

}
