using UnityEngine;
using System.Collections;

public class AiHermReproduction : HermReproduction {

	protected override void Reproduce ()
	{
		Self ();
	}

	protected override void Update ()
	{
		if (!mature)
			return;
		base.Update ();
	}

}
