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
//	private bool coroutine=false;

	#endregion

	void Start ()
	{
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
		setTrajectoryPoints(transform.position);

//		if(!coroutine){
//			StartCoroutine (moveTrajectoryPoints ());
//			coroutine = true;
//		}

	}

	void setTrajectoryPoints(Vector3 pStartPosition)
	{
		
		float fTime = 0f;
		fTime += 0.1f;
		for (int i = 0 ; i < numOfTrajectoryPoints ; i++)
		{
			float dy = fTime * distance_between_dot;
			Vector3 pos = new Vector3(pStartPosition.x, pStartPosition.y + distance_to_head_of_launcher + dy ,2);
			trajectoryPoints[i].transform.position = pos;
			trajectoryPoints[i].GetComponent<SpriteRenderer> ().enabled = true;
			fTime += 0.1f;
		}
	}

//	IEnumerator moveTrajectoryPoints()
//	{
//		yield return new WaitForSeconds (0.05f);
//
//		foreach(GameObject dot in trajectoryPoints)
//		{
//			Vector3 pos = new Vector3 (dot.transform.position.x, dot.transform.position.y, dot.transform.position.z);
//			pos.y += 0.5f;
//			dot.transform.position = pos;
//		}
//
//		StartCoroutine (moveTrajectoryPoints ());
//	}
}
