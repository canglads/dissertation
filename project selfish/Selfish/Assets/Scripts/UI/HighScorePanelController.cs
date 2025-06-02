using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScorePanelController : MonoBehaviour {

	Text scoreText;
	InputField nameInputField;
	Button submitButton;
	Text messageText;
	int score;

	public string error = "failed to submit score";
	public string submitting = "submitting...";
	public string submitted = "Submitted!";
	public float submittedDuration = 1f;

	void Awake ()
	{
		Transform panel = transform.Find ("Panel");
		scoreText = panel.Find ("ScoreText").GetComponent<Text> ();
		nameInputField = panel.Find ("NameInputField").GetComponent<InputField> ();
		submitButton = panel.Find ("SubmitButton").GetComponent<Button> ();
		messageText = panel.Find ("MessageText").GetComponent<Text>();
	}

	public void Show (int score)
	{
		gameObject.SetActive (true);
		this.score = score;
		scoreText.text = score.ToString ();
	}

	public void HandleBack ()
	{
		gameObject.SetActive (false);
	}

	public void HandleSubmit ()
	{
		string input = nameInputField.text;
		if (string.IsNullOrEmpty (input))
			return;
		submitButton.interactable = false;
		messageText.text = submitting;
		GameController.instance.scoresWebServiceClient.Submit (
			new Score (input, score), SubmitCallback);                            
	}

	void SubmitCallback (bool successful)
	{
		if (!successful) {
			submitButton.interactable = true;
			messageText.text = error;
			return;
		}
		messageText.text = submitted;
		StartCoroutine (WaitForDurationThenClose());
	}

	IEnumerator WaitForDurationThenClose ()
	{
		yield return new WaitForSeconds (submittedDuration);
		gameObject.SetActive (false);
	}

}
