using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MaleReproduction : FishReproduction {
	
	// Required in case player is following fish waiting for maturity
	// thus, needs to use OnTriggerStay rather than OnTriggerEnter
	protected bool newlyMature;

	// keep a list of herms so can inform them when die
	List<HermReproduction> nearbyHerms;

	protected override void Awake ()
	{
		base.Awake ();
		nearbyHerms = new List<HermReproduction>();
	}

	protected override void Update ()
	{
		//override to disable generative behaviour
	}

	protected override void Reproduce()
	{
		//again, not required but must be implemented - inelegant but caused by the multiple inheritance issue in the design
	}

	public override void MakeMature () 
	{
		mature = true;
		newlyMature = true;
	}
	
	protected abstract void OnTriggerEnter2D (Collider2D other);

	protected abstract void OnTriggerStay2D (Collider2D other);
	
	protected abstract void OnTriggerExit2D (Collider2D other);

	public void InformHermsOfDeath ()
	{
		nearbyHerms.ForEach (herm => {
			if (herm != null) // herm may have died
				herm.HandleNotNearMale(gameObject);
		});
	}

	public virtual void HandleNearHerm (GameObject herm)
	{
		nearbyHerms.Add (herm.GetComponent<HermReproduction>());
	}
	
	public virtual void HandleNotNearHerm (GameObject herm)
	{
		nearbyHerms.Remove (herm.GetComponent<HermReproduction>());
	}

}
