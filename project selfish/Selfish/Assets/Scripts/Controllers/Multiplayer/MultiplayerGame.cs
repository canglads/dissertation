using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerGame : MonoBehaviour {

	public string Id { get; private set; }
	public List<Multiplayer> Players { get; private set; }
	// sometimes we do not have the players names but we can still display a count
	public int PlayerCount { get; set; }
	public bool IsMasterClient { get; private set; }

	public void Init (Multiplayer owner, string id)
	{
		Id = id;
		Players = new List<Multiplayer> () {owner};
		PlayerCount = 1;
		IsMasterClient = true;
	}

	public void Init (string id, int playerCount)
	{
		Id = id;
		PlayerCount = playerCount;
		Players = new List<Multiplayer> ();
	}

	public void AddPlayers (List<Multiplayer> players)
	{
		Players = players;
		PlayerCount = players.Count;
	}

	public void AddPlayer (Multiplayer player)
	{
		Players.Add (player);
		PlayerCount++;
	}

}
