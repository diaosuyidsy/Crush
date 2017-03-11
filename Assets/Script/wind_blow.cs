using UnityEngine;
using System.Collections;

public class wind_blow : MonoBehaviour {

	public Vector2 offset;

	void OnCollisionStay2D(Collision2D other){
		Debug.Log ("Collision stay");
		if(other.gameObject.tag == "Bullet"){
			Vector2 v2 = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			v2 += offset;
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = v2;
		}
	}
}
