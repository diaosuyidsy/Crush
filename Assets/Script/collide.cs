using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class collide : MonoBehaviour
{

	public GameObject explode;

	private bool enabledd = false;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Bullet") {
			explode = null;
			explode = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller> ().explode;
			Instantiate (explode, this.transform.position, Quaternion.identity);
			gameObject.SetActive (false);
		}
	}

	void OnEnable ()
	{
		if (!enabledd) {
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("target_numAdd1");
			enabledd = true;
		}
	}
}
