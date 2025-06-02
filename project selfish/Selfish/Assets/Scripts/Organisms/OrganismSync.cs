using UnityEngine;
using System.Collections;
using Photon;

public class OrganismSync : PunBehaviour {

	public virtual void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {//our player or, if masterclient, the AI
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			transform.position = (Vector3) stream.ReceiveNext();
			transform.rotation = (Quaternion) stream.ReceiveNext();
		}
	}

}
