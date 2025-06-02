using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class CustomSceneController : SinglePlayerSceneController {

	//public static SinglePlayerSceneController instance;
	public int defaultPathogenDiversity = 5;
	// for brevity in CustomPanelController we expose param here
	public ParameterController publicParam;

	bool initialised;

	public void Spawn ()
	{
		CreateFish ();
		SpawnOrganisms (SpawnFoodOrganism, param.initialFoodCount);
		SpawnPathogens ();
		SpawnOrganisms (SpawnPredator, param.initialPredatorCount);
		uiController.UpdateFood (param.foodToMature);
		timer = param.levelLength;
		if (param.displayImmPanels)
			uiController.ShowOwnImmPanel ();
		initialised = true;
	}

	// expects values from 1 to pathogenTemplates.Length 
	public void SetPathogenDiversity (int diversity)
	{
		if (diversity < 1 || diversity == pathogenTemplates.Length)
			return;// no change needed or, if invalid, just silently returns
		GameObject[] newPathogens = new GameObject[diversity];
		for (int i = 0; i < diversity;) {
			GameObject possible = pathogenTemplates[Random.Range (0, pathogenTemplates.Length)];
			if (!Contains(newPathogens, possible)) {
				newPathogens[i] = possible;
			    i++;
			}
		}
		pathogenTemplates = newPathogens;
	}

	protected override void Awake ()
	{
		//instance = this;
		base.Awake ();
	}
	protected override void Start ()
	{
		StartCoroutine (AppendToReportAndChangeTemp ());
		uiController = UIController.instance;
		startPos = StartPositionsController.instance;
		param = ParameterController.instance;
		publicParam = param;
		uiController.DisplayImmPanels = param.displayImmPanels;
		CustomPanelController.instance.Init ();
		uiController.EnableTempPanel ();
		param.Temp = PlayerPrefs.GetInt ("Temperature", 22);
		uiController.DisplayCurrentTemp (param.Temp);
		
	}


	private IEnumerator AppendToReportAndChangeTemp(){
		while(true)
		{
			if(!gameOver){
				yield return new WaitForSeconds(1f);
				CSVManager.GetStats(GetStats());
				CSVManager.AppendToReport();
				CSVManager.SetTemp(param.Temp);
				int dice1 = Random.Range(0, 100);
				if(dice1 > 70){
					int dice2 = Random.Range(0,2);
					if(dice2 == 0){
						if(param.Temp < 38){
						param.Temp++;
						}
					}
					else{
						if(param.Temp > 12){
						param.Temp--;
						}
					}
					uiController.DisplayCurrentTemp (param.Temp);
					CustomPanelController.instance.ReproductionCurve(param.Temp);
				}
			} 	
		} 
	}

	// private IEnumerator ChangeTemperature(){
	// 	while(timer != 0){
	// 		yield return new WaitForSeconds(Random.Range(2, 11));
	// 		int dice = Random.Range(0, 2);
			
	// 	}
	// }

	protected override void Update ()
	{
		if (initialised){
			base.Update ();
		}
	}

	protected override void HandleSurvived ()
	{
		Time.timeScale = 0;
		Camera.main.GetComponent<CameraController> ().enabled = false;
		gameOver = true;
		SoundController.instance.PlaySingle (survivedAudio);
		uiController.ShowSurvivedPanel (SurvivedPanelController.Level.Custom);
	}

	protected override void HandleGameOverDead ()
	{
		PauseAction ();
		gameOver = true;
		uiController.touchPanelController.enabled = false;
		SoundController.instance.PlaySingle (deadAudio);
		RecalculateScores ();
		uiController.ShowDeadPanel ();
	}

	// Array.Contains does not exist in this version of Mono
	bool Contains (GameObject[] gos, GameObject toFind)
	{
		foreach (GameObject toCheck in gos)
			if (toCheck == toFind) // identity not equality
				return true;
		return false;
	}

}