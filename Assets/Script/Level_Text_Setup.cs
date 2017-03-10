using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Text_Setup : MonoBehaviour {
	public Text text;

	void Start()
	{
		int levelnum = SceneManager.GetActiveScene ().buildIndex - 1;
		text.text = "LEVEL " + levelnum;
	}
}
