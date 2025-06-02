using UnityEngine;
using System.Collections;

public class TouchPanelController : MonoBehaviour
{

	public float thresholdDistanceForSpurt = 3f;
	//bool pointerHold;//uncomment for mouse testing

	void Update ()
	{
		if (Input.touchCount != 0) {
		//if (pointerHold) {
		//	MouseTest();//uncomment for mouse testing
			HandleTouch ();
		}
	}

	void HandleTouch ()
	{
		if (Input.touchCount == 0 || GameController.instance.sc.CurrentPlayer == null)
			return;
		Touch touch = Input.touches [0];//no multi-touch
		PlayerMovement playerMovement = GameController.instance.sc.CurrentPlayer
			.GetComponent<PlayerMovement> ();
		if (touch.phase == TouchPhase.Ended) {
			playerMovement.TouchSpurt = false;
			return;
		}
		if (TouchFarFromFish (Camera.main.ScreenToWorldPoint (touch.position)))
			playerMovement.TouchSpurt = true;
		playerMovement.TurnToward (Camera.main.ScreenToWorldPoint (touch.position));
	}

	bool TouchFarFromFish (Vector3 pointerPosition)
	{
		Vector3 playerPosition = GameController.instance.sc.CurrentPlayer.transform.position;
		return Vector2.Distance (pointerPosition, playerPosition) > thresholdDistanceForSpurt;
	}
	
	//uncomment for mouse testing
	//(fired from mouse and touch)
	/*public void HandlePointerDown ()
	{
		pointerHold = true;
	}*/
	
	//uncomment for mouse testing
	//(fired from mouse and touch)
	/*public void HandlePointerUp ()
	{
		pointerHold = false;
		GameController.instance.sc.CurrentPlayer
			.GetComponent<PlayerMovement> ().TouchSpurt = false;
	}*/
	
	//for mouse testing only - not used in production builds
	/*void MouseTest ()
	{
		PlayerMovement playerMovement = GameController.instance.sc.CurrentPlayer
				.GetComponent<PlayerMovement> ();
		if (TouchFarFromFish (Camera.main.ScreenToWorldPoint (Input.mousePosition)))
			playerMovement.TouchSpurt = true;
		playerMovement.TurnToward (Camera.main.ScreenToWorldPoint (Input.mousePosition));
	}*/

}
