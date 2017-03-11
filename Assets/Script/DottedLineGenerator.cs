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
		for (int i = 0; i < numOfTrajectoryPoints; i++) {
			float dy = fTime * distance_between_dot * Mathf.Cos (angle * 2);
			float dx = -fTime * distance_between_dot * Mathf.Sin (angle * 2);
			Vector3 pos = new Vector3 (pStartPosition.x + dx - distance_to_head_of_launcher * Mathf.Sin (angle * 2), pStartPosition.y + distance_to_head_of_launcher * Mathf.Cos (angle * 2) + dy, 2);
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
			// If a blocking area show up, don't go on anymore
			Collider2D hitcollider = Physics2D.OverlapCircle (pos, 1);
			if(hitcollider != null && hitcollider.gameObject.tag == "Brick"){
				for(;i < numOfTrajectoryPoints; i++){
					trajectoryPoints [i].GetComponent<SpriteRenderer> ().enabled = false;
				}
				break;
			}
			// If encountered a wind area, bend
			// TODO

			pos = Camera.main.ScreenToWorldPoint(tempos);
			trajectoryPoints[i].transform.position = pos;
			trajectoryPoints[i].GetComponent<SpriteRenderer> ().enabled = true;
			fTime += 0.1f;
		}
	}
}
