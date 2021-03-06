﻿using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System;


public class DottedLineGenerator : MonoBehaviour
{
	#region variable

	public GameObject TrajectoryPointPrefeb;
	public float distance_to_head_of_launcher;
	public float distance_between_dot;

	public int numOfTrajectoryPoints = 30;
	private List<GameObject> trajectoryPoints;
	private float bulletSpeed;
	private float leftScreen;
	private float t;

	#endregion

	void Start ()
	{
		//Set left screen registration
		leftScreen = Camera.main.WorldToScreenPoint (GameObject.FindGameObjectWithTag ("Registration").transform.position).x;
		//get the bullet speed
		bulletSpeed = GetComponent<LauncherControl> ().bulletSpeed;
		trajectoryPoints = new List<GameObject> ();
		// TrajectoryPoints are instatiated
		for (int i = 0; i < numOfTrajectoryPoints; i++) {
			GameObject dot = (GameObject)Instantiate (TrajectoryPointPrefeb);
			dot.GetComponent<SpriteRenderer> ().enabled = true;
			trajectoryPoints.Insert (i, dot);
		}
	}

	void Update ()
	{
		setTrajectoryPoints (transform.position, transform.rotation.eulerAngles.z);
	}

	public void metTarget (int i)
	{
		trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
	}

	public void metBrick (int i)
	{
		trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
	}

	public void metBounce (int i, Collider2D hitcollider, ref Vector3 pos, ref bool first_time)
	{
		BoxCollider2D boxcollider = (BoxCollider2D)hitcollider;
		Vector3 _center = boxcollider.bounds.center;
		Vector3 _extents = boxcollider.bounds.extents;
		float z = hitcollider.gameObject.transform.eulerAngles.z;
		if (z < 0) {
			z += 360f;
		}
		float upsidedown = ((z > 90 && z < 180) || (z > 270 && z < 360)) ? -1f : 1f;
		Vector3 tmp1 = new Vector3 (_center.x + _extents.x, _center.y + upsidedown * _extents.y);
		Vector3 tmp2 = new Vector3 (_center.x - _extents.x, _center.y - upsidedown * _extents.y);
		float a = tmp2.x - tmp1.x;
		float b = tmp1.y - tmp2.y;
		float c = tmp1.x * tmp2.y - tmp2.x * tmp1.y;
		float p = pos.x, q = pos.y;

		Vector3 newPos = new Vector3 ((p * (a * a - b * b) - 2 * b * (a * q + c)) / (a * a + b * b), 
			                 (q * (b * b - a * a) - 2 * a * (b * p + c)) / (a * a + b * b)); // Calculate the mirro point
		trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = true;
		if (first_time) {
			first_time = false;
			trajectoryPoints [i - 1].GetComponent<SpriteRenderer> ().enabled = false;
		}
		pos = newPos;
	}

	public void metPortalSender (int i, Collider2D hitcollider, ref float temp_dis, ref Vector3 pos, ref bool first_time_portal, ref float fTime)
	{
		Vector3 temp_distance = pos - hitcollider.gameObject.transform.position;
		pos = hitcollider.gameObject.GetComponent<Portal_Block> ().receiver.position + temp_distance;

	}

	//	public void metWindzone (int i, Collider2D hitcollider, ref Vector3 pos, Vector3 bulletVelocity, Vector3 wind_center, Vector3 wind_extents)
	//	{
	////		float theta = transform.localRotation.z * 2.5f;
	//		float theta = transform.eulerAngles.z * Mathf.Deg2Rad;
	//		float s1 = pos.y - hitcollider.bounds.center.y + hitcollider.bounds.extents.y;
	//		float a = hitcollider.gameObject.GetComponent<wind_blow> ().offset.x;
	//
	//		float vx = bulletVelocity.x;
	//		float vy = bulletVelocity.y;
	//		float t1 = s1 / (Mathf.Cos (theta) * Mathf.Sqrt (vx * vx + vy * vy));
	//		float s2 = 30f * a * t1 * t1 + vx * t1;
	//
	//		pos = new Vector3 (pos.x + s2, pos.y, pos.z);
	//		wind_center = hitcollider.bounds.center;
	//		wind_extents = hitcollider.bounds.extents;
	//	}

	public void metWindzone (int i, Collider2D hitcollider, ref Vector3 pos, Vector3 bulletVelocity, Vector3 wind_center, Vector3 wind_extents, ref bool first_time, ref Vector3 wind_zone_enter_pos)
	{
		Vector2 windForce = hitcollider.gameObject.GetComponent<wind_blow> ().offset;
		wind_center = hitcollider.bounds.center;
		wind_extents = hitcollider.bounds.extents;

		if (first_time) {
			first_time = false;

			Vector2 leftP = new Vector2 (wind_center.x - wind_extents.x, wind_center.y - wind_extents.y);
			Vector2 rightP = new Vector2 (wind_center.x + wind_extents.x, wind_center.y - wind_extents.y);
			wind_zone_enter_pos = Math3d.LineIntersectionPoint (new Vector2 (pos.x, pos.y), new Vector2 (transform.position.x, transform.position.y), leftP, rightP);

			t = (Vector2.Distance (wind_zone_enter_pos, new Vector2 (pos.x, pos.y)) / Vector3.Distance (pos, trajectoryPoints [i - 1].transform.position)) * 0.1f;
		}
		t += 0.1f;
		float c = wind_zone_enter_pos.x + 0.5f * windForce.x * t * t + bulletVelocity.x;
		float d = wind_zone_enter_pos.y + 0.5f * windForce.y * t * t + bulletVelocity.y;
		pos = new Vector3 (c, d);
	}

