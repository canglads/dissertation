using UnityEngine;
using System.Collections;

public class AiHealth : FishHealth
{
	
	protected override void Update ()
	{
		if (timeToDeath < 0f)
			HandleDeath ();
		timeToDeath -= Time.deltaTime;
	}

	public virtual void HandleDeath ()
	{
		GetComponent<AiFish> ().RemoveFromSceneController();
		AiSceneController sc = (AiSceneController) GameController.instance.sc;
		sc.RecalculateScores ();
		sc.DestroyOrganism (gameObject);
	}

}