using UnityEngine;
using System.Collections;

public abstract class OrganismReproduction : MonoBehaviour {

	public float minLatency = 5f;
	public float maxLatency = 5f;
	public int minOffspring = 2;
	public int maxOffspring = 6;

	protected float latencyTimer;
	protected bool latent;
	
	protected virtual void Awake ()
	{
		latent = true;
		latencyTimer = Random.Range (minLatency, maxLatency);
	}
	
	protected abstract void Reproduce ();

	protected virtual void Update ()
	{
		if (latent) {
			if (latencyTimer < 0f)
				latent = false;
			else {
				latencyTimer -= Time.deltaTime;
			}
		} else {
			Reproduce ();
			latencyTimer = Random.Range (minLatency, maxLatency);
			latent = true;
		}
	}

	protected Quaternion GetRandomRotation()
	{
		return Quaternion.Euler (new Vector3(0f,0f,Random.Range (0, 360)));
	}

}
