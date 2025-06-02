using UnityEngine;
using System.Collections;

public class PlayerMovement : AiOrganismMovement
{
	// note that while it is possible to change this is in the prefabs,
	// the change will also need to made here in order to affect
	// switch fish
	public float spurtCoefficient = 2f;
	public bool TouchSpurt { get; set; }

	bool spurt;

	protected override void FixedUpdate ()
	{
		float h = Input.GetAxis ("Horizontal");
		spurt = Input.GetButton ("Spurt") || TouchSpurt;
		if (h != 0f) {
			Rotate (h);
		}
		Move (); 
	}
	
	protected override void Move ()
	{
		int spurtInt = spurt ? 1 : 0;
		Vector3 movement = transform.right;
		movement *= speed * (spurtInt * spurtCoefficient + 1) * Time.deltaTime;
		transform.position += movement;
	}

	public void TurnToward (Vector3 target)
	{
		Vector3 targetVector = target - transform.position;
		Vector3 movementVector = transform.rotation * new Vector3(1, 1, 0);
		Vector3 cross = Vector3.Cross (movementVector, targetVector);
		Rotate (cross.z < 0 ? 1 : -1);
	}

}
