using NUnit.Framework;

[TestFixture]
public class GameControllerTest
{

	[Test]
	public void GetLowestScore ()
	{
		ScoresData data = new ScoresData ();
		data.scores = new Score[]{
			new Score ("a", 2),
			new Score ("b", 1),
			new Score ("c", 3)
		};
		int lowestScore = GameController.GetLowestScore (data);
		Assert.AreEqual (1, lowestScore);
	}

}
