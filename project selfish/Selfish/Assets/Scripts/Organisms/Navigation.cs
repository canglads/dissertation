using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour
{

	void OnTriggerEnter2D (Collider2D other)
	{
		RotateToOrigin (other.gameObject.transform);
	}

	void OnTriggerStay2D (Collider2D other)
	{
		RotateToOrigin (other.gameObject.transform);
	}

	void RotateToOrigin (Transform other)
	{
		Vector3 v = -other.position.normalized;
		float rotation = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
		other.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
	}

}
