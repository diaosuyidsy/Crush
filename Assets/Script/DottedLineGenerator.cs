using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DottedLineGenerator : MonoBehaviour {
	#region variable

	public GameObject TrajectoryPointPrefeb;
	public float distance_to_head_of_launcher;
	public float distance_between_dot;

	public int numOfTrajectoryPoints = 30;
	private List<GameObject> trajectoryPoints;
	private float bulletSpeed;

	#endregion

	void Start ()
	{
		//get the bullet speed
		bulletSpeed = GetComponent<LauncherControl> ().bulletSpeed;
		trajectoryPoints = new List<GameObject> ();
		// TrajectoryPoints are instatiated
		for(int i=0;i<numOfTrajectoryPoints;i++)
		{
			GameObject dot= (GameObject) Instantiate(TrajectoryPointPrefeb);
			dot.GetComponent<SpriteRenderer>().enabled = true;
			trajectoryPoints.Insert(i,dot);
		}
	}

	void Update ()
	{
		setTrajectoryPoints(transform.position, transform.rotation.eulerAngles.z);
	}

	void setTrajectoryPoints(Vector3 pStartPosition, float angle)
	{
		float fTime = 0f;
		fTime += 0.1f;
		Vector2 bulletVelocity = new Vector2 (-bulletSpeed * Mathf.Sin (transform.localRotation.z * 2), bulletSpeed * Mathf.Cos (transform.localRotation.z * 2));
		Vector3 wind_center = Vector3.zero;
		Vector3 wind_extents = Vector3.zero;
		float temp_dis = distance_to_head_of_launcher;
		bool first_time_portal = true;
		for (int i = 0; i < numOfTrajectoryPoints; i++) {
			float dy = fTime * distance_between_dot * Mathf.Cos (Mathf.Deg2Rad * angle);
			float dx = -fTime * distance_between_dot * Mathf.Sin (Mathf.Deg2Rad * angle);
			Vector3 pos = new Vector3 (pStartPosition.x + dx - temp_dis * Mathf.Sin (Mathf.Deg2Rad * angle), 
				pStartPosition.y + temp_dis * Mathf.Cos (Mathf.Deg2Rad * angle) + dy, 2);

			// If a blocking area show up, don't render anymore
			Collider2D hitcollider = Physics2D.OverlapCircle (pos, 0.4f);
			if(hitcollider != null && hitcollider.gameObject.tag == "Target"){
				for(;i < numOfTrajectoryPoints; i++){
					trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
				}
				break;
			}
			if(hitcollider != null && hitcollider.gameObject.tag == "Brick"){
				for(;i < numOfTrajectoryPoints; i++){
					trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
				}
				break;
			}
			if(hitcollider != null && hitcollider.gameObject.tag == "Bounce"){
				BoxCollider2D boxcollider = (BoxCollider2D)hitcollider;
				Vector3 _center = boxcollider.bounds.center;
				Vector3 _extents = boxcollider.bounds.extents;
				Vector3 tmp1 = new Vector3 (_center.x + _extents.x, _center.y + _extents.y - boxcollider.size.y * 0.5f);
				Vector3 tmp2 = new Vector3 (_center.x - _extents.x, _center.y - _extents.y + boxcollider.size.y * 0.5f);
				bool firstentry = true;
				for(;i < numOfTrajectoryPoints; i++){
					if(firstentry){
						trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
						firstentry = false;
					}else{
						dy = fTime * distance_between_dot * Mathf.Cos (Mathf.Deg2Rad * angle);
						dx = -fTime * distance_between_dot * Mathf.Sin (Mathf.Deg2Rad * angle);
						pos = new Vector3 (pStartPosition.x + dx - temp_dis * Mathf.Sin (Mathf.Deg2Rad * angle), 
							pStartPosition.y + temp_dis * Mathf.Cos (Mathf.Deg2Rad * angle) + dy, 2);
						float a = tmp2.x - tmp1.x;
						float b = tmp1.y - tmp2.y;
						float c = tmp1.x * tmp2.y - tmp2.x * tmp1.y;
						float p = pos.x, q = pos.y;
						Vector3 newPos = new Vector3 ((p * (a * a - b * b) - 2 * b * (a * q + c)) / (a * a + b * b), 
							(q * (b * b - a * a) - 2 * a * (b * p + c)) / (a * a + b * b)); // Calculate the mirro point
						trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = true;
						trajectoryPoints [i].transform.position = newPos;
					}
					fTime += 0.1f;
				}
				break;
				//TODO
			}
			if(hitcollider != null &&  hitcollider.gameObject.tag == "Portal_Sender"){
				pStartPosition = hitcollider.gameObject.GetComponent <Portal_Block> ().receiver.position;
				temp_dis = 0;
				if(first_time_portal){
					fTime = 0.1f;
					first_time_portal = false;
				}
			}
			// If encountered a wind area, bend
			if(hitcollider != null && hitcollider.gameObject.tag == "Windzone"){

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
		}
	}
}
