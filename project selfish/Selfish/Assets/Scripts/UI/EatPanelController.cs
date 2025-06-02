using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EatPanelController : MonoBehaviour {

	public string prefix = "EAT: ";
	Text text;

	void Awake ()
	{
		text = transform.Find ("EatText").GetComponent<Text>();
	}

	public void UpdateFood (int value)
	{
		text.text = prefix + value;
	}

}
