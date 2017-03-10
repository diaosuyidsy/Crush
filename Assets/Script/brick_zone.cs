using UnityEngine;
using System.Collections;

public class brick_zone : MonoBehaviour {

	private GameObject explode;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Bullet"){
			explode = null;
			explode = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller> ().explode;
			Instantiate (explode, this.transform.position, Quaternion.identity );
			Destroy (other.gameObject);
			StartCoroutine (destroy_this (this.gameObject, 0.1f));
		}
	}

	IEnumerator destroy_this(GameObject GO, float wait_time)
	{
		yield return new WaitForSeconds (wait_time);

		Destroy (GO);
	}

}
