using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public abstract class HermReproduction : FishReproduction
{
	[Serializable]
	public class Parameters 
	{
		public int maxOffspringCross = 3;
		public int minOffspringCross = 2;
		public float maleRatioSelf = 0.1f;
		public float maleRatioCross = 0.5f;
		public int maxOffspring = 4;
		public int minOffspring = 3;
		public float minLatencyDuration = 8f;
		public float maxLatencyDuration = 20f;
	}

	public List<GameObject> nearbyMales;

	protected Parameters parameters;

	protected override void Awake ()
	{
		// values initialized from ParameterController to allow for
		// build-your-own level fx
		parameters = ParameterController.instance.hermReproductionParameters;
		maxOffspring = parameters.maxOffspring;
		minOffspring = parameters.minOffspring;
		minLatency = parameters.minLatencyDuration;
		maxLatency = parameters.maxLatencyDuration; // this is for selfing
		nearbyMales = new List<GameObject> ();
		latencyTimer = Random.Range(minLatency, maxLatency);
	}

	public virtual void Self ()
	{
		MakeLatent ();
		int numberOfOffspring = Random.Range (minOffspring, maxOffspring);
		GameController.instance.sc.CreateOffspring (numberOfOffspring, GetComponent<Fish>().PlayerIndexes, GetComponent<ImmuneSystem>().Immunities, true, transform.position);
	}
	
	public virtual void Cross ()
	{
		DiscountDiedMales ();
		// may not have cleaned list since a network destroy, so check male null as well
		if (nearbyMales.Count == 0 || nearbyMales [0] == null)	
			return;
		GameObject male = nearbyMales [0];
		MakeLatent ();
		int numberOfOffspring = Random.Range (parameters.minOffspringCross, parameters.maxOffspringCross);
		GameController.instance.sc.CreateOffspring (numberOfOffspring, GetSharedPlayerIndexes(male), GetSharedImmunities(male), false, transform.position);
	}

	protected void DiscountDiedMales ()
	{
		nearbyMales.RemoveAll (item => item == null);
	}

	protected List<PathogenType> GetSharedImmunities (GameObject male)
	{
		List<PathogenType> immunities = new List<PathogenType> ();
		foreach (PathogenType imm in GetComponent<ImmuneSystem>().Immunities)
			immunities.Add (imm);
		foreach (PathogenType imm in male.GetComponent<ImmuneSystem>().Immunities) {
			if (!immunities.Contains(imm))
				immunities.Add(imm);
		}
		return immunities;
	}

	protected List<int> GetSharedPlayerIndexes (GameObject male)
	{
		List<int> playerIndexes = new List<int> ();
		foreach (int index in GetComponent<Fish>().PlayerIndexes)
			playerIndexes.Add (index);
		foreach (int index in male.GetComponent<Fish>().PlayerIndexes) {
			if (!playerIndexes.Contains(index))
				playerIndexes.Add(index);
		}
		return playerIndexes;
	}

	public virtual void HandleNearMale (GameObject male)
	{
		nearbyMales.Add (male);
	}
	
	public virtual void HandleNotNearMale (GameObject male)
	{
		nearbyMales.Remove (male);
	}

	protected virtual void MakeLatent ()
	{
		latencyTimer = Random.Range (minLatency, maxLatency);
		latent = true;
	}

}
