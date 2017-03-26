using UnityEngine;
using System.Collections;

public class Launcher_Rotation : MonoBehaviour {

	public GameObject Launcher;
	private bool canrotate=false;
	private Color color;

	// Use this for initialization
	void Start () {
		canrotate = Launcher.GetComponent<LauncherControl> ().can_rotate;
		if(canrotate){
			GetComponent <SpriteRenderer> ().enabled=true;
		}else{
			GetComponent <SpriteRenderer> ().enabled=false;
		}
		color = GetComponent<SpriteRenderer> ().color;
	}

	void OnMouseDown()
	{
		if(canrotate){
			Color tmp = color;
			tmp.a = 1f;
			GetComponent<SpriteRenderer> ().color = tmp;
		}
	}

	void OnMouseDrag ()
	{
		if (canrotate) {
				Vector3 mouse_pos = Input.mousePosition;
				mouse_pos.z = 0;
				Vector3 object_pos = Camera.main.WorldToScreenPoint(Launcher.transform.position);
				mouse_pos.x = mouse_pos.x - object_pos.x;
				mouse_pos.y = mouse_pos.y - object_pos.y;
				float angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
				Launcher.transform.rotation =  Quaternion.Euler(0, 0, angle);
		} 
	}

	void OnMouseUp()
	{
		GetComponent<SpriteRenderer> ().color = color;
	}
}
