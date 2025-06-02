using UnityEngine;
using System.Collections;

public class Stats {

	public string YourFish { get; private set; }
	public string TotalFish { get; private set; }
	public string Score { get; private set; }
	public string TimeLeft { get; private set; }
	public string Selfs { get; private set; }
	public string Crosses { get; private set; }

	public Stats (string yourFish, string totalFish, string score, string timeLeft,
	              string selfs, string crosses)
	{
		YourFish = yourFish;
		TotalFish = totalFish;
		Score = score;
		TimeLeft = timeLeft;
		Selfs = selfs;
		Crosses = crosses;
	}

}
