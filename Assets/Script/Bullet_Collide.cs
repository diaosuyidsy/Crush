using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Collide : MonoBehaviour {

	private List<Vector3> bulletposs;

	void OnEnable()
	{
		bulletposs = new List<Vector3>();
		StartCoroutine (savePos ());
	}

	IEnumerator savePos()
	{
		bulletposs.Add (transform.position);
		yield return new WaitForSeconds (0.08f);
		StartCoroutine (savePos ());
	}

	void sendPos()
	{
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller>().bulletPoses.Add (bulletposs);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Target"){
			Score_param a = new Score_param (Time.time, true);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");
			sendPos ();
			Destroy (gameObject);
		}else if(other.gameObject.tag == "Brick" || other.gameObject.tag == "Deathzone"){
			Score_param a = new Score_param (Time.time, false);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");
			sendPos ();
			Destroy (gameObject);
		}else if(other.gameObject.tag == "Bounce" || other.gameObject.tag == "EnviromentBounce")
		{
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Bounce");
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Target"){
			Score_param a = new Score_param (Time.time, true);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");

			sendPos ();
			Destroy (gameObject);
		}else if(other.gameObject.tag == "Brick" || other.gameObject.tag == "Deathzone"){
			Score_param a = new Score_param (Time.time, false);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");

			sendPos ();
			Destroy (gameObject);
		}else if(other.gameObject.tag == "Bounce" || other.gameObject.tag == "EnvironmentBounce")
		{
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Bounce");

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
