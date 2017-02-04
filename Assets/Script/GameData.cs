using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	public static GameData gd;

	public int levelNum;
	public int starNum;
	public float score;

	void Awake()
	{
		if(gd == null)
		{
			DontDestroyOnLoad (gameObject);
			gd = this;
		}
		else if(gd == this)
		{
			Destroy (gameObject);
		}
	}

	public void setLevelInfo(int LevelNum, int star, float finalscore)
	{
		levelNum = LevelNum;
		starNum = star;
		score = finalscore;
	}


	public void Save()
	{
		if(File.Exists (Application.persistentDataPath + "/levelinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/levelinfo.dat", FileMode.Open);
			Levels levels = (Levels)bf.Deserialize (file);
			file.Close ();
			FileStream file1 = File.Open (Application.persistentDataPath + "/levelinfo.dat", FileMode.Open);
			// If there is a level named levelname, try to update the starNum and score if it's higher
			if(levels.LevelBook.ContainsKey (levelNum)){
//				levels.LevelBook [levelNum].score = levels.LevelBook [levelNum].score < score ? score : levels.LevelBook [levelNum].score;
//				levels.LevelBook [levelNum].starNum = levels.LevelBook [levelNum].starNum < starNum ? starNum : levels.LevelBook [levelNum].starNum;
				if(levels.LevelBook[levelNum].starNum >= this.starNum)
				{
				}else
				{
					levels.LevelBook [levelNum].starNum = this.starNum;
				}
			}else{
				// If there is no level, then create one
				Level curLevel = new Level ();
				curLevel.starNum = this.starNum;
				curLevel.score = this.score;
				levels.LevelBook [levelNum] = curLevel;
			}

			// Save the levels to file
			bf.Serialize (file1, levels);
			file1.Close ();
		}else{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/levelinfo.dat");
			Levels levels = new Levels();

			Level curLevel = new Level ();
			curLevel.starNum = this.starNum;
			curLevel.score = this.score;
			levels.LevelBook = new Dictionary<int, Level>();
			levels.LevelBook [levelNum] = curLevel;
			// Save the levels to file
			bf.Serialize (file, levels);
			file.Close ();
		}

	}

	public Dictionary<int, Level> Load()
	{
		if(File.Exists (Application.persistentDataPath + "/levelinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/levelinfo.dat", FileMode.Open);
			Levels levels = (Levels)bf.Deserialize (file);

			file.Close ();
			return levels.LevelBook;
		}
		return null;
	}
}

[Serializable]
public class Levels
{
	public Dictionary<int, Level> LevelBook;

}

[Serializable]
public class Level
{
	public int starNum;
	public float score;
}