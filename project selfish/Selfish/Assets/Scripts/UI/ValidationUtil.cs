using System;

public class ValidationUtil {

	public static int ParseInput (string input, int defaultValue, int minValid, int maxValid)
	{
		if (string.IsNullOrEmpty (input))
			return defaultValue;
		int parsed = Int32.Parse (input);
		return (parsed < minValid || parsed > maxValid) ? defaultValue :
			parsed;
	}

	public static float ParseInput (string input, float defaultValue, float minValid, float maxValid)
	{
		if (string.IsNullOrEmpty (input))
			return defaultValue;
		float parsed = float.Parse (input);
		return (parsed < minValid || parsed > maxValid) ? defaultValue :
			parsed;
	}

}
