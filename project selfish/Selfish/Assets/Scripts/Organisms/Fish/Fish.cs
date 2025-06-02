using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Fish : MonoBehaviour {

	public List<int> PlayerIndexes { get; set; }
	public OrganismMovement Movement { get; protected set; }

	protected virtual void Awake ()
	{
		PlayerIndexes = new List<int> ();
	}

}
