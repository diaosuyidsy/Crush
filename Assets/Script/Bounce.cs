using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	private float speed;
	private float UpEdge;
	private bool dirUp = true;

	// Use this for initialization
	void Start () {
		speed = GetComponent <Rigidbody2D> ().velocity.y;
		UpEdge = Camera.main.orthographicSize * Screen.height / Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dirUp){
			transform.Translate (-Vector2.up * speed * Time.deltaTime * 2);
		}
			
		if(transform.position.y >= UpEdge) {
			dirUp = false;
		}
	}
}
