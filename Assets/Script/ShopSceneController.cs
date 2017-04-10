using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopSceneController : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Canvas
	public Canvas m_Canvas;

	public GUIAnimFREE intro_panel;
	public GUIAnimFREE buy_button;
	public GUIAnimFREE cancel_button;

	public GameObject intro_panel_data;

	//	private int gold;



	#endregion // Variables


	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	void Awake ()
	{
		if (enabled) {
			// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false in Awake() will let you control all GUI Animator elements in the scene via scripts.
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start ()
	{
		// Disable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable (m_Canvas, true);

		// Get the gold num
//		gold = GameData.gd.Load ().Gold_Coin;

	}

	#endregion // MonoBehaviour

	// ########################################
	// MoveIn/MoveOut functions
	// ########################################


	// ########################################
	// Enable/Disable button functions
	// ########################################


	// ########################################
	// UI Responder functions
	// ########################################

	#region UI Responder

	public void OnButton_ClickHome ()
	{
		SceneManager.LoadScene ("StartScene");
	}

	public void OnButton_ClickItem ()
	{
		GameObject cur_sel = EventSystem.current.currentSelectedGameObject;
		// Setup item
		intro_panel_data.GetComponent<ItemIntroData> ().price = cur_sel.GetComponent<ItemData> ().price;
		intro_panel_data.GetComponent<ItemIntroData> ().price_text.text = "Price:       " + intro_panel_data.GetComponent<ItemIntroData> ().price.ToString ();
		intro_panel_data.GetComponent<ItemIntroData> ().item_name = cur_sel.GetComponent<ItemData> ().item_name;
		intro_panel_data.GetComponent<ItemIntroData> ().introduction_text.text = cur_sel.GetComponent<ItemData> ().Introduction_text;
		intro_panel_data.GetComponent<ItemIntroData> ().replacing_object = cur_sel.GetComponent<ItemData> ().replacing_art_prefab;

		Shop shop = GameData.gd.Load_Shop ();
		string name_of_item = EventSystem.current.currentSelectedGameObject.GetComponent<ItemData> ().item_name;
		intro_panel_data.GetComponent<ItemIntroData> ().is_bought = false;
		if (shop == null) {
			
		} else {
			if (shop.items.ContainsKey (name_of_item)) {
				intro_panel_data.GetComponent<ItemIntroData> ().is_bought = true;
				intro_panel_data.GetComponent<ItemIntroData> ().is_equipped = shop.items [name_of_item].is_equipped;
			}
		}
		display_intro ();
	}

	public void OnButton_ClickBuy ()
	{
		Shop shop = GameData.gd.Load_Shop ();
		GameObject selected_button = EventSystem.current.currentSelectedGameObject;
		if (!intro_panel_data.GetComponent<ItemIntroData> ().is_bought) {
			int price = selected_button.GetComponentInParent<ItemIntroData> ().price;
			if (canbuy (price)) {
				// Unequip last equipped item
				if (shop != null) {
					Item last_equipped = shop.items [shop.last_equipped_item];
					GameData.gd.Save_Shop_Item (shop.last_equipped_item, last_equipped.price, false);
				}

				// 1st, reduce the gold
				GameData.gd.setLevelInfo (1, 0, 0, -price);
				GameData.gd.Save ();
				GameData.gd.Save_Shop_Item (intro_panel_data.GetComponent<ItemIntroData> ().item_name, intro_panel_data.GetComponent<ItemIntroData> ().price, true);
				// TODO: tell the panel that gold has changed;

			}
		} else {
			foreach (KeyValuePair<string, Item> i in shop.items) {
				GameData.gd.Save_Shop_Item (i.Key, i.Value.price, false);
			}
			GameData.gd.Save_Shop_Item (intro_panel_data.GetComponent<ItemIntroData> ().item_name, intro_panel_data.GetComponent<ItemIntroData> ().price, true);
		}
		// 2nd, equip it
		// set button not interactable
		selected_button.GetComponent<Button> ().interactable = false;
		selected_button.GetComponentInParent<ItemIntroData> ().buy_text.text = "Equipped";

		// set the last equipped button interactable

	}

	public void OnButton_ClickCancel ()
	{
		undisplay_intro ();
	}


	#endregion // UI Responder

	// ########################################
	// Toggle button functions
	// ########################################

	#region Toggle Button

	// Toggle TopLeft buttons
	public void display_intro ()
	{
		intro_panel.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		buy_button.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		cancel_button.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		reset_buy_button ();
		if (intro_panel_data.GetComponent<ItemIntroData> ().is_bought) {
			if (intro_panel_data.GetComponent<ItemIntroData> ().is_equipped) {
				buy_button.GetComponent<Button> ().interactable = false;
				buy_button.GetComponentInParent<ItemIntroData> ().buy_text.text = "Equipped";
			} else {
				buy_button.GetComponentInParent<ItemIntroData> ().buy_text.text = "Equip";
			}
		}
	}

	public void undisplay_intro ()
	{
		intro_panel.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		buy_button.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		cancel_button.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}

	#endregion // Toggle Button

	#region helper function

	private bool canbuy (int price)
	{
		Levels levels = GameData.gd.Load ();
		if (levels == null)
			return false;
		int gold_player_had = levels.Gold_Coin;
		return gold_player_had >= price;
	}

	private void reset_buy_button ()
	{
		buy_button.GetComponent<Button> ().interactable = true;
		buy_button.GetComponentInParent<ItemIntroData> ().buy_text.text = "Buy";
	}

	#endregion
}
