using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Collide : MonoBehaviour
{

	private List<Vector3> bulletposs;
	public bool hit = false;

	void OnEnable ()
	{
		bulletposs = new List<Vector3> ();
		StartCoroutine (savePos ());
	}

	IEnumerator savePos ()
	{
		bulletposs.Add (transform.position);
		yield return new WaitForSeconds (0.08f);
		StartCoroutine (savePos ());
	}

	void sendPos ()
	{
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller> ().bulletPoses.Add (bulletposs);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Target") {
			Score_param a = new Score_param (Time.time, true);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");
			sendPos ();
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Brick" || other.gameObject.tag == "Deathzone") {
			Score_param a = new Score_param (Time.time, false);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");
			sendPos ();
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Bounce" || other.gameObject.tag == "EnvironmentBounce") {
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Bounce");
		} else if (other.gameObject.tag == "Bullet") {
//			if (!hit) {
//				Vector2 newV = elasticCollision (gameObject.GetComponent<Rigidbody2D> ().velocity, other.gameObject.GetComponent<Rigidbody2D> ().velocity, transform.position, other.gameObject.transform.position);
//				Vector2 newVP = elasticCollision (other.gameObject.GetComponent<Rigidbody2D> ().velocity, gameObject.GetComponent<Rigidbody2D> ().velocity, other.gameObject.transform.position, transform.position);
//
//				gameObject.GetComponent<Rigidbody2D> ().velocity = newV;
//				other.attachedRigidbody.velocity = newVP;
//				other.gameObject.GetComponent<Bullet_Collide> ().hit = true;
//			} else {
//				hit = false;
//			}

		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Target") {
			Score_param a = new Score_param (Time.time, true);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");

			sendPos ();
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Brick" || other.gameObject.tag == "Deathzone") {
			Score_param a = new Score_param (Time.time, false);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessageUpwards ("Score", a);
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Explosion");

			sendPos ();
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Bounce" || other.gameObject.tag == "EnvironmentBounce") {
			GameObject.FindGameObjectWithTag ("GameManager").SendMessage ("playSound", "Bounce");
		}
	}

	Vector2 elasticCollision (Vector3 _v1, Vector3 _v2, Vector3 _x1, Vector3 _x2)
	{
		Vector2 v1 = new Vector2 (_v1.x, _v1.y);
		Vector2 v2 = new Vector2 (_v2.x, _v2.y);
		Vector2 x1 = new Vector2 (_x1.x, _x1.y);
		Vector2 x2 = new Vector2 (_x2.x, _x2.y);
		Vector2 v1P = v1 - (Vector2.Dot (v1 - v2, x1 - x2) / (Vector2.SqrMagnitude (x1 - x2))) * (x1 - x2);
		return v1P;
	}
}

public struct Score_param
{
	public float time;
	public bool succeed;

	public Score_param (float time1, bool succeed1)
	{
		time = time1;
		succeed = succeed1;
	}
}
