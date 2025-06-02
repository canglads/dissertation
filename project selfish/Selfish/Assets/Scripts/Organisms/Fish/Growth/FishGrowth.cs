using UnityEngine;
using System.Collections;

public abstract class FishGrowth : MonoBehaviour {

	public int FoodCounter { get; set; }
	public float linearScaleFactor = 1.008f;

	protected int foodToMature;

	protected virtual void Awake ()
	{
		// take params from SceneController so can be edited in
		// make-your-own-level
		foodToMature = ParameterController.instance.foodToMature;
		FoodCounter = foodToMature;
	}

	public virtual void Eat (int value)
	{
		FoodCounter = FoodCounter - value < 0 ? 0 : FoodCounter - value;
		if (FoodCounter == 0)
			gameObject.GetComponent<FishReproduction> ().MakeMature ();
		else
			Grow (value);
	}

	public void Grow(int value) {
		gameObject.transform.localScale *= Mathf.Pow(linearScaleFactor, value);
	}

}
