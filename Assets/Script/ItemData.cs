using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemData : MonoBehaviour {

	public string item_name;
	public int price;
	public string Introduction_text;
	public bool equipped;
	public Text displayed_text;

	void Start()
	{
		displayed_text.text = item_name;
	}
}
