using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	public Text score_text;
	public float velocity_text;
	private float initial_score = 0f;
	private float smooth_time = 1f;

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
		// Display score
		StartCoroutine (score_to (final_score));
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

	IEnumerator score_to(float target)
	{
		float start = initial_score;
		for(float timer = 0; timer < smooth_time; timer += Time.deltaTime)
		{
			float progress = timer / smooth_time;
			initial_score = (float)Mathf.Lerp (start, target, progress);
			int display = (int)Mathf.Floor (initial_score * 10);
			score_text.text = display.ToString () + " / 1000";
			yield return null;
		}

	}
}
