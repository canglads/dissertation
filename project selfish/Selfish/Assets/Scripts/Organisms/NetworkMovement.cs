using UnityEngine;
using System.Collections;

public class NetworkMovement : OrganismMovement {

	// standard move forward, no turns, called much more often than network data received
	// smoother than network data alone
	protected virtual void FixedUpdate ()
	{
		Vector3 movement = transform.right;
		movement *= speed * Time.deltaTime;
		transform.position += movement;
	}

}
