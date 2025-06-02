using UnityEngine;
using System.Collections;

public abstract class AiFish : NonNetworkFish {

	protected override void Awake ()
	{
		base.Awake();
		Movement = GetComponent<AiOrganismMovement>();
		Growth = GetComponent<AiGrowth>();
		Health = GetComponent<AiHealth>();
	}

}
