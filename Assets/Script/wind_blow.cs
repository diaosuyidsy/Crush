using UnityEngine;
using System.Collections;

public class wind_blow : MonoBehaviour
{

	public Vector2 offset;

	void OnCollisionStay2D (Collision2D other)
	{
		if (other.gameObject.tag == "Bullet") {
			Vector2 v2 = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			v2 += offset;
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = v2;
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "Bullet") {
			Vector2 v2 = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			v2 += offset;
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = v2;
		}
	}
}
