using UnityEngine;
using System.Collections;

public class MultiplayerOverrideUIController : UIController {

	public override void ShowOwnImmPanel ()
	{
		ownImmPanel.SetActive (true);
		ownImmPanel.GetComponent<ImmPanelController> ().ShowImmunities (
			MultiplayerLevelController.instance.CurrentPlayer.GetComponent<ImmuneSystem>().Immunities
			);
	}

}
