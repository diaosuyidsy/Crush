using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	private float speedy;
	private float speedx;
	private float UpEdge;
	private float RightEdge;

	private bool turnBack = false;
	private bool turnLeft = false;
	private bool turnRight = false;


	// Use this for initialization
	void Start () {
		speedy = GetComponent <Rigidbody2D> ().velocity.y;
		speedx = GetComponent <Rigidbody2D> ().velocity.x;
		UpEdge = Screen.height;
		RightEdge = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempos = Camera.main.WorldToScreenPoint (transform.position);

		if(tempos.x >= RightEdge){
			Debug.Log ("fall right");
			turnLeft = true;
			turnRight = false;
			turnBack = false;
		}

		if(tempos.x <= 0){
			Debug.Log ("fall left");
			turnRight = true;
			turnLeft = false;
			turnBack = false;
		}

		if(tempos.y >= UpEdge) {
			Debug.Log ("turn back");
			turnBack = true;
			turnLeft = false;
			turnRight = false;
		}

		if(turnLeft){
			speedx = GetComponent <Rigidbody2D> ().velocity.x;
			speedy = GetComponent <Rigidbody2D> ().velocity.y;
			transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-speedx, speedy);
			turnLeft = false;
		}

		if(turnRight){
			speedx = GetComponent <Rigidbody2D> ().velocity.x;
			speedy = GetComponent <Rigidbody2D> ().velocity.y;
			transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-speedx, speedy);
			turnRight = false;
		}

		if (turnBack){
			speedy = GetComponent <Rigidbody2D> ().velocity.y;
			speedx = GetComponent <Rigidbody2D> ().velocity.x;
			transform.GetComponent<Rigidbody2D>().velocity = new Vector2(speedx, -speedy);
			turnBack = false;
		}
	}
}
// Just in case I forgot how to use translate
//if (!dirUp){
//	transform.Translate (-Vector2.up * speedy * Time.deltaTime * 2);
//}