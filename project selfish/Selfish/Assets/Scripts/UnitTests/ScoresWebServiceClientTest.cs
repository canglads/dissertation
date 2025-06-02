using NUnit.Framework;

[TestFixture]
public class ScoresWebServiceClientTest
{
	
	[Test]
	public void ParseScoresJson ()
	{
		string json = "[{\"scores\": [{\"name\": \"a\", \"value\": 3}, {\"name\": \"b\", \"value\": 1}, {\"name\": \"c\", \"value\": 3}]}]";
		ScoresData data = ScoresWebServiceClient.ParseScoresJson (json);
		Assert.AreEqual ("c", data.scores[2].name);
		Assert.AreEqual (1, data.scores[1].value);
	}
	
}