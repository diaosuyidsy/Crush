using UnityEngine;
using System.Collections;

public class Bullet_Collide : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Target"){
			Score_param a = new Score_param (Time.time, true);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
		}else if(other.gameObject.tag == "Brick" || other.gameObject.tag == "Deathzone"){
			Score_param a = new Score_param (Time.time, false);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		Debug.Log (other.gameObject.tag);
		if(other.gameObject.tag == "Target"){
			Score_param a = new Score_param (Time.time, true);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
		}else if(other.gameObject.tag == "Brick" || other.gameObject.tag == "Deathzone"){
			Score_param a = new Score_param (Time.time, false);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
		}
	}
}

public struct Score_param{
	public float time;
	public bool succeed;
	public Score_param(float time1, bool succeed1){
		time = time1;
		succeed = succeed1;
	}
}
