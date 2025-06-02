using NUnit.Framework;

[TestFixture]
public class ValidationUtilTest
{
	
	[Test]
	public void ParseInputInt ()
	{
		int parsed = ValidationUtil.ParseInput ("1", 7, 0, 9);
		Assert.AreEqual (1, parsed);
		parsed  = ValidationUtil.ParseInput ("-1", 7, 0, 9);
		Assert.AreEqual (7, parsed);
		parsed  = ValidationUtil.ParseInput ("10", 7, 0, 9);
		Assert.AreEqual (7, parsed);
		parsed  = ValidationUtil.ParseInput ("", 7, 0, 9);
		Assert.AreEqual (7, parsed);
		parsed  = ValidationUtil.ParseInput ("9", 7, 0, 9);
		Assert.AreEqual (9, parsed);
		parsed  = ValidationUtil.ParseInput ("0", 7, 0, 9);
		Assert.AreEqual (0, parsed);
	}

	[Test]
	public void ParseInputFloat ()
	{
		float parsed = ValidationUtil.ParseInput ("1.1", 7.1f, 0.1f, 9.1f);
		Assert.AreEqual (1.1f, parsed);
		parsed  = ValidationUtil.ParseInput ("-1.1", 7.1f, 0.1f, 9.1f);
		Assert.AreEqual (7.1f, parsed);
		parsed  = ValidationUtil.ParseInput ("10.1", 7.1f, 0.1f, 9.1f);
		Assert.AreEqual (7.1f, parsed);
		parsed  = ValidationUtil.ParseInput ("", 7.1f, 0.1f, 9.1f);
		Assert.AreEqual (7.1f, parsed);
		parsed  = ValidationUtil.ParseInput ("9.1", 7.1f, 0.1f, 9.1f);
		Assert.AreEqual (9.1f, parsed);
		parsed  = ValidationUtil.ParseInput ("0.1", 7.1f, 0.1f, 9.1f);
		Assert.AreEqual (0.1f, parsed);
	}

}

