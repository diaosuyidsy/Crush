using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Bullet"){
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("FailLevel");
			Destroy (other.gameObject);
		}
	}
}
