using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerReproduction : HermReproduction
{
	Button selfButton;
	Button crossButton;

	protected override void Awake ()
	{
		base.Awake ();
		selfButton = UIController.instance.selfButton;
		crossButton = UIController.instance.crossButton;
	}

	protected override void Update ()
	{
		if (!mature)
			return;
		if (latent) {
			if (latencyTimer < 0f) {
				latent = false;
				selfButton.interactable = true;
			} else
				latencyTimer -= Time.deltaTime;
		} else {
			if (nearbyMales.Count != 0) {
				crossButton.interactable = true;
				UIController.instance.ShowMaleImm (nearbyMales [0]);
				if (Input.GetButton ("Cross"))
					Cross ();
			}
			if (Input.GetButton ("Self"))
				Self ();
		}
	}

	protected override void Reproduce ()
	{
		// generic Reproduce method is not called by our overridden Update but must be implemented
		// inelegant but caused by the multiple inheritance issue in the design
	}

	public override void MakeMature ()
	{
		mature = true;
		UIController.instance.HandlePlayerMature ();
	}

	public override void Self ()
	{
		base.Self ();
		GameController.instance.sc.Selfs++;
		SoundController.instance.PlaySingle (SoundController.instance.reproduceAudio);
	}

	public override void Cross ()
	{
		base.Cross ();
		GameController.instance.sc.Crosses++;
		SoundController.instance.PlaySingle (SoundController.instance.reproduceAudio);
	}

	public override void HandleNearMale (GameObject male)
	{
		base.HandleNearMale (male);
		if (mature && !latent) {
			crossButton.interactable = true;
			UIController.instance.ShowMaleImm (male);
		}
	}

	public override void HandleNotNearMale (GameObject male)
	{
		base.HandleNotNearMale (male);
		if (nearbyMales.Count == 0) {
			crossButton.interactable = false;
			UIController.instance.HideMaleImmPanel ();
		}
	}

	protected override void MakeLatent ()
	{
		base.MakeLatent ();
		selfButton.interactable = false;
		crossButton.interactable = false;
	}

	public void ShowMatureUIComponents ()
	{
		UIController.instance.HideMaleImmPanel ();
		selfButton.gameObject.SetActive (true);
		crossButton.gameObject.SetActive (true);
		if (latent) {
			selfButton.interactable = false;
			crossButton.interactable = false;
		} else {
			selfButton.interactable = true;
			if (nearbyMales.Count == 0) {
				crossButton.interactable = false;
			} else {
				crossButton.interactable = true;
				UIController.instance.ShowMaleImm (nearbyMales [0]);
			}
		}
	}
}
