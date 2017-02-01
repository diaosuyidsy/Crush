using UnityEngine;
using System.Collections;

public class collide : MonoBehaviour {

	public GameObject explode;

	void OnTriggerEnter2D(Collider2D other){
		Instantiate (explode, this.transform.position, Quaternion.identity );
		GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", Time.time);
		Destroy (this.gameObject);
	}
}
