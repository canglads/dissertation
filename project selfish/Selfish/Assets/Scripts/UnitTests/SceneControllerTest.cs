using NUnit.Framework;

[TestFixture]
public class SceneControllerTest
{
	
	[Test]
	public void Format ()
	{
		Assert.AreEqual (SceneController.Format(5, 45), "5 (10%)");
		Assert.AreEqual (SceneController.Format(45, 5), "45 (90%)");
		Assert.AreEqual (SceneController.Format(0, 0), "0 (0%)");
	}
	
}
