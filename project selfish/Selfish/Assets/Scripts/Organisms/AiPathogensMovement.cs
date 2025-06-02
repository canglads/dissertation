using UnityEngine;
using System.Collections;

public class AiPathogensMovement : OrganismMovement {

	public float turnCoefficient = 9f;
	public float turnSmoothing = 15f; 

	public float turnFrequency = 0.5f;

	protected virtual void FixedUpdate ()
	{
		if (Random.value < turnFrequency) {
			float h = Random.Range (0, 2) * 2 - 1;
			Rotate (h);
		}
		Move ();
	}

	protected void Rotate (float rotation) {
		Quaternion targetRotation = transform.rotation * Quaternion.Euler (0f, 0f, -rotation * turnCoefficient);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
	}

	protected virtual void Move ()
	{
		Vector3 movement = transform.right;
		movement *= speed/4 * Time.deltaTime;
		transform.position += movement;
	}
}
