using System.Collections;
using UnityEngine;
using SimpleJSON;

public class ScoresWebServiceClient : MonoBehaviour
{

	const string GetUrl = "http://selfish.itclarke.webfactional.com/scores";
	const string SubmitBaseUrl = "http://selfish.itclarke.webfactional.com/submitscore";

	public delegate void ScoresDataCallback (ScoresData data);
	public delegate void SubmitScoreCallback (bool successful);

	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
	}

	public void Get (ScoresDataCallback callback) {
		WWW www = new WWW (GetUrl);
		StartCoroutine (GetScores (www, callback));
	}

	public void Submit (Score score, SubmitScoreCallback callback) {
		string url = SubmitBaseUrl + "/" + WWW.EscapeURL (score.name) + "/" + score.value;
		WWW www = new WWW (url);
		StartCoroutine (SubmitScore (www, callback));
	}

	IEnumerator GetScores (WWW www, ScoresDataCallback callback)
	{
		yield return www;
		if (www.error == null) {
			callback (ParseScoresJson(www.text));
		} else {
			callback (null);
		}
	}

	IEnumerator SubmitScore (WWW www, SubmitScoreCallback callback)
	{
		yield return www;
		callback (www.error == null);
	}

	public static ScoresData ParseScoresJson (string jsonString)
	{
		JSONNode obj = JSON.Parse(jsonString);
		JSONNode scores = obj [0] ["scores"];
		ScoresData data = new ScoresData ();
		data.scores = new Score[scores.AsArray.Count];
		int count = 0;
		foreach (JSONNode score in scores.AsArray) {
			if (score.Value.Equals("null")) break;
			data.scores[count] = new Score(score["name"].Value, score["value"].AsInt);
			count++;
		}
		return data;
	}

}
