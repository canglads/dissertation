using UnityEngine;
using System.Collections;

public class AiGrowth : FishGrowth
{

	//TODO in a future version this may be varied for food abundance
	public float maxEatTime = 8f;
	public int eatValue = 10;
	float timeToEat;

	void Start ()
	{
		timeToEat = Random.Range(0, maxEatTime);
	}

	void Update ()
	{
		if (FoodCounter == 0)
			return;
		if (timeToEat < 0f) {
			Eat (eatValue);
			timeToEat = Random.Range(0, maxEatTime);
		}
		timeToEat -= Time.deltaTime;
	}

}
