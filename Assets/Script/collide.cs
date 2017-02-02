using UnityEngine;
using System.Collections;

public class collide : MonoBehaviour {

	public GameObject explode;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Bullet"){
			Instantiate (explode, this.transform.position, Quaternion.identity );
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", Time.time);
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}

	}
}
