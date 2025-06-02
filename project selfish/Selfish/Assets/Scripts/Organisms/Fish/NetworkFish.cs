using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NetworkFish : Fish {

	protected override void Awake ()
	{
		base.Awake ();
		Movement = GetComponent<NetworkMovement> ();
	}

}
