using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	private float speed;
	private float UpEdge;
	private bool dirUp = true;

	// Use this for initialization
	void Start () {
		speed = GetComponent <Rigidbody2D> ().velocity.y;
		UpEdge = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempos = Camera.main.WorldToScreenPoint (transform.position);
		if (!dirUp){
			transform.Translate (-Vector2.up * speed * Time.deltaTime * 2);
		}
			
		if(tempos.y >= UpEdge) {
			dirUp = false;
		}
	}
}
