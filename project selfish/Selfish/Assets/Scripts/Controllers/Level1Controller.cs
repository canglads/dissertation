using UnityEngine;
using System.Collections;

public class Level1Controller : LevelController {

	public float dyingMessageNoticePeriod = 5f;
	[TextArea(3,10)]
	public string[] matureMessages = new string[] {
		"Well done!\nYou are mature",
		"As a mangrove killifish,\nyou are self-fertile",
		"Just hit self\nto reproduce"
	};
	[TextArea(3,10)]
	public string[] dyingMessages = new string[] {
		"Your fish has reached\nthe end of its lifetime\nbut don't panic",
		"Because you've reproduced\nyou will now control\none of the offspring"
	};

	bool matureMessagesShown;
	bool dyingMessagesShown;

	void Update () {
		if (!matureMessagesShown && sc.Herms.Count == 1 &&
		    sc.CurrentPlayer.GetComponent<PlayerReproduction> ().mature) {
			matureMessagesShown = true;	
			ShowMessage(matureMessages, 0);
		}
		if (!dyingMessagesShown && sc.Herms.Count > 1 &&
			sc.CurrentPlayer.GetComponent<PlayerHealth> ().timeToDeath < dyingMessageNoticePeriod) {
			dyingMessagesShown = true;
			ShowMessage(dyingMessages, 0);
		}
	}

}
