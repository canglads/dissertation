using UnityEngine;
using System.Collections;

public class PlayerGrowth : FishGrowth {

	public override void Eat (int value)
	{
		base.Eat (value);
		SoundController.instance.PlaySingle (SoundController.instance.eatAudio);
		UIController.instance.UpdateFood (FoodCounter);
	}

}
