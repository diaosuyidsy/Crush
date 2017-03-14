using UnityEngine;
using System.Collections;

public class Slow_Block : MonoBehaviour {

	[Range(0.0f, 5.0f)]
	public float Slow_Ratio;

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Bullet"){
			Vector2 v2 = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			v2 = new Vector2 (v2.x * Slow_Ratio, v2.y * Slow_Ratio);
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = v2;
			Destroy (gameObject);
		}
	}
}
