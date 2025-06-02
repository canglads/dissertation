using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FishReproduction : OrganismReproduction {

	public bool mature;

	public virtual void MakeMature () {
		mature = true;
	}

}
