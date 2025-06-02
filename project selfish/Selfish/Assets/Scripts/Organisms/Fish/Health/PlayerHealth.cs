using UnityEngine;
using System.Collections;

public class PlayerHealth : FishHealth
{

	public string infectedWarning = "INFECTED";
	public string dyingWarning = "DYING";
	public float dyingWarningNoticePeriod = 4f;

	protected bool warnedDying;

	bool registeredDeath;

	protected override void Awake() {
		base.Awake ();
		// actually we always want the maxLifetime for players for gameplay reasons
		timeToDeath = maxLifetime;
	}

	protected override void Update ()
	{
		if (registeredDeath)
			return;//awaiting new fish assignment over network, perhaps
		if (timeToDeath < 0f) {
			registeredDeath = true;
			GetComponent<NonNetworkFish> ().RemoveFromSceneController();
			GameController.instance.sc.HandleCurrentPlayerFishDead ();
		}
		if (!warnedDying && timeToDeath < dyingWarningNoticePeriod)
			HandleDying ();
		timeToDeath -= Time.deltaTime;
		UIController.instance.DisplayRemainingLifetime (timeToDeath);
	}

	void HandleDying ()
	{
		warnedDying = true;
		UIController.instance.ShowWarningText (dyingWarning);
	}

	public override bool MakeInfected (PathogenType pathogenType)
	{
		if (!base.MakeInfected (pathogenType))
			return false;
		UIController.instance.StartDamageImageFlashing ();
		UIController.instance.ShowWarningText (infectedWarning);
		SoundController.instance.PlaySingle (SoundController.instance.infectedAudio);
		return true;
	}

}
