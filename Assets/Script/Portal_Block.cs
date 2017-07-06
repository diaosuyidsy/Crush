using UnityEngine;
using System.Collections;

public class Portal_Block : MonoBehaviour
{

	public Transform receiver;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Bullet") {
			other.gameObject.transform.position = receiver.position;
		}
	}
}
