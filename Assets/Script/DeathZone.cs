using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("FailLevel");
		Time.timeScale = 0;
		Destroy (other.gameObject);
	}
}
