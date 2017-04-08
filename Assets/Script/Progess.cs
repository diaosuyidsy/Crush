using UnityEngine;
using System.Collections;

public class Progess : MonoBehaviour
{
	public float barDisplay;
	//current progress
	public Vector2 pos = new Vector2 (20, 40);
	public Vector2 size = new Vector2 (60, 20);
	public GUIStyle progress_empty;
	public GUIStyle progress_full;


	void OnGUI ()
	{
		//draw the background:
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), string.Empty, progress_empty);
//		//draw the filled-in part:
		GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), string.Empty, progress_full);

		GUI.EndGroup ();
		GUI.EndGroup ();
	}

	void Update ()
	{
		//for this example, the bar display is linked to the current time,
		//however you would set this value based on your desired display
		//eg, the loading progress, the player's health, or whatever.
		if (Input.anyKeyDown) {
			barDisplay -= Time.deltaTime * 0.5f;
		}
		//        barDisplay = MyControlScript.staticHealth;
	}
}
