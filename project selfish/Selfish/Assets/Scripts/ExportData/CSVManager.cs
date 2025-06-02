using UnityEngine;
using System.IO;
using System;

public static class CSVManager{

	private static Stats stats;
	private static string timeStart;
	private static string time;
	private static string timeLastRecord;
	private static string currentLevel = "0";
	private static string[] strings ;
	private static string exportFolderName = "ExportedData";
	private static string exportFileName = "Level-";
	private static string fileSeparator = ",";
	private static string fileExtension = ".csv";
	private static string[] fileHeaders = new string[6] { "Time", "PlayerFish", "TotalFish", "Self", "Cross", "Score" };
	private static string fileHeaderTemp = "Temp";
	private static string currentTemp = "0";
	private static string getTime = "1970-01-01-00-00-00";


	public static void VerifyDirectory()
	{
		string directoryPath = getDirectoryPath();
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
	}
	
	public static void VerifyFile()
	{
		string filePath = getFilePath();
		if (!File.Exists(filePath))
		{
			CreateFile();
		}
	}

	static string getDirectoryPath(){
		return Application.dataPath + "/" + exportFolderName;
	}

	static string getFilePath(){
		return getDirectoryPath() + "/" + exportFileName + currentLevel + "-" + getTime + fileExtension;
	}

	public static void CreateFile(){
		VerifyDirectory();
		GetTime();
		using(StreamWriter sw = File.CreateText(getFilePath())){
			string finalString = "";
			for(int i = 0; i < fileHeaders.Length; i++){
				if(finalString != ""){
					finalString += fileSeparator;
				}
				finalString += fileHeaders[i];
			}
			if(currentLevel == "12"){
				finalString += fileSeparator;
				finalString += fileHeaderTemp;
			
			}
			finalString += "";
			sw.WriteLine(finalString);
		}
	}

	public static void AppendToReport(){
		VerifyDirectory();
		VerifyFile();
		using(StreamWriter sw = File.AppendText(getFilePath())){
			string finalString = "";
			for(int i = 0; i < strings.Length; i++){
				if(finalString != ""){
					finalString += fileSeparator;
				}
				finalString += strings[i]; // Add the Data to the final string
			}
			if(currentLevel == "12"){
				finalString += fileSeparator;
				finalString += currentTemp;
			}
			finalString += "";
			sw.WriteLine(finalString);
			
		}
	}

	public static void GetStats(Stats incomingStats){
		stats = incomingStats;
		SetTime();
		time = ConvertTime(timeStart, stats.TimeLeft.Split(' ')[0]);
		CheckIfTimeIsRight();
		RecordTime();
		strings = new string[6] { time, stats.YourFish, stats.TotalFish, stats.Selfs, stats.Crosses, stats.Score };
	}

	private static void SetTime(){
		if(timeStart == null){
			timeStart = stats.TimeLeft.Split(' ')[0];
			timeLastRecord = timeStart;
		}
	}
	
	public static void GetTime(){
		getTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
	}

	private static string ConvertTime(string timeStart, string timeLeft){
		int timeStartInt = int.Parse(timeStart);
		int timeLeftInt = int.Parse(timeLeft);
		return (timeStartInt - timeLeftInt).ToString();
	}
	
	public static void DeleteFile(){
		File.Delete(getFilePath());
	}
	
	private static void RecordTime(){
		timeLastRecord = stats.TimeLeft.Split(' ')[0];
	}

	private static void CheckIfTimeIsRight(){
		if(int.Parse(stats.TimeLeft.Split(' ')[0]).ToString() == timeLastRecord){
			return;
		}
		if((int.Parse(stats.TimeLeft.Split(' ')[0]) + 1).ToString()  != timeLastRecord){
			timeStart = null;
			SetTime();
			RecordTime();
			time = ConvertTime(timeStart, stats.TimeLeft.Split(' ')[0]);
			CreateFile();
		}
	}

	public static void SetCurrentLevel(int level){
		currentLevel = level.ToString();
	}

	public static void SetTemp(int temp){
		currentTemp = temp.ToString();
	}
}
