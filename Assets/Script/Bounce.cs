using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	private float speedy;
	private float speedx;
	private float UpEdge;
	private bool dirUp = true;
	private float RightEdge;
	private bool turnLeft = false;
	private bool turnRight = false;


	// Use this for initialization
	void Start () {
		speedy = GetComponent <Rigidbody2D> ().velocity.y;
		speedx = GetComponent <Rigidbody2D> ().velocity.x;
		UpEdge = Screen.height;
		RightEdge = Screen.width;
		Debug.Log (speedx);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempos = Camera.main.WorldToScreenPoint (transform.position);

		if (!dirUp){
			transform.Translate (-Vector2.up * speedy * Time.deltaTime * 2);
		}

//		if(turnLeft){
//			Debug.Log ("turning left");
//			transform.Translate (-Vector2.right * speedx * Time.deltaTime * 2);
//			speedx = GetComponent <Rigidbody2D> ().velocity.x;
//			Debug.Log (speedx);
////			turnLeft = false;
//		}

		if(tempos.x >= RightEdge){
			Debug.Log ("fall right");
			turnLeft = true;
			turnRight = false;
		}

		if(tempos.x <= 0){
			Debug.Log ("fall left");
			turnRight = true;
			turnLeft = false;
		}

		if(turnLeft){
			speedx = GetComponent <Rigidbody2D> ().velocity.x;
			transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-speedx, speedy);
			turnLeft = false;
		}

//		if(turnRight){
//			Debug.Log ("turning right");
//			transform.Translate (Vector2.left * speedx * Time.deltaTime * 2);
//			speedx = GetComponent <Rigidbody2D> ().velocity.x;
////			turnRight = false;
//		}

		if(turnRight){
			speedx = GetComponent <Rigidbody2D> ().velocity.x;
			transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-speedx, speedy);
			turnRight = false;
		}

			
		if(tempos.y >= UpEdge) {
			dirUp = false;
		}


	}
}
