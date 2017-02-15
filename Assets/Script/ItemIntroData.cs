using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemIntroData : MonoBehaviour {

	#region variabel

	public string item_name;
	public int price;
	public Text introduction_text;
	public Text price_text;
	public Text buy_text;
	public Text buy_text2;
	public bool is_bought;
	public bool is_equipped;
	public GameObject replacing_object;

	#endregion
}
