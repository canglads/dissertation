using UnityEngine;
using System.Collections.Generic;

public abstract class SceneController : MonoBehaviour {
	
	public GameObject playerTemplate;
	public AudioClip deadAudio;
	public AudioClip survivedAudio;
	public AudioClip switchAudio;
	
	public int Score { get; set; }
	public int Selfs { get; set; }
	public int Crosses { get; set; }
	public GameObject CurrentPlayer { get; set; }
	public bool IsMultiplayer { get; protected set; }
	
	protected ParameterController param;
	protected StartPositionsController startPos;
	protected UIController uiController;
	protected bool gameOver;
	protected float timer;

	public abstract Stats GetStats ();

	public abstract void HandleCurrentPlayerFishDead ();
	public abstract void CreateOffspring (int numberOfOffspring, List<int> playerIndexes, List<PathogenType> immunities, bool self, Vector3 position);

	public virtual void RemoveHerm (GameObject herm)
	{
		// default to no behaviour
	}

	public void AddRandomImmunities (GameObject fish)
	{
		fish.GetComponent<ImmuneSystem>().Immunities.AddRange (GetRandomImmmunities());
	}

	public List<PathogenType> GetRandomImmmunities ()
	{
		int totalCount = PathogenType.GetValues (typeof(PathogenType)).Length;
		List<PathogenType> immunities = new List<PathogenType> ();
		int number = Random.Range (0, param.maxInitialImm) + 1;
		for (int i = 0; i < number;) {
			PathogenType imm = (PathogenType) Random.Range (0, totalCount);
			if (!immunities.Contains (imm)) {
				immunities.Add (imm);
				i++;
			}
		}
		return immunities;
	}

	public abstract void PropagateMakeMaleMature (GameObject male);

	protected virtual void Awake ()
	{
		GameController.instance.sc = this;
	}

	protected virtual void Start ()
	{
		uiController = UIController.instance;
		startPos = StartPositionsController.instance;
		param = ParameterController.instance;
		uiController.DisplayImmPanels = param.displayImmPanels;
		timer = param.levelLength;
	}
	
	protected virtual void HandleSurvived ()
	{
		Time.timeScale = 0;
		Camera.main.GetComponent<CameraController> ().enabled = false;
		gameOver = true;
		SoundController.instance.PlaySingle (survivedAudio);
	}

	protected virtual void Update ()
	{
		if (gameOver)
			return;
		timer -= Time.deltaTime;
		uiController.DisplayLevelRemainingTime (timer);
		if (timer < 0)
			HandleSurvived ();
	}

	public static string Format (int a, int b)
	{
		if (a == 0 && b == 0)
			return "0 (0%)";
		int percentage = (int)Mathf.Round (a / (float)(a + b) * 100);
		return a + " (" + percentage + "%)";
	}

}
