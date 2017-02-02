using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

	public Transform launcher1;
	public Transform launcher2;
	public Transform launcher3;

	public GameObject bullet;

	public float bulletSpeed = 6f;

	public GameObject explode;

	public GameObject OneStar;
	public GameObject TwoStar;
	public GameObject ThreeStar;

	private bool lock1=true;
	private bool lock2=true;
	private bool lock3=true;

	private List<float> timeContainer;
	public int countForLevel = 0;

	void Start(){
		timeContainer = new List<float>();
	}

		
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("Q") && lock1){
			lock1 = false;
			GameObject bullet1 = (GameObject) Instantiate (bullet,launcher1.position,Quaternion.identity);
			Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D> ();
			rb.velocity = new Vector2 (0, bulletSpeed);
		}
		if(Input.GetButtonDown ("W") && lock2){
			lock2 = false;
			GameObject bullet1 = (GameObject) Instantiate (bullet,launcher2.position,Quaternion.identity);
			Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D> ();
			rb.velocity = new Vector2 (0, bulletSpeed);
		}
		if(Input.GetButtonDown ("E") && lock3){
			lock3 = false;
			GameObject bullet1 = (GameObject) Instantiate (bullet,launcher3.position,Quaternion.identity);
			Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D> ();
			rb.velocity = new Vector2 (0, bulletSpeed);
		}

	}

	void Score(float time){
		timeContainer.Add (time);
		countForLevel++;
		Debug.Log ("receice time: " + time + "count = : " + countForLevel);
		if(countForLevel==3){
			CalculateScore ();
		}

	}

	private void CalculateScore(){
		float score = 0;
		float lasti = timeContainer [0];
		foreach(float i in timeContainer){
			score += (i - lasti);
			lasti = i;
		}
		Debug.Log ("Final Score: " + (100f - score * 100));
		float finalScore = 100f - score * 100;
		if(finalScore>84){
			ThreeStar.SetActive (true);
		}else if(finalScore > 60){
			TwoStar.SetActive (true);
		}else{
			OneStar.SetActive (true);
		}
	}

	void FailLevel(){
		
	}
}
