using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SinglePlayerSceneController : AiSceneController {

	public static SinglePlayerSceneController instance;

	public bool introLevel;

	public int DirectFishCount { get; set; }
	public bool Pressed { get; set; }

	LevelController levelController;
	bool paused;

	public override void DestroyOrganism (GameObject organism)
	{
		Destroy (organism);
	}

	public override void PropagateMakeMaleMature (GameObject male)
	{
		// single player mode - no need to tell anyone else
	}

	protected override void CreateFish ()
	{
		base.CreateFish ();
		RecalculateScores ();
	}

	public void ShowPauseScreenAndPauseAction ()
	{
		PauseAction ();
		uiController.ShowPausedPanel ();
	}
	
	public void HidePauseScreenAndUnpauseAction ()
	{
		uiController.HidePausedPanel ();
		UnpauseAction ();
	}

	public void ShowMessage (string message, Action callback)
	{
		PauseAction ();
		UIController.instance.ShowMessage (message);
		StartCoroutine (AwaitPress (callback));
	}

	public override void RecalculateScores ()
	{
		// prevent div by 0
		if (Herms.Count + Males.Count == 0)
			Score = 0;
		else
			Score = Mathf.RoundToInt(DirectFishCount * 100f / (Herms.Count + Males.Count));
		uiController.DisplayScore (Score);
	}
	
	public override void HandleCurrentPlayerFishDead ()
	{
		Destroy (CurrentPlayer);
		if (DirectFishCount == 0)
			HandleGameOverDead ();
		else {//switch
			SoundController.instance.PlaySingle (switchAudio);
			GameObject switchFish = GetSwitchFish (0);// player index in single player always 0
			if (switchFish == null) {
				HandleGameOverDead ();//belt and braces
				return;
			}
			CurrentPlayer = switchFish;
			AiHerm oldFishScript = switchFish.GetComponent<AiHerm>();
			oldFishScript.ConvertToSelfPlayer();
			Destroy (oldFishScript);
			uiController.HandleSwitchCurrentPlayer ();
			if (param.displayImmPanels)
				uiController.ShowOwnImmPanel ();
			RecalculateScores ();
		}
	}

	public override void RemoveHerm (GameObject herm)
	{
		if (herm.GetComponent<Fish> ().PlayerIndexes.Count == 1)
			DirectFishCount--;
		base.RemoveHerm (herm);
	}

	public override void RemoveMale (GameObject male)
	{
		if (male.GetComponent<Fish> ().PlayerIndexes.Count == 1)
			DirectFishCount--;
		base.RemoveMale (male);
	}

	protected override void Awake ()
	{
		instance = this;
		base.Awake ();
		levelController = GetComponent<LevelController> ();
	}

	protected override void Start ()
	{
		base.Start ();
		if (levelController != null)
			levelController.Init ();
	}

	protected override void Update ()
	{
		if (!paused)
			base.Update ();
		if (Input.GetButton ("Pause"))
			ShowPauseScreenAndPauseAction ();
	}

	protected override int GetCountOfFishWithPlayerIndex ()
	{
		return DirectFishCount;
	}

	protected override void CreateSelfPlayer (bool startOfLevel)
	{
		CurrentPlayer = Instantiate (playerTemplate, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		CurrentPlayer.GetComponent<Fish>().PlayerIndexes.Add(0);
		DirectFishCount++;
		base.CreateSelfPlayer (startOfLevel);
	}

	protected override void HandleSurvived ()
	{
		base.HandleSurvived ();
		GameController.instance.RecordScore (Score);
		if (GameController.instance.LastLevel)
			ShowHighScoreThenSurvivedGame ();
		else
			uiController.ShowSurvivedPanel (introLevel ? SurvivedPanelController.Level.Intro
			                                : SurvivedPanelController.Level.Advanced);
		// but maybe show some messages over the top
		if (levelController != null)
			levelController.HandleSurvived ();
	}

	protected override void SpawnHerm (bool startOfLevel)
	{
		GameObject newHerm = Instantiate (hermTemplate, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation()) as GameObject;
		Herms.Add (newHerm);
		AddRandomImmunities (newHerm);
	}

	protected override void SpawnMale (bool startOfLevel)
	{
		GameObject newMale = Instantiate (maleTemplate, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation()) as GameObject;
		Males.Add (newMale);
		AddRandomImmunities (newMale);
	}
	
	protected override void SpawnPathogen (GameObject template, bool startOfLevel)
	{
		Pathogens[template.GetComponent<Pathogen>().type].Add (Instantiate (template, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation()) as GameObject);
	}
	
	protected override void SpawnFoodOrganism (bool startOfLevel)
	{
		FoodOrganisms.Add (Instantiate (foodOrganismTemplate, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation()) as GameObject);
	}
	
	protected override void SpawnPredator (bool startOfLevel)
	{
		Predators.Add (Instantiate (predatorTemplate, startPos.GetRandomStartPosition (startOfLevel), GetRandomRotation()) as GameObject);
	}

	protected override void CreateOffspring (List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position)
	{
		if (playerIndexes.Count == 1)
			DirectFishCount++;
		float maleRatio = self ? param.hermReproductionParameters.maleRatioSelf : param.hermReproductionParameters.maleRatioCross;
		bool male = Random.value < maleRatio;
		GameObject template = male ? maleTemplate : hermTemplate;
		GameObject newNpc = Instantiate (template, position, GetRandomRotation ()) as GameObject;
		InitNpcFish (newNpc, male, playerIndexes, immunities);
	}

	protected virtual void HandleGameOverDead ()
	{
		PauseAction ();
		gameOver = true;
		uiController.touchPanelController.enabled = false;
		SoundController.instance.PlaySingle (deadAudio);
		RecalculateScores ();
		ShowHighScoreThenDead ();
	}

	protected void PauseAction ()
	{
		paused = true;
		Time.timeScale = 0;
		Camera.main.GetComponent<CameraController> ().enabled = false;
	}
	
	void UnpauseAction ()
	{
		paused = false;
		Time.timeScale = 1;
		Camera.main.GetComponent<CameraController> ().enabled = true;
	}

	IEnumerator AwaitPress (Action callback)
	{
		while (!Pressed) {
			yield return null;
		}
		Pressed = false;
		uiController.HideMessage ();
		if (!gameOver)
			UnpauseAction ();
		if (callback != null)
			callback ();
	}

	void ShowHighScoreThenDead ()
	{
		if (!InitialScoreCheck ()) {
			uiController.ShowDeadPanel ();
			return;
		}
		//uiController.PreventInteraction ();// this is never re-enabled on some platforms under some conditions - may function adequately without
		GameController.instance.scoresWebServiceClient.Get (HighScoreDeadCallback);
	}
	
	void HighScoreDeadCallback (ScoresData data)
	{
		// always show this...
		uiController.ShowDeadPanel ();
		// ...but maybe temporarily cover with this:
		if (data != null && GameController.instance.TotalScore > GameController.GetLowestScore (data)) {
			uiController.ShowHighScorePanel (GameController.instance.TotalScore);
		}
		//uiController.EnableInteraction ();
	}
	
	void ShowHighScoreThenSurvivedGame ()
	{
		if (!InitialScoreCheck ()) {
			uiController.ShowSurvivedPanel (SurvivedPanelController.Level.Last);
			return;
		}
		//uiController.PreventInteraction ();
		GameController.instance.scoresWebServiceClient.Get (HighScoreSurvivedGameCallback);
	}
	
	void HighScoreSurvivedGameCallback (ScoresData data)
	{
		// always show this...
		uiController.ShowSurvivedPanel (SurvivedPanelController.Level.Last);
		// ...but maybe temporarily cover with this:
		if (GameController.instance.TotalScore > GameController.GetLowestScore(data))
			uiController.ShowHighScorePanel (GameController.instance.TotalScore);
		uiController.EnableInteraction ();
	}
	
	bool InitialScoreCheck ()
	{
		return GameController.instance.TotalScore > GameController.instance.LowestHighScore;
	}

	protected override void KillHerms (int number)
	{
		for (int i = 0; i < number;) {
			GameObject herm = Herms[Random.Range(0, Herms.Count)];
			if (herm.GetComponent<SelfPlayer>() == null) {
				herm.GetComponent<AiHealth>().HandleDeath();
				i++;
			}
		}
	}

	protected override void KillOrganism (List<GameObject> organisms)
	{
		KillOrganism (organisms);
		int index = Random.Range (0, organisms.Count);
		GameObject organism = organisms[index];
		organisms.RemoveAt(index);
		Destroy (organism);
	}

	void InitNpcFish (GameObject npcFish, bool male, List<int> playerIndexes, List<PathogenType> immunities)
	{
		npcFish.transform.localScale = new Vector3 (0.7f, 0.7f, 1f);
		AiFish fish = npcFish.GetComponent<AiFish> ();
		if (playerIndexes != null)
			fish.PlayerIndexes = playerIndexes;
		fish.ImmuneSystem.Immunities = immunities;
		if (male)
			Males.Add (npcFish);
		else
			Herms.Add (npcFish);
	}

}
