using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public abstract class AiSceneController : SceneController
{	

	public GameObject hermTemplate;
	public GameObject maleTemplate;
	public GameObject foodOrganismTemplate;
	public GameObject predatorTemplate;
	public GameObject[] pathogenTemplates;

	public List<GameObject> Herms { get; private set; }
	public List<GameObject> Males { get; protected set; }
	public Dictionary<PathogenType, List<GameObject>> Pathogens { get; protected set; }
	public List<GameObject> FoodOrganisms { get; protected set; }
	public List<GameObject> Predators { get; protected set; }

	public abstract void RecalculateScores ();
	public abstract void DestroyOrganism (GameObject organism); 

	protected abstract void SpawnHerm (bool startOfLevel);
	protected abstract void SpawnMale (bool startOfLevel);
	protected abstract void SpawnPathogen (GameObject template, bool startOfLevel);
	protected abstract void SpawnFoodOrganism (bool startOfLevel);
	protected abstract void SpawnPredator (bool startOfLevel);
	protected abstract void KillOrganism (List<GameObject> organisms);
	protected abstract void KillHerms (int number);
	protected abstract void CreateOffspring (List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position);

	protected delegate void SpawnDelegate (bool startOfLevel);

	public override void CreateOffspring (int numberOfOffspring, List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position)
	{
		for (int i = 0; i < numberOfOffspring; i++)
			CreateOffspring (playerIndexes, immunities, self, position);
		RecalculateScores ();
	}

	public override void RemoveHerm (GameObject herm)
	{
		Herms.Remove (herm);
	}

	public virtual void RemoveMale (GameObject male)
	{
		Males.Remove (male);
	}
	
	protected override void Awake ()
	{
		base.Awake ();
		Herms = new List<GameObject>();
		Males = new List<GameObject> ();
		FoodOrganisms = new List<GameObject> ();
		Predators = new List<GameObject> ();
		Pathogens = new Dictionary<PathogenType, List<GameObject>> ();
	}

	protected override void Start ()
	{
		base.Start ();
		CreateFish ();
		SpawnOrganisms (SpawnFoodOrganism, param.initialFoodCount);
		SpawnPathogens ();
		SpawnOrganisms (SpawnPredator, param.initialPredatorCount);
		uiController.UpdateFood (param.foodToMature);
		if (param.displayImmPanels)
			uiController.ShowOwnImmPanel ();
		StartCoroutine(AppendToReport());
	}

	protected override void Update ()
	{
		base.Update ();
		if (!gameOver) {
			CapOrganismNumbers ();
			Respawn ();
		} 
		GetStats();
	}

	private IEnumerator AppendToReport(){
		while(true)
		{
			if(!gameOver){
				yield return new WaitForSeconds(1f);
				CSVManager.GetStats(GetStats());
				CSVManager.AppendToReport();
			} 
		} 
	}


	protected void KillMales (int number)
	{
		for (int i = 0; i < number; i++) {
			GameObject male = Males[Random.Range(0, Males.Count)];
			male.GetComponent<MaleHealth>().HandleDeath();
		}
	}

	protected virtual void CreateSelfPlayer (bool startOfLevel)
	{
		AddRandomImmunities (CurrentPlayer);
		Herms.Add (CurrentPlayer);
		CameraController.instance.Target = CurrentPlayer.transform;
	}
	
	protected void CapOrganismNumbers ()
	{
		if (Herms.Count > param.maxHerms)
			KillHerms (Herms.Count - param.maxHerms);
		if (Males.Count > param.maxMales)
			KillMales (Males.Count - param.maxMales);
		if (FoodOrganisms.Count > param.maxFoodOrganisms)
			KillOrganisms (FoodOrganisms, FoodOrganisms.Count - param.maxFoodOrganisms);
		if (Predators.Count > param.maxPredators)
			KillOrganisms (Predators, Predators.Count - param.maxPredators);
		foreach (GameObject template in pathogenTemplates) {
			PathogenType type = template.GetComponent<Pathogen>().type;
			if (Pathogens[type].Count > param.maxPathogens)
				KillOrganisms (Pathogens[type], Pathogens[type].Count - param.maxPathogens);
		}
	}
	
	protected void RespawnOrganisms (SpawnDelegate spawner, int min, List<GameObject> organisms)
	{
		if (organisms.Count >= min)
			return;
		for (int i = 0; i < min - organisms.Count; i++)
			spawner (false);
	}

	protected void RespawnPathogens (GameObject template)
	{
		PathogenType type = template.GetComponent<Pathogen> ().type;
		if (Pathogens[type].Count >= param.minPathogens)
			return;
		for (int i = 0; i < param.minPathogens - Pathogens[type].Count; i++)
			SpawnPathogen (template, false);
	}

	protected void SpawnOrganisms (SpawnDelegate spawner, int number)
	{
		for (int i = 0; i < number; i++)
			spawner (true);
	}

	protected void SpawnPathogens ()
	{
		foreach (GameObject template in pathogenTemplates) {
			Pathogens.Add (template.GetComponent<Pathogen>().type, new List<GameObject>());
			for (int i = 0; i < param.initialPathogenCount; i++)
				SpawnPathogen (template, true);
		}
	}

	protected void Respawn ()
	{
		RespawnOrganisms (SpawnHerm, param.minHerms, Herms);
		RespawnOrganisms (SpawnMale, param.minMales, Males);
		RespawnOrganisms (SpawnFoodOrganism, param.minFoodOrganisms, FoodOrganisms);
		RespawnOrganisms (SpawnPredator, param.minPredators, Predators);
		foreach (GameObject template in pathogenTemplates)
			RespawnPathogens (template);
	}

	protected GameObject GetRandomPathogenTemplate ()
	{
		return pathogenTemplates[Random.Range (0, pathogenTemplates.Length)];
	}

	protected void KillOrganisms (List<GameObject> organisms, int number)
	{
		for (int i = 0; i < number; i++)
			KillOrganism (organisms);
	}
	
	protected virtual void CreateFish ()
	{
		CreateSelfPlayer (true);
		SpawnOrganisms (SpawnHerm, param.initialNpcHermCount);
		SpawnOrganisms (SpawnMale, param.initialMaleCount);
		// wait until other fish have joined until calculating scores
		// on multiplayer but override to calculate scores now for single player
	}

	protected GameObject GetSwitchFish (int playerIndex)
	{
		List<GameObject> switchFishes = new List<GameObject> ();
		Herms.ForEach (herm => {
			if (herm != null &&
				herm.GetComponent<Fish>().PlayerIndexes != null &&
				herm.GetComponent<Fish>().PlayerIndexes.Contains(playerIndex) &&
			    herm.GetComponent<AiHerm>() != null)//not a player
				switchFishes.Add (herm.gameObject);
		});
		if (switchFishes.Count == 0)
			return null;
		return switchFishes[Random.Range(0, switchFishes.Count)];
	}
	
	public override Stats GetStats ()
	{
		int countOfFishWithPlayerIndex = GetCountOfFishWithPlayerIndex ();
		return new Stats (countOfFishWithPlayerIndex.ToString (),
			(Herms.Count + Males.Count).ToString (),
			Score.ToString (),
			((int)timer) + " seconds",
			Format (Selfs, Crosses),
			Format (Crosses, Selfs));
	}

	protected abstract int  GetCountOfFishWithPlayerIndex ();

	protected Quaternion GetRandomRotation()
	{
		return Quaternion.Euler (new Vector3(0f, 0f, Random.Range (0, 360)));
	}

}

