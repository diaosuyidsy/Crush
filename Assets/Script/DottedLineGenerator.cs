using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DottedLineGenerator : MonoBehaviour {
	#region variable

	public GameObject TrajectoryPointPrefeb;
	public float distance_to_head_of_launcher;
	public float distance_between_dot;

	private int numOfTrajectoryPoints = 30;
	private List<GameObject> trajectoryPoints;
	private int obstacleLayer = 9;
	private int obstacleLayerMask;
	private float bulletSpeed;

	#endregion

	void Start ()
	{
		obstacleLayerMask = 1 << obstacleLayer;
		//get the bullet speed
		bulletSpeed = GetComponent<LauncherControl> ().bulletSpeed;
		trajectoryPoints = new List<GameObject> ();
		// TrajectoryPoints are instatiated
		for(int i=0;i<numOfTrajectoryPoints;i++)
		{
			GameObject dot= (GameObject) Instantiate(TrajectoryPointPrefeb);
			dot.GetComponent<SpriteRenderer>().enabled = false;
			trajectoryPoints.Insert(i,dot);
		}
	}

	void Update ()
	{
		setTrajectoryPoints(transform.position, transform.localRotation.z);
	}

	void setTrajectoryPoints(Vector3 pStartPosition, float angle)
	{
		float fTime = 0f;
		fTime += 0.1f;
		Vector2 bulletVelocity = new Vector2 (-bulletSpeed * Mathf.Sin (transform.localRotation.z * 2), bulletSpeed * Mathf.Cos (transform.localRotation.z * 2));
		Vector3 wind_center = Vector3.zero;
		Vector3 wind_extents = Vector3.zero;
		Collider2D lastcollider = null;
		for (int i = 0; i < numOfTrajectoryPoints; i++) {
			float dy = fTime * distance_between_dot * Mathf.Cos (angle * 2);
			float dx = -fTime * distance_between_dot * Mathf.Sin (angle * 2);
			Vector3 pos = new Vector3 (pStartPosition.x + dx - distance_to_head_of_launcher * Mathf.Sin (angle * 2), 
				pStartPosition.y + distance_to_head_of_launcher * Mathf.Cos (angle * 2) + dy, 2);

			// If a blocking area show up, don't render anymore
			Collider2D hitcollider = Physics2D.OverlapCircle (pos, 0.1f);
			if(hitcollider != null && hitcollider.gameObject.tag == "Brick"){
				for(;i < numOfTrajectoryPoints; i++){
					trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
				}
				break;
			}
			// If encountered a wind area, bend
			// TODO
			else if(hitcollider != null && hitcollider.gameObject.tag == "Windzone"){

				float theta = transform.localRotation.z * 2.5f;
				float s1 = pos.y - hitcollider.bounds.center.y + hitcollider.bounds.extents.y;
				float a = hitcollider.gameObject.GetComponent<wind_blow> ().offset.x;

				float vx = bulletVelocity.x;
				float vy = bulletVelocity.y;
				float t1 = s1 / (Mathf.Cos (theta) * Mathf.Sqrt (vx * vx + vy * vy));
				float s2 = 30f * a * t1 * t1 + vx * t1;

				pos = new Vector3 (pos.x + s2, pos.y, pos.z);
				wind_center = hitcollider.bounds.center;
				wind_extents = hitcollider.bounds.extents;
			}

			// If leave wind area, be normal
			if(wind_extents.x != 0 && wind_extents.y != 0){
				if(wind_center.y + wind_extents.y <= pos.y && wind_center.y + wind_extents.y >= trajectoryPoints[i - 1].transform.position.y){
//					float theta = transform.localRotation.z * 2.5f;
//					float s1 = pos.y - wind_center.y + wind_extents.y;
//					float a = lastcollider.gameObject.GetComponent<wind_blow> ().offset.x;
//
//					float vx = bulletVelocity.x;
//					float vy = bulletVelocity.y;
//					float t1 = s1 / (Mathf.Cos (theta) * Mathf.Sqrt (vx * vx + vy * vy));
//					float s2 = 30f * a * t1 * t1 + vx * t1;
//					pos = new Vector3 (pos.x + s2, pos.y, pos.z);
//
//
//					Vector3 dir = pos - trajectoryPoints [i - 1].transform.position;
////					Debug.Log (pos);
//					float angle1 = Mathf.Atan2 (dir.x, dir.y) * Mathf.Rad2Deg;
//					fTime = 0.1f;
					for(;i < numOfTrajectoryPoints; i++){
//						float dy1 = fTime * distance_between_dot * Mathf.Cos (angle1 * 2);
//						float dx1 = -fTime * distance_between_dot * Mathf.Sin (angle1 * 2);
//						Debug.Log ("dx1: " + dx1 + ". dy1: " + dy1);
//						Vector3 new_pos = new Vector3 (pos.x + dx1, pos.y + dy1, 2);
//						Debug.Log ("new position: " + new_pos);
//						trajectoryPoints [i].transform.position = new_pos;
						trajectoryPoints[i].GetComponent<SpriteRenderer> ().enabled = false;
//						fTime += 0.1f;
					}
					break;
				}
			}

			Vector3 tempos = Camera.main.WorldToScreenPoint (pos);
			// Set tragectory point to reflect
			if (tempos.x <= 0) {
				tempos.x *= -1;
			}
			if (tempos.x >= Screen.width) {
				tempos.x = 2 * Screen.width - tempos.x;
			}
			if (tempos.y >= Screen.height) {
				tempos.y = 2 * Screen.height - tempos.y;
			}
			pos = Camera.main.ScreenToWorldPoint(tempos);



			trajectoryPoints[i].transform.position = pos;
			trajectoryPoints[i].GetComponent<SpriteRenderer> ().enabled = true;
			fTime += 0.1f;
			lastcollider = hitcollider;
		}
	}
}
