using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// static convenience methods for transform manipulation
public class TransformUtil : MonoBehaviour {

	public static void DestroyAllChildren (Transform parent) 
	{
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in parent)
			children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
	}

}
