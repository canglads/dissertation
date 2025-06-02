using UnityEngine;
using System.Collections;

public class MaleHealth : AiHealth {

	public override void HandleDeath ()
	{
		GetComponent<MaleReproduction> ().InformHermsOfDeath ();
		base.HandleDeath ();
	}

}
