using NUnit.Framework;

[TestFixture]
public class MultiplayerLevelControllerTest
{
	
	[Test]
	public void GetResult ()
	{
		MultiplayerScore[] scores = new MultiplayerScore[]{
			new MultiplayerScore (0, 3),
			new MultiplayerScore (1, 2),
			new MultiplayerScore (2, 1),
		};
		int result = MultiplayerLevelController.GetResult (scores, 1);
		Assert.AreEqual (-1, result);
		result = MultiplayerLevelController.GetResult (scores, 0);
		Assert.AreEqual (1, result);
		scores [2].Score = 3;
		result = MultiplayerLevelController.GetResult (scores, 0);
		Assert.AreEqual (0, result);
	}
	
}
