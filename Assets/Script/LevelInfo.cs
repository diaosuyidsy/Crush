using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// #############################
// Note this script is attached to the different level button in select level scene
// #############################
public class LevelInfo : MonoBehaviour {

	public int LevelNumber;

	public GameObject First_Star;
	public GameObject Second_Star;
	public GameObject Third_Star;


	public int GetLevelNumber()
	{
		return LevelNumber;
	}

	public void Enable_Stars(int StarNum)
	{
		switch (StarNum)
		{
		case 0:
			GetComponent<Button> ().interactable = true;
			break;
		case 1:
			Enable_One ();
			break;
		case 2:
			Enable_Two ();
			break;
		case 3:
			Enable_Three ();
			break;
		}
	}


	private void Enable_One()
	{
		First_Star.SetActive (true);
	}
	private void Enable_Two()
	{
		Enable_One ();
		Second_Star.SetActive (true);
	}
	private void Enable_Three()
	{
		Enable_Two ();
		Third_Star.SetActive (true);
	}
}
