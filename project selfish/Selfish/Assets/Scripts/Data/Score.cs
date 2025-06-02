using System;

[Serializable]
public class Score : IComparable
{
	// lower-case: json	
	public string name { get; set; }
	// lower-case: json
	public int value { get; set; }
		
	public Score (string name, int value)
	{
		this.name = name;
		this.value = value;
	}
		
	public int CompareTo (object obj)
	{
		return value.CompareTo ((obj as Score).value);
	}
		
}

