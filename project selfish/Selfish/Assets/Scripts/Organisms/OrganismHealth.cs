using UnityEngine;
using System.Collections;

public abstract class OrganismHealth : MonoBehaviour {

	public float minLifetime;
	public float maxLifetime;
	public float timeToDeath;

	protected virtual void Awake ()
	{
		timeToDeath = Random.Range (minLifetime, maxLifetime);
	}

	protected abstract void Update ();

}
