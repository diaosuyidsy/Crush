using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HintPool : MonoBehaviour
{
	public Text HintText;

	public void ChangeHintText ()
	{
		if (SceneManager.GetActiveScene ().name == "Level_1") {
			HintText.text = "To Win, Hit all targets at the same time";
		} else {
			HintTextC ();
		}
	}

	void HintTextC ()
	{
		int r = Random.Range (1, 5);
		switch (r) {
		case 1:
			HintText.text = "Hint: Try Make Best Use of Auxiliary Line.";
			break;
		case 2:
			HintText.text = "Hint: Try More. :)";
			break;
		case 3:
			HintText.text = "Hint: Try Different Angles.";
			break;
		case 4:
			HintText.text = "Hint: If You Too Stupid, Uninstall The Game. :)";
			break;
		}
	}
}