	public void metBound (int i, Collider2D hitcollider, ref Vector3 pos)
	{
		Vector3 tempos = Camera.main.WorldToScreenPoint (pos);
		// Set tragectory point to reflect
		if (tempos.x <= 0) {
			tempos.x = -tempos.x;
		}
		if (tempos.x >= Screen.width) {
			tempos.x = 2 * Screen.width - tempos.x;
		}
		if (tempos.y >= Screen.height) {
			tempos.y = 2 * Screen.height - tempos.y;
		}
		pos = Camera.main.ScreenToWorldPoint (tempos);
	}

	void setTrajectoryPoints (Vector3 pStartPosition, float angle)
	{
		float fTime = 0f;
		fTime += 0.1f;
		Vector2 bulletVelocity = new Vector2 (-bulletSpeed * Mathf.Sin (transform.localRotation.z * 2), bulletSpeed * Mathf.Cos (transform.localRotation.z * 2));
		Vector3 wind_center = Vector3.zero;
		Vector3 wind_extents = Vector3.zero;
		float temp_dis = distance_to_head_of_launcher;
		bool first_time_portal = true;
		bool first_time_met_bound = true;
		bool first_time_met_Target = true;
		bool first_time_met_Brick = true;
		bool first_time_met_Bounce = true;
		bool first_time_wind_zone = true;
		Vector3 wind_zone_enter_pos = Vector3.zero;
		List<string> methodQueue = new List<string> ();
		Collider2D hitcollider = null;
		List<Collider2D> temp = new List<Collider2D> ();
		for (int i = 0; i < numOfTrajectoryPoints; i++) {
			trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = true;
			float dy = fTime * distance_between_dot * Mathf.Cos (Mathf.Deg2Rad * angle);
			float dx = -fTime * distance_between_dot * Mathf.Sin (Mathf.Deg2Rad * angle);
			Vector3 pos = new Vector3 (pStartPosition.x + dx - temp_dis * Mathf.Sin (Mathf.Deg2Rad * angle), 
				              pStartPosition.y + temp_dis * Mathf.Cos (Mathf.Deg2Rad * angle) + dy, 2);
			if (methodQueue.Count > 0) {
				int hitCount = 0;
				foreach (string method in methodQueue) {
					switch (method) {
					case "metTarget":
						metTarget (i);
						break;
					case "metBrick":
						metBrick (i);
						break;
					case "metBounce":
						metBounce (i, temp [hitCount], ref pos, ref first_time_met_Bounce);
						hitCount++;
						break;
					case "metPortalSender":
						metPortalSender (i, temp [hitCount], ref temp_dis, ref pos, ref first_time_portal, ref fTime);
						hitCount++;
						break;
					case "metWindzone":
						metWindzone (i, temp [hitCount], ref pos, bulletVelocity, wind_center, wind_extents, ref first_time_wind_zone, ref wind_zone_enter_pos);
						hitCount++;
						break;
					case "metBound":
						metBound (i, hitcollider, ref pos);
						break;
					}
					if (method == "metBrick" || method == "metTarget") {
						break;
					}
				}
			}
			hitcollider = Physics2D.OverlapCircle (pos, 0.4f);
			if (hitcollider != null && hitcollider.gameObject.tag == "Target") {
				methodQueue.Add ("metTarget");
				if (first_time_met_Target) {
					metTarget (i);
					first_time_met_Target = false;
				}
			}
			if (hitcollider != null && hitcollider.gameObject.tag == "Brick") {
				methodQueue.Add ("metBrick");
				if (first_time_met_Brick) {
					metBrick (i);
					first_time_met_Brick = false;
				}
			}
			if (hitcollider != null && hitcollider.gameObject.tag == "Bounce") {
				if (temp.Count == 0 || temp [temp.Count - 1] != hitcollider) {
					temp.Add (hitcollider);
					methodQueue.Add ("metBounce");
				}
			}
			if (hitcollider != null && hitcollider.gameObject.tag == "Portal_Sender") {
				if (temp.Count == 0 || temp [temp.Count - 1] != hitcollider) {
					temp.Add (hitcollider);
					methodQueue.Add ("metPortalSender");
				}
			}
			// If encountered a wind area, bend
			if (hitcollider != null && hitcollider.gameObject.tag == "Windzone") {
				if (temp.Count == 0 || temp [temp.Count - 1] != hitcollider) {
					temp.Add (hitcollider);
					methodQueue.Add ("metWindzone");
				}
			}

			// If leave wind area, be normal
//			if (wind_extents.x != 0 && wind_extents.y != 0) {
//				if (wind_center.y + wind_extents.y <= pos.y && wind_center.y + wind_extents.y >= trajectoryPoints [i - 1].transform.position.y) {
//					for (; i < numOfTrajectoryPoints; i++) {
//					}
//					break;
//				}
//			}

			Vector3 tempos = Camera.main.WorldToScreenPoint (pos);
//			 Set tragectory point to reflect
			if (tempos.x <= leftScreen || tempos.x >= Screen.width
			    || tempos.y >= Screen.height) {
				methodQueue.Add ("metBound");
				if (first_time_met_bound) {
					metBound (i, hitcollider, ref pos);
					first_time_met_bound = false;
				}
			}

			trajectoryPoints [i].transform.position = pos;
			fTime += 0.1f;
		}
	}
}
