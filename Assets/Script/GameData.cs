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
	public int coin_add;

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

	public void setLevelInfo(int LevelNum, int star, float finalscore, int coin_added)
	{
		levelNum = LevelNum;
		starNum = star;
		score = finalscore;
		coin_add = coin_added;
	}

	public void setLevelInfo(int LevelNum, int star, float finalscore)
	{
		setLevelInfo (LevelNum, star, finalscore, 0);
	}

	private void save_coin(Levels levels)
	{
		// Save Coin for player
		if(starNum > levels.LevelBook [levelNum].starNum)
		{
			int temp = levels.Gold_Coin;
			if(starNum == 2)
			{
				temp += 1;
			}
			if(starNum == 3)
			{
				temp += 3;
			}
			levels.Gold_Coin = temp;
		}
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
			// Save the coin addition whatsoever
			levels.Gold_Coin += coin_add;
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

	public Levels Load()
	{
		if(File.Exists (Application.persistentDataPath + "/levelinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/levelinfo.dat", FileMode.Open);
			Levels levels = (Levels)bf.Deserialize (file);

			file.Close ();
			return levels;
		}
		return null;
	}

	public void Save_Shop_Item(string item_name, int item_price, bool isequipped)
	{
		if(File.Exists (Application.persistentDataPath + "/shopinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/shopinfo.dat", FileMode.Open);
			Shop shop = (Shop)bf.Deserialize (file);
			file.Close ();
			FileStream file1 = File.Open (Application.persistentDataPath + "/shopinfo.dat", FileMode.Open);
			if(shop.items.ContainsKey (item_name))
			{
				shop.items [item_name].price = item_price;
				shop.items [item_name].is_equipped = isequipped;
			}else{
				Item this_item = new Item (item_price, isequipped);
				shop.items [item_name] = this_item;
			}
			bf.Serialize (file1, shop);
			file1.Close ();
		}else{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/shopinfo.dat");
			Shop shop = new Shop ();
			shop.items = new Dictionary<string, Item> ();
			Item this_item = new Item (item_price, isequipped);
			shop.items [item_name] = this_item;

			// Save the shop to file
			bf.Serialize (file, shop);
			file.Close ();
		}
	}

	public Shop Load_Shop()
	{
		if(File.Exists (Application.persistentDataPath + "/shopinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/shopinfo.dat", FileMode.Open);
			Shop shop = (Shop)bf.Deserialize (file);

			file.Close ();
			return shop;
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

[Serializable]
public class Shop
{
	public Dictionary<string, Item> items;
}

[Serializable]
public class Item
{
	public int price;
	public bool is_equipped;

	public Item(int price, bool isequipped)
	{
		this.price = price;
		is_equipped = isequipped;
	}
}