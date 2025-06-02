using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FishHealth : OrganismHealth {

	protected bool infected;
	protected ParameterController param;
	protected float infectedLifetime;

	protected override void Awake() {
		//abbreviation only
		param = ParameterController.instance;
		// for build-your-own-level
		minLifetime = param.minFishLifetime;
		maxLifetime = param.maxFishLifetime;
		infectedLifetime = param.infectedFishLifetime;
		timeToDeath = Random.Range (minLifetime, maxLifetime);
	}

	//returns true if successfully infected, false if the fish is immune
	public virtual bool MakeInfected (PathogenType pathogenType) 
	{
		if (GetComponent<ImmuneSystem>() != null && GetComponent<ImmuneSystem>().Immunities.Contains (pathogenType))
			return false;
		infected = true;
		timeToDeath = timeToDeath < infectedLifetime ? timeToDeath : infectedLifetime;
		return true;
	}

	public void GetEaten ()
	{
		// just set the timeToDeath and everything should be taken care of
		timeToDeath = 0;
	}

}
