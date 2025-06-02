using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NonNetworkFish : Fish {

	public FishGrowth Growth { get; protected set; }
	public FishHealth Health { get; protected set; }
	public FishReproduction Reproduction { get; protected set; }
	public ImmuneSystem ImmuneSystem { get; protected set; }

	public abstract void RemoveFromSceneController ();
	
	public void Init (List<int> playerIndexes, List<PathogenType> immunities)
	{
		PlayerIndexes = playerIndexes ?? PlayerIndexes;
		ImmuneSystem.Immunities = immunities;
	}

	public abstract void ConvertToNetworkFish ();

	protected override void Awake ()
	{
		base.Awake ();
		ImmuneSystem = GetComponent<ImmuneSystem> ();
	}

}
