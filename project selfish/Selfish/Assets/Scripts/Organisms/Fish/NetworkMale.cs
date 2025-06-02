using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMale : NetworkFish {

	public NetworkMaleReproduction Reproduction { get; private set; }
	public ImmuneSystem ImmuneSystem { get; private set; }

	protected override void Awake ()
	{
		base.Awake ();
		ImmuneSystem = GetComponent<ImmuneSystem> ();
		Reproduction = GetComponent<NetworkMaleReproduction> ();
	}

}
