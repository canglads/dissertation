using UnityEngine;
using System.Collections;
using System;

// This and its subclasses control level specific behaviour (primarily used for message display in the Intro levels)
// Not to be used with multiplayer unless altered
public class LevelController : MonoBehaviour {

	[TextArea(3,10)]
	public string[] initialMessages;
	[TextArea(3,10)]
	public string[] finalMessages;

	protected SinglePlayerSceneController sc;

	void Awake ()
	{
		sc = GetComponent<SinglePlayerSceneController> ();
	}

	// called by Start in SceneController -> ensures SceneController is ready
	public void Init ()
	{
		if (initialMessages.Length != 0)
			ShowMessage (initialMessages, 0);
	}

	protected void ShowMessage (string[] messages, int index) {
		if (index == messages.Length) //base case
			return;
		sc.ShowMessage (messages [index], delegate {
			ShowMessage (messages, ++index);
		});
	}

	public void HandleSurvived ()
	{
		if (finalMessages.Length != 0)
			ShowMessage (finalMessages, 0);
	}

}
