using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CustomPanelController : MonoBehaviour {

	public static CustomPanelController instance;

	public InputField minHermCountField;
	public InputField maxHermCountField;
	public InputField minMaleCountField;
	public InputField maxMaleCountField;
	public InputField minFoodCountField;
	public InputField maxFoodCountField;
	public InputField minPathogenCountField;
	public InputField maxPathogenCountField;
	public InputField minPredatorCountField;
	public InputField maxPredatorCountField;
	public InputField maxImmunitiesField;
	public InputField levelLengthField;
	public InputField maleRatioSelfField;
	public InputField maleRatioCrossField;
	public InputField foodToMatureField;
	public InputField minFishLifetimeField;
	public InputField maxFishLifetimeField;
	public InputField pathogenDiversityField;
	public InputField temperatureField;

	public int maxHermCount = 20;
	public int maxMaleCount = 20;
	public int maxMaxImmunities = 5;
	public int maxFoodCount = 20;
	public int maxPathogenCount = 20;
	public int maxPredatorCount = 20;
	public int maxLevelLength = 6000;
	public int minFoodToMature = 10;
	public int maxFoodToMature = 1000;
	public int maxFishLifetime = 1000;
	public int maxPathogenDiversity = 5;
	public int maxTemperature = 38;

	// for prefs
	const string MinHermCountString = "MinHermCount";
	const string MaxHermCountString = "MaxHermCount";
	const string MinMaleCountString = "MinMaleCount";
	const string MaxMaleCountString = "MaxMaleCount";
	const string MinFoodCountString = "MinFoodCount";
	const string MaxFoodCountString = "MaxFoodCount";
	const string MinPathogenCountString = "MinPathogenCount";
	const string MaxPathogenCountString = "MaxPathogenCount";
	const string MinPredatorCountString = "MinPredatorCount";
	const string MaxPredatorCountString = "MaxPredatorCount";
	const string MaxImmunitiesString = "MaxImmunities";
	const string LevelLengthString = "LevelLength";
	const string MaleRatioSelfString = "MaleRatioSelf";
	const string MaleRatioCrossString = "MaleRatioCross";
	const string FoodToMatureString = "FoodToMature";
	const string MinFishLifetimeString = "MinFishLifetime";
	const string MaxFishLifetimeString = "MaxFishLifetime";
	const string PathogenDiversityString = "PathogenDiversity";
	const string TemperatureString = "Temperature";

	CustomSceneController sc;
	ParameterController param;

	public void HandlePlay ()
	{
		UIController.instance.PlayButtonPressAudio ();
		ProcessInput ();
		sc.Spawn ();
		gameObject.SetActive (false);
	}

	public void HandleExit ()
	{
		UIController.instance.PlayButtonPressAudio ();
		GameController.instance.EndGame ();
	}

	public void Init ()
	{
		sc = (CustomSceneController) GameController.instance.sc;
		param = sc.publicParam;
		ReadInPrefs ();
	}

	void Awake () 
	{
		instance = this;
	}


	void ProcessInput ()
	{
		// numeric input enforced in the UI, but check for empty strings and out of range
		// (then ignore them and use defaults)
		TrySetHermCounts ();
		TrySetMinAndMax (minMaleCountField, maxMaleCountField, out param.minMales, 
		                 out param.initialMaleCount, out param.maxMales, param.minMales, 
		                 param.maxMales, MinMaleCountString, MaxMaleCountString, maxMaleCount);
		TrySetMinAndMax (minFoodCountField, maxFoodCountField, out param.minFoodOrganisms, 
		                 out param.initialFoodCount, out param.maxFoodOrganisms, param.minFoodOrganisms, 
		                 param.maxFoodOrganisms, MinFoodCountString, MaxFoodCountString, maxFoodCount);
		TrySetMinAndMax (minPathogenCountField, maxPathogenCountField, out param.minPathogens, 
		                 out param.initialPathogenCount, out param.maxPathogens, param.minPathogens, 
		                 param.maxPathogens, MinPathogenCountString, MaxPathogenCountString, maxPathogenCount);
		TrySetMinAndMax (minPredatorCountField, maxPredatorCountField, out param.minPredators, 
		                 out param.initialPredatorCount, out param.maxPredators, param.minPredators, 
		                 param.maxPredators, MinPredatorCountString, MaxPredatorCountString, maxPredatorCount);
		TrySetParamInt (maxImmunitiesField, out param.maxInitialImm, param.maxInitialImm, 
		                MaxImmunitiesString, maxMaxImmunities);
		TrySetParamFloat (levelLengthField, out param.levelLength, param.levelLength, 
		                  LevelLengthString, maxLevelLength, 1f);
		TrySetParamFloat (maleRatioSelfField, out param.hermReproductionParameters.maleRatioSelf,
		                  param.hermReproductionParameters.maleRatioSelf, MaleRatioSelfString, 
		                  1f);
		TrySetParamFloat (maleRatioCrossField, out param.hermReproductionParameters.maleRatioCross,
		                 param.hermReproductionParameters.maleRatioCross, MaleRatioCrossString, 
		                  1f);
		TrySetParamInt (foodToMatureField, out param.foodToMature, param.foodToMature, 
		                FoodToMatureString, maxFoodToMature, minFoodToMature);
		TrySetFishLifetimes ();
		TrySetPathogenDiversity ();
		TrySetParamInt (temperatureField, out param.Temp, param.Temp, TemperatureString, 
		                maxTemperature);
		ReproductionCurve (param.Temp);
	}

	void TrySetParamInt (InputField field, out int intParam, int defaultValue, 
	                     string prefsString, int max, int min = 0)
	{
		intParam = ParseInput (field, defaultValue, min, max);
		PlayerPrefs.SetInt (prefsString, intParam);
	}
		
	void TrySetParamFloat (InputField field, out float floatParam, float defaultValue, 
	                       string prefsString, float max, float min = 0f)
	{
		floatParam = ParseInput (field, defaultValue, min, max);
		PlayerPrefs.SetFloat (prefsString, floatParam);
	}

	// special case because minHerms = initialHermCount + 1
	void TrySetHermCounts ()
	{
		// this sets initial incorrectly
		TrySetMinAndMax (minHermCountField, maxHermCountField, out param.minHerms, 
		                 out param.initialNpcHermCount, out param.maxHerms, param.minHerms, 
		                 param.maxHerms, MinHermCountString, MaxHermCountString, maxHermCount, 1);
		// but we correct here
		param.initialNpcHermCount = param.minHerms - 1;
	}

	// special case because need to make changes in scene controller
	void TrySetPathogenDiversity ()
	{
		int diversity = ParseInput (pathogenDiversityField, sc.defaultPathogenDiversity, 1, maxPathogenDiversity);
		sc.SetPathogenDiversity (diversity);
		PlayerPrefs.SetInt (PathogenDiversityString, diversity);
	}

	// init param is set the same as min for the custom level (but not necessarily elsewhere)
	void TrySetMinAndMax (InputField minField, InputField maxField, out int minParam, out int initParam, 
	                      out int maxParam, int defaultMin, int defaultMax, string minPrefsString, 
	                      string maxPrefsString, int maxValid, int minValid = 0)
	{
		int min = ParseInput (minField, defaultMin, minValid, maxValid);
		int max = ParseInput (maxField, defaultMax, minValid, maxValid);
		if (min > max) {
			min = defaultMin;
			max = defaultMax;
		}
		minParam = min;
		initParam = min;
		PlayerPrefs.SetInt (minPrefsString, min);
		maxParam = max;
		PlayerPrefs.SetInt (maxPrefsString, max);
	}

	int ParseInput (InputField field, int defaultValue, int minValid, int maxValid)
	{
		return ValidationUtil.ParseInput (field.text, defaultValue, minValid, maxValid);
	}

	float ParseInput (InputField field, float defaultValue, float minValid, float maxValid)
	{
		return ValidationUtil.ParseInput (field.text, defaultValue, minValid, maxValid);
	}

	// separate method to allow checking max >= min with floats
	void TrySetFishLifetimes ()
	{
		float min = ParseInput (minFishLifetimeField, param.minFishLifetime, 1f, maxFishLifetime);
		float max = ParseInput (maxFishLifetimeField, param.maxFishLifetime, 1f, maxFishLifetime);
		if (min > max) {
			//min = param.minFishLifetime;
			//max = param.maxFishLifetime;
			float temp = min;
			min = max;
			max = temp;
			TrySetFishLifetimes ();
			return;
		}
		param.minFishLifetime = min;
		PlayerPrefs.SetFloat (MinFishLifetimeString, min);
		param.maxFishLifetime = max;
		PlayerPrefs.SetFloat (MaxFishLifetimeString, max);
	}

	void ReadInPrefs ()
	{
		ReadInPref (minHermCountField, MinHermCountString, param.initialNpcHermCount);
		ReadInPref (maxHermCountField, MaxHermCountString, param.maxHerms);
		ReadInPref (minMaleCountField, MinMaleCountString, param.initialMaleCount);
		ReadInPref (maxMaleCountField, MaxMaleCountString, param.maxMales);
		ReadInPref (minFoodCountField, MinFoodCountString, param.initialFoodCount);
		ReadInPref (maxFoodCountField, MaxFoodCountString, param.maxFoodOrganisms);
		ReadInPref (minPathogenCountField, MinPathogenCountString, param.initialPathogenCount);
		ReadInPref (maxPathogenCountField, MaxPathogenCountString, param.maxPathogens);
		ReadInPref (minPredatorCountField, MinPredatorCountString, param.initialPredatorCount);
		ReadInPref (maxPredatorCountField, MaxPredatorCountString, param.maxPredators);
		ReadInPref (maxImmunitiesField, MaxImmunitiesString, param.maxInitialImm);
		ReadInPref (levelLengthField, LevelLengthString, param.levelLength);
		ReadInPref (maleRatioSelfField, MaleRatioSelfString, param.hermReproductionParameters.maleRatioSelf);
		ReadInPref (maleRatioCrossField, MaleRatioCrossString, param.hermReproductionParameters.maleRatioCross);
		ReadInPref (foodToMatureField, FoodToMatureString, param.foodToMature);
		ReadInPref (minFishLifetimeField, MinFishLifetimeString, param.minFishLifetime);
		ReadInPref (maxFishLifetimeField, MaxFishLifetimeString, param.maxFishLifetime);
		ReadInPref (pathogenDiversityField, PathogenDiversityString, sc.defaultPathogenDiversity);
		ReadInPref (temperatureField, TemperatureString, param.Temp);
	}

	void ReadInPref (InputField field, string prefsString, int defaultValue)
	{
		field.text = PlayerPrefs.GetInt (prefsString, defaultValue).ToString();
	}

	void ReadInPref (InputField field, string prefsString, float defaultValue)
	{
		field.text = PlayerPrefs.GetFloat (prefsString, defaultValue).ToString ();
	}

	public void ReproductionCurve(int temp){
		if (temp <= 20){
			param.hermReproductionParameters.maleRatioSelf = 0.75f + (20 - temp) * 0.02f;
			param.hermReproductionParameters.maleRatioCross = 0.75f + (20 - temp) * 0.02f;
		}
		else if (temp > 20 && temp <= 25){
			param.hermReproductionParameters.maleRatioSelf = 0.2f + (25 - temp) * 0.1f;
			param.hermReproductionParameters.maleRatioCross = 0.2f + (25 - temp) * 0.1f;
		}
		else if (temp > 25 && temp <= 30){
			param.hermReproductionParameters.maleRatioSelf = 0.05f + (30 - temp) * 0.05f;
			param.hermReproductionParameters.maleRatioCross = 0.05f + (30 - temp) * 0.05f;
		}
		else {
			param.hermReproductionParameters.maleRatioSelf = 0.01f;
			param.hermReproductionParameters.maleRatioCross = 0.01f;
		}
	}

}
