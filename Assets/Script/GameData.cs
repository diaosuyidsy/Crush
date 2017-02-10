using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	#region Variable Declaration

	public static GameData gd;

	public int levelNum;
	public int starNum;
	public float score;

	public int Level_Select_Pointer;

	public int max_Level_Select_Pointer;
	public int min_Level_Select_Pointer;

	#endregion

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

	private void save_coin(Levels levels)
	{
		// Save Coin for player
		if(starNum > levels.LevelBook [levelNum].starNum)
		{
			Debug.Log ("entered");
			int temp = levels.Gold_Coin;
			Debug.Log (temp);
			if(starNum == 2)
			{
				Debug.Log ("Added 1");
				temp += 1;
			}
			if(starNum == 3)
			{
				Debug.Log ("Added 3");
				temp += 3;
			}
			levels.Gold_Coin = temp;
		}
		Debug.Log ("Now player Coin: " + levels.Gold_Coin);
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
				save_coin (levels);
				levels.LevelBook [levelNum].score = levels.LevelBook [levelNum].score < score ? score : levels.LevelBook [levelNum].score;
				levels.LevelBook [levelNum].starNum = levels.LevelBook [levelNum].starNum < starNum ? starNum : levels.LevelBook [levelNum].starNum;

			}else{
				// If there is no level, then create one
				levels.LevelBook [levelNum] = new Level();
				save_coin (levels);
				levels.LevelBook [levelNum].starNum = this.starNum;
				levels.LevelBook [levelNum].score = this.score;

			}
			// Save the levels to file
			bf.Serialize (file1, levels);
			file1.Close ();
		}else{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/levelinfo.dat");
			Levels levels = new Levels();

			levels.LevelBook = new Dictionary<int, Level>();
			levels.LevelBook [levelNum] = new Level();
			save_coin (levels);
			levels.LevelBook [levelNum].starNum = this.starNum;
			levels.LevelBook [levelNum].score = this.score;

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
	public int Gold_Coin;
}

[Serializable]
public class Level
{
	public int starNum;
	public float score;
}