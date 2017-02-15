using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

	public int LevelNum;

	public Transform launcher1;
	public Transform launcher2;
	public Transform launcher3;

	public GameObject bullet;

	public float bulletSpeed = 6f;

	public GameObject explode;

	public GUIAnimFREE FirstStar;
	public GUIAnimFREE SecondStar;
	public GUIAnimFREE ThirdStar;

	public GUIAnimFREE Level_Clear_Panel;
	public GUIAnimFREE Level_Fail_Panel;
	public GameObject Need_Disable_When_Win;

	private bool lock1=true;
	private bool lock2=true;
	private bool lock3=true;

	private List<float> timeContainer;
	public int countForLevel = 0;

	void Start(){
		timeContainer = new List<float>();
		string explosion_prefab_name = GameData.gd.Load_Equipped_Item_name ();
		if(explosion_prefab_name != null)
		{
			explode = (GameObject) Resources.Load ("Explosion_Effect/" + explosion_prefab_name, typeof(GameObject));
		}


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
		if(countForLevel==3){
			CalculateScore ();
		}

	}

	private void CalculateScore(){
		// Enable Level Clear panel
		// Disable Pause Button
		Need_Disable_When_Win.SetActive (false);

		float score = 0;
		float lasti = timeContainer [0];
		foreach(float i in timeContainer){
			score += (i - lasti);
			lasti = i;
		}
		Debug.Log ("Final Score: " + (100f - score * 100));
		float finalScore = 100f - score * 100;
		if(finalScore > 40){
			PassLevel (finalScore);
		}else{
			FailLevel ();
		}
	}

	void PassLevel(float final_score)
	{
		int starnum = 1;
		Level_Clear_Panel.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		if(final_score > 84){
			starnum = 3;
			FirstStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
			SecondStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
			ThirdStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
		}else if(final_score > 60){
			starnum = 2;
			FirstStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
			SecondStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
		}else if(final_score > 40){
			FirstStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
		}
		Save (final_score, starnum);
	}

	void FailLevel(){
		Level_Fail_Panel.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}

	void Save(float final_score, int starnum)
	{
		GameData.gd.setLevelInfo (LevelNum, starnum, final_score);
		GameData.gd.Save ();
		GameData.gd.setLevelInfo (LevelNum + 1, 0, 0);
		GameData.gd.Save ();
	}
}
