using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImmPanelController : MonoBehaviour
{

	GameObject trichodinid;
	GameObject bacillus;
	GameObject acanthocephalan;
	GameObject virus;
	GameObject nematode;
	Dictionary<PathogenType, GameObject> immGameObjects;

	void Awake ()
	{
		trichodinid = transform.Find ("Trichodinid").gameObject;
		bacillus = transform.Find ("Bacillus").gameObject;
		acanthocephalan = transform.Find ("Acanthocephalan").gameObject;
		virus = transform.Find ("Virus").gameObject;
		nematode = transform.Find ("Nematode").gameObject;
		immGameObjects = new Dictionary<PathogenType, GameObject> () { 
			{PathogenType.Trichodinid, trichodinid},
			{PathogenType.Bacillus, bacillus},
			{PathogenType.Acanthocephalan, acanthocephalan},
			{PathogenType.Virus, virus},
			{PathogenType.Nematode, nematode}
		};
	}

	public void ShowImmunities (List<PathogenType> immunities)
	{
		HideAll ();
		foreach (PathogenType imm in immunities) {
			GameObject go = null;
			immGameObjects.TryGetValue (imm, out go);
			if (go != null)
				go.SetActive (true);
		}
	}

	void HideAll ()
	{
		foreach (GameObject go in immGameObjects.Values) {
			if (go != null)
				go.SetActive (false);
		}
	}

}
