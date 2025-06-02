using UnityEngine;
using System.Collections;

public class FishSync : OrganismSync {

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {//our player or, if masterclient, the AI
			stream.SendNext(transform.localScale);
		} else {
			transform.localScale = (Vector3) stream.ReceiveNext();
		}
	}
}
