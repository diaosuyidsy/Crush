using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class collide : MonoBehaviour {

	public GameObject explode;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Bullet"){
			explode = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller> ().explode;
			Instantiate (explode, this.transform.position, Quaternion.identity );
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", Time.time);
			Destroy (other.gameObject);
			StartCoroutine (destroy_this (this.gameObject, 0.2f));
		}
	}

	IEnumerator destroy_this(GameObject GO, float wait_time)
	{
		yield return new WaitForSeconds (wait_time);

		Destroy (GO);
	}

}
