using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImmuneSystem : MonoBehaviour {

	public List<PathogenType> Immunities { get; set; }

	void Awake ()
	{
		Immunities = new List<PathogenType> ();
	}

}
