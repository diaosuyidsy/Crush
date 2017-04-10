using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StepLayout : MonoBehaviour
{
	#region Variable

	public Image greenStep;
	public Image yellowStep;
	public Image redStep;
	public Image ThreeStar;
	public Image TwoStar;
	public Image OneStar;

	public GameObject placeHolder;

	private Image step;
	private int pointer;
	private int MAXStep;
	private Image three;
	private Image two;
	private Image one;
	private GridLayoutGroup grid;
	private int Three_Star_Step;
	private int Two_Star_Step;
	private int One_Star_Step;
	public Vector3 offset;

	#endregion

	// Use this for initialization
	void Start ()
	{
		GameObject gm = GameObject.FindGameObjectWithTag ("GameManager");
		Three_Star_Step = gm.GetComponent <Controller> ().three_star_step;
		Two_Star_Step = gm.GetComponent <Controller> ().two_star_step;
		One_Star_Step = gm.GetComponent <Controller> ().one_star_step;
		MAXStep = Three_Star_Step + Two_Star_Step + One_Star_Step;
		pointer = 0;
		grid = GetComponent <GridLayoutGroup> ();
		for (int i = 0; i < Three_Star_Step; i++) {
			step = (Image)Instantiate (greenStep);
			step.transform.SetParent (transform, false);
		}
			
		// Instatiate the three Star below it
		three = (Image)Instantiate (ThreeStar, new Vector3 (0, -Three_Star_Step * grid.cellSize.y), Quaternion.identity);
		three.transform.SetParent (placeHolder.transform, false);
		for (int i = 0; i < Two_Star_Step; i++) {
			step = (Image)Instantiate (yellowStep);
			step.transform.SetParent (transform, false);
		}
		two = (Image)Instantiate (TwoStar, new Vector3 (0, -(Three_Star_Step + Two_Star_Step) * grid.cellSize.y), Quaternion.identity);
		two.transform.SetParent (placeHolder.transform, false);
		for (int i = 0; i < One_Star_Step; i++) {
			step = (Image)Instantiate (redStep);
			step.transform.SetParent (transform, false);
		}
		one = (Image)Instantiate (OneStar, new Vector3 (0, -(Three_Star_Step + Two_Star_Step + One_Star_Step + 0.5f) * grid.cellSize.y), Quaternion.identity);
		one.transform.SetParent (placeHolder.transform, false);
	}

	public void pushFirstChild ()
	{
//		transform.GetChild (pointer).gameObject.SetActive (false);
		transform.GetChild (pointer).gameObject.GetComponent<Image> ().color = Color.gray;

		pointer++;
		Debug.Assert (pointer <= MAXStep + 1);
//		three.gameObject.transform.localPosition += new Vector3 (0, grid.cellSize.y);
//		two.gameObject.transform.localPosition += new Vector3 (0, grid.cellSize.y);
//		one.gameObject.transform.localPosition += new Vector3 (0, grid.cellSize.y);

		if (pointer == Three_Star_Step) {
//			three.gameObject.SetActive (false);
			three.gameObject.GetComponent<Image> ().color = Color.gray;
		} else if (pointer == Two_Star_Step + Three_Star_Step) {
//			two.gameObject.SetActive (false);
			two.gameObject.GetComponent<Image> ().color = Color.gray;
		} else if (pointer == One_Star_Step + Two_Star_Step + Three_Star_Step) {
//			one.gameObject.SetActive (false);
			one.gameObject.GetComponent<Image> ().color = Color.gray;
		}

	}
}
