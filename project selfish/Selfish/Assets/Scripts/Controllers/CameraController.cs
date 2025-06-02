using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController instance;

	public Transform Target { get; set; }

	public float smoothing = 5f;
	public float slerpSpeed = 5f;
	
	Vector3 offset = new Vector3 (0f, 0f, -10f);

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}
	
	void LateUpdate () {
		if (Target == null)
			return;
		Vector3 targetCamPos = Target.position + offset;
		transform.position = Vector3.Lerp (transform.position, 
		                                   targetCamPos, smoothing + Time.deltaTime);
		Quaternion r = Target.rotation * Quaternion.Euler(0f, 0f, -90f);
		transform.rotation = Quaternion.Slerp(transform.rotation, r, slerpSpeed * Time.deltaTime);
	}
	
}
