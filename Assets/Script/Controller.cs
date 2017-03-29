﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{

	private int LevelNum;

	public GameObject[] Launchers;

	public GameObject bullet;

	public float bulletSpeed = 6f;
	public float three_star_bar = 84f;
	public float two_star_bar = 64f;
	public float one_star_bar = 40f;

	public GameObject explode;
	public GameObject shadowDotPrefab;
	public GameObject shadowLauncherPrefab;

	public GUIAnimFREE FirstStar;
	public GUIAnimFREE SecondStar;
	public GUIAnimFREE ThirdStar;

	public GUIAnimFREE Level_Clear_Panel;
	public GUIAnimFREE Level_Fail_Panel;
	public GameObject Need_Disable_When_Win;
	public AudioClip bounceSound;
	public AudioClip explosiveSound;
	private AudioSource audiosource;

	public Text score_text;
	public float velocity_text;
	private float initial_score = 0f;
	private float smooth_time = 1f;
	private int target_num;
	private int countForFail = 0;
	private bool launcherEnable = false;

	private List<float> timeContainer;
	public int countForLevel = 0;

	public List<List<Vector3>> bulletPoses;

	void Start ()
	{
		audiosource = GetComponent<AudioSource> ();
		bulletPoses = new List<List<Vector3>> ();
		LevelNum = SceneManager.GetActiveScene ().buildIndex - 1;
		timeContainer = new List<float> ();
		string explosion_prefab_name = GameData.gd.Load_Equipped_Item_name ();
		if (explosion_prefab_name != null) {
			explode = (GameObject)Resources.Load ("Explosion_Effect/" + explosion_prefab_name, typeof(GameObject));
		}
		setNewLaunchers ();
	}

	public void playSound(string what)
	{
		if(what == "Bounce"){
			audiosource.clip = bounceSound;
		}else if(what == "Explosion"){
			audiosource.clip = explosiveSound;
		}
		audiosource.Play ();
	}

	void Score (Score_param a)
	{
		float time = a.time;
		bool succeed = a.succeed;
		timeContainer.Add (time);
		if (succeed)
			countForLevel++;
		else
			countForFail++;
		if (countForLevel + countForFail >= target_num) {
			if (countForFail > 0) {
				FailLevel ();
			} else {
				CalculateScore ();
			}
			save_launchers_info ();
		}
	}

	private void CalculateScore ()
	{
		// Enable Level Clear panel
		// Disable Pause Button
		Need_Disable_When_Win.SetActive (false);

		float score = 0;
		float lasti = timeContainer [0];
		foreach (float i in timeContainer) {
			score += (i - lasti);
			lasti = i;
		}
		Debug.Log ("Final Score: " + (100f - score * 100));
		float finalScore = 100f - score * 100;
		if (finalScore > one_star_bar) {
			PassLevel (finalScore);
		} else {
			FailLevel ();
		}
	}

	void PassLevel (float final_score)
	{
		int starnum = 1;
		Level_Clear_Panel.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		if (final_score > three_star_bar) {
			starnum = 3;
			FirstStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
			SecondStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
			ThirdStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
		} else if (final_score > two_star_bar) {
			starnum = 2;
			FirstStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
			SecondStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
		} else if (final_score > one_star_bar) {
			FirstStar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);
		}
		// Display score
		StartCoroutine (score_to (final_score));
		Save (final_score, starnum);
	}

	public void FailLevel ()
	{
		Level_Fail_Panel.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}

	void Save (float final_score, int starnum)
	{
		GameData.gd.setLevelInfo (LevelNum, starnum, final_score);
		GameData.gd.Save ();
		GameData.gd.setLevelInfo (LevelNum + 1, 0, 0);
		GameData.gd.Save ();
	}

	IEnumerator score_to (float target)
	{
		float start = initial_score;
		for (float timer = 0; timer <= smooth_time; timer += Time.deltaTime) {
			float progress = timer / smooth_time;
			initial_score = Mathf.Lerp (start, target, progress);
			int display = (int)Mathf.Floor (initial_score * 10);
			score_text.text = display.ToString () + " / 1000";
			yield return null;
		}
	}

	void save_launchers_info ()
	{
		GameObject[] launchers = GameObject.FindGameObjectsWithTag ("Launcher");
		int amount = launchers.Length;
		List<Vector3> positions = new List<Vector3> ();
		List<float> zRs = new List<float> ();
		foreach (GameObject launcher in launchers) {
			positions.Add (launcher.transform.position);
			zRs.Add (launcher.transform.rotation.eulerAngles.z);
		}
		LaunchersInfo li = new LaunchersInfo (amount, positions, zRs, bulletPoses);
		GameData.gd.saveLaunchersInfo (LevelNum, li);

	}

	void target_numAdd1 ()
	{
		target_num++;
	}

	public void setShadow ()
	{
		if (!launcherEnable) {
			launcherEnable = true;
			Dictionary<int, LaunchersInfo> launchersinfomap = GameData.gd.launchersinfomap;
			if (launchersinfomap.ContainsKey (LevelNum)) {
				int i = 0;
				foreach (GameObject launcher in Launchers) {
					GameObject shadowLauncher = (GameObject)Instantiate (shadowLauncherPrefab);
					shadowLauncher.transform.position = launchersinfomap [LevelNum].launchers [i].position;
					shadowLauncher.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, launchersinfomap [LevelNum].launchers [i].zRotation));
					i++;
				}
				foreach (List<Vector3> poslist in launchersinfomap[LevelNum].bulletTracks) {
					foreach (Vector3 pos in poslist) {
						GameObject track = (GameObject.Instantiate (shadowDotPrefab));
						track.transform.position = pos;
					}
				}
					
			}
		}
	}

	void setNewLaunchers ()
	{
		int levelNum = SceneManager.GetActiveScene ().buildIndex - 1;
		Dictionary<int, LaunchersInfo> launchersinfomap = GameData.gd.launchersinfomap;
		if (launchersinfomap.ContainsKey (levelNum)) {
			int i = 0;
			foreach (GameObject launcher in Launchers) {
				launcher.transform.position = launchersinfomap [levelNum].launchers [i].position;
				launcher.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, launchersinfomap [levelNum].launchers [i].zRotation));
				i++;
			}
		}
	}
}
