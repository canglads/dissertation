using UnityEngine;
using System.Collections;

public class ParameterController : MonoBehaviour {

	public static ParameterController instance;

	public int initialNpcHermCount = 6;
	public int initialMaleCount = 2;
	public int maxInitialImm = 1;//min is always 1
	public int initialFoodCount = 20;
	public int initialPathogenCount = 0;
	public int initialPredatorCount = 0;
	public bool displayImmPanels = true;
	public float levelLength = 300f;// in seconds
	public int maxHerms = 20;
	public int maxMales = 10;
	public int maxPathogens = 50;
	public int maxFoodOrganisms = 50;
	public int maxPredators = 5;
	public int minHerms = 2;
	public int minMales = 1;
	public int minPathogens = 0;
	public int minFoodOrganisms = 20;
	public int minPredators = 0;
	public HermReproduction.Parameters hermReproductionParameters;
	public int foodToMature = 50;
	public float minFishLifetime = 30f;
	public float maxFishLifetime = 30f;
	public float infectedFishLifetime = 5f;
	public int Temp = 22;

	void Awake ()
	{
		instance = this;
	}

}
