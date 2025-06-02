using UnityEngine;
using System.Collections;

public class StartPositionsController : MonoBehaviour {

	public static StartPositionsController instance;

	public Transform[] startPositions;

	bool[] startPositionOccupied;

	void Awake ()
	{
		instance = this;
		startPositionOccupied = new bool[startPositions.Length];
	}
	
	public Vector3 GetRandomStartPosition (bool startOfLevel)
	{
		if (startOfLevel)
			for (int i = 0; i < startPositions.Length; i++)
			if (!startPositionOccupied [i]) {
				startPositionOccupied[i] = true;
				return startPositions [i].position;
			}
		return startPositions[Random.Range (0, startPositions.Length)].position;
	}
	
}
