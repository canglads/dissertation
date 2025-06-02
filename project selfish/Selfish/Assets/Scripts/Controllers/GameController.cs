using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{
	
	// singleton pattern (only one instance for all scenes)
	public static GameController instance;
	public string version;
	public int numberOfLevels = 11;//include start but exclude multiplayer and custom
	public int customLevelIndex = 12;
	public int[] scores;//scores[0] will always be 0 (start)
	// this is 1-based because "Start" is Level 0
	public int CurrentLevel { get; private set; }
	// this is the scene controller for either single or multiplayer (master or non-master) 
	public SceneController sc;
	// this is used for saving advanced level scores to an array
	// and loading the correct save game
	public int advancedLevelToSceneOffset = 6;
	public int numberOfSavedGames = 5;//changing this will necessitate changes to the SaveGamePanel and the LoadPanel

	public bool FirstLoad { get; set; }

	public ScoresWebServiceClient scoresWebServiceClient;

	public int LowestHighScore { get; set; }

	public bool LastLevel {
		get {
			return CurrentLevel == numberOfLevels - 1;
		}
	}

	public int TotalScore {
		get {
			int total = 0;
			for (int i = advancedLevelToSceneOffset; i < scores.Length; i++)
				total += scores [i];
			return total;
		}
	}

	public SavedGame[] SavedGames { get; private set; }

	public float SoundFxVolume {
		get {
			return soundFxVolume;
		}
		set {
			soundFxVolume = value;
			SoundController.instance.SoundFxVolume = soundFxVolume;
			PlayerPrefs.SetFloat (SoundFxPrefsString, soundFxVolume);
		}
	}

	public float MusicVolume {
		get {
			return musicVolume;
		}
		set {
			musicVolume = value;
			SoundController.instance.MusicVolume = musicVolume;
			PlayerPrefs.SetFloat (MusicPrefsString, musicVolume);
		}
	}

	static int FirstLevelIndex = 1;
	const string SoundFxPrefsString = "SoundFx";
	const string MusicPrefsString = "Music";
	const string SavedGamesFileName = "saved-games.dat";
	float soundFxVolume;
	float musicVolume;
	string savedGamesPath;
	BinaryFormatter bf;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);	
		}
		DontDestroyOnLoad (gameObject);
		FirstLoad = true;
		scores = new int[numberOfLevels];
		#if !(UNITY_WEBGL || UNITY_WEBPLAYER)
		bf = new BinaryFormatter ();
		savedGamesPath = Application.persistentDataPath + "/" + SavedGamesFileName;
		ReadInSavedGames ();
		#endif
	}

	void Start ()
	{
		ReadInSettings ();
		scoresWebServiceClient.Get (delegate (ScoresData data) {
			if (data == null) 
				// if no network prevent high score notifications
				LowestHighScore = Int32.MaxValue;
			else 
				LowestHighScore = GetLowestScore (data);
		});
	}

	public void EndGame ()
	{
		Time.timeScale = 1f;
		CurrentLevel = 0;
		scores = new int[numberOfLevels]; 
		Application.LoadLevel ("Start");
	}

	public void RecordScore (int score)
	{
		scores [CurrentLevel] = score;
	}

	public void NextLevel ()
	{
		Time.timeScale = 1f;
		Application.LoadLevel (++CurrentLevel);
		CSVManager.SetCurrentLevel (CurrentLevel);
	}

	
	public void LoadFirstIntroLevel ()
	{
		CurrentLevel = FirstLevelIndex;
		CSVManager.SetCurrentLevel (CurrentLevel);
		scores = new int[numberOfLevels];
		Time.timeScale = 1;
		Application.LoadLevel (CurrentLevel);
	}

	public void LoadFirstAdvancedLevel ()
	{
		CurrentLevel = advancedLevelToSceneOffset;
		CSVManager.SetCurrentLevel (CurrentLevel);
		scores = new int[numberOfLevels];
		Time.timeScale = 1;
		Application.LoadLevel (CurrentLevel);
	}

	public void LoadCustomLevel ()
	{
		CurrentLevel = customLevelIndex;
		CSVManager.SetCurrentLevel (CurrentLevel);
		Time.timeScale = 1;
		Application.LoadLevel (CurrentLevel);
	}

	void ReadInSettings ()
	{
		soundFxVolume = PlayerPrefs.GetFloat (SoundFxPrefsString, 0.5f);
		musicVolume = PlayerPrefs.GetFloat (MusicPrefsString, 0.5f);
		SoundController.instance.SoundFxVolume = soundFxVolume;
		SoundController.instance.MusicVolume = musicVolume;
	}

	public static int GetLowestScore (ScoresData data)
	{
		int lowestScore = Int32.MaxValue;
		foreach (Score score in data.scores) {
			if (score == null)
				return lowestScore;
			if (score.value < lowestScore)
				lowestScore = score.value;
		}
		return lowestScore;
	}

	void ReadInSavedGames ()
	{
		if (File.Exists (savedGamesPath)) {
			FileStream file = File.OpenRead (savedGamesPath);
			SavedGamesData data = (bf.Deserialize (file) as SavedGamesData);
			SavedGames = data == null ? new SavedGame[numberOfSavedGames] : data.Games;
		} else 
			SavedGames = new SavedGame[numberOfSavedGames];
	}

	public bool SaveGame (int slot)
	{
		SavedGames [slot] = new SavedGame (CurrentLevel + 1, scores);
		return PersistSavedGames ();
	}

	bool PersistSavedGames ()
	{
		try {
			FileStream file = File.Open (savedGamesPath, FileMode.OpenOrCreate);
			bf.Serialize (file, new SavedGamesData (SavedGames));
			file.Close ();
			return true;
		} catch (Exception e) {
			Debug.Log(e.Message);
			return false;
		}
	}

	public void LoadGame (int gameIndex)
	{
		SavedGame game = SavedGames [gameIndex];
		scores = game.Scores;
		CurrentLevel = game.CurrentLevel - 1;
		NextLevel ();
	}

}