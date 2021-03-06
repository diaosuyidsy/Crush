﻿using UnityEngine;
using System.Collections;
using Spine.Unity;

public class LauncherControl : MonoBehaviour
{

	#region Variable

	public GameObject bullet;
	public float bulletSpeed = 10f;
	public GameObject cannon_smoke_particel;
	public float smoke_distance_to_cannon;
	public SkeletonAnimation skeletonAnimation;
	public bool can_rotate = true;
	public bool can_move_horizontal = true;
	public bool can_move_vertical = false;

	private Vector3 screenPoint;
	private Vector3 offset;

	private bool Lock = false;
	private float check_click = 0f;

	private string edit_type = "";

	#endregion

	void OnEnable ()
	{
//		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller> ().setShadow ();
	}

	public void unlock ()
	{
		Lock = false;
		skeletonAnimation.AnimationName = null;
	}

	void Update ()
	{
		foreach (Touch touch in Input.touches) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero);
			if (hit != null && hit.collider != null && hit.collider.gameObject == this.gameObject) {
				if (touch.phase == TouchPhase.Began) {
					OMDo ();
				} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					OMDr ();
				} else if (touch.phase == TouchPhase.Ended) {
					OME ();
				}
			}

		}
	}

	#if UNITY_EDITOR_OSX
	void OnMouseDown ()
	{
		OMDo ();
	}

	void OnMouseDrag ()
	{
		OMDr ();
	}

	void OnMouseUp ()
	{
		OME ();
	}
	#endif

	void OMDo ()
	{
		if (!Lock) {
			check_click = Time.time;
			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			Vector3 v3 = Input.mousePosition;
			Vector3 v4 = Camera.main.ScreenToWorldPoint (v3);
			Collider2D hitcollider = Physics2D.OverlapCircle (v4, 0.1f);

			if (hitcollider.gameObject.tag == "Rotation") {
				edit_type = "rotate";
			} else if (hitcollider.gameObject.tag == "Launcher") {
				edit_type = "transform";
			}
			if (can_move_horizontal && !can_move_vertical) {
				offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, transform.position.y, screenPoint.z));
			} else if (can_move_vertical && !can_move_horizontal) {
				offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (transform.position.x, Input.mousePosition.y, screenPoint.z));
			} else if (can_move_vertical && can_move_horizontal) {
				offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			}
		}
	}

	void OMDr ()
	{
		float time_diff = Time.time - check_click;
		if (time_diff > 0.15f && !Lock) {
			if (edit_type == "rotate" && can_rotate) {
//				Vector3 mouse_pos = Input.mousePosition;
//				mouse_pos.z = 0;
//				Vector3 object_pos = Camera.main.WorldToScreenPoint (transform.position);
//				mouse_pos.x = mouse_pos.x - object_pos.x;
//				mouse_pos.y = mouse_pos.y - object_pos.y;
//				float angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
//				transform.rotation = Quaternion.Euler (0, 0, angle);
			} else if (edit_type == "transform" && (can_move_horizontal || can_move_vertical)) {
				Vector3 curScreenPoint = new Vector3 (transform.position.x, transform.position.y, screenPoint.z);
				if (can_move_horizontal && !can_move_vertical) {
					curScreenPoint = new Vector3 (Input.mousePosition.x, transform.position.y, screenPoint.z);
				} else if (can_move_vertical && !can_move_horizontal) {
					curScreenPoint = new Vector3 (transform.position.x, Input.mousePosition.y, screenPoint.z);
				} else if (can_move_horizontal && can_move_vertical) {
					curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
				}

				Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
				transform.position = curPosition;
			}
		}

	}

	void OME ()
	{
		float time_diff = Time.time - check_click;
		if (time_diff <= 0.15f) {
			if (!Lock) {
				Lock = true;
				//set animation
				skeletonAnimation.AnimationName = "animation";
				GameObject bullet1 = (GameObject)Instantiate (bullet, transform.position, Quaternion.identity);
				// Also, initiate the smoke particel effect
				GameObject smoke_effect = (GameObject)Instantiate (cannon_smoke_particel, transform.position + new Vector3 (0, smoke_distance_to_cannon, 0), Quaternion.identity);
				smoke_effect.GetComponent<Renderer> ().sortingOrder = 10;
				Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D> ();
				rb.gravityScale = 0f;
				rb.velocity = new Vector2 (-bulletSpeed * Mathf.Sin (transform.rotation.eulerAngles.z * Mathf.Deg2Rad), bulletSpeed * Mathf.Cos (transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
			}
		}
	}
}
