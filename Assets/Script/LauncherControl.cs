using UnityEngine;
using System.Collections;
using Spine.Unity;

public class LauncherControl : MonoBehaviour
{

	#region Variable

	public bool dragging = false;
	public GameObject bullet;
	public float bulletSpeed = 10f;
	public GameObject cannon_smoke_particel;
	public float smoke_distance_to_cannon;
	public SkeletonAnimation skeletonAnimation;

	private Vector3 screenPoint;
	private Vector3 offset;

	private bool Lock = false;
	private float check_click = 0f;

	#endregion

	void OnMouseDown ()
	{
		check_click = Time.time;
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, transform.position.y, screenPoint.z));
	}

	void OnMouseDrag ()
	{
		float time_diff = Time.time - check_click;
		if(time_diff > 0.15f && !Lock)
		{
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, transform.position.y, screenPoint.z);

			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}

	void OnMouseUp ()
	{
		float time_diff = Time.time - check_click;
		if(time_diff <= 0.15f){
			if (!Lock) {
				Lock = true;
				//set animation
				skeletonAnimation.AnimationName = "animation";
				GameObject bullet1 = (GameObject)Instantiate (bullet, transform.position, Quaternion.identity);
				// Also, initiate the smoke particel effect
				GameObject smoke_effect = (GameObject)Instantiate (cannon_smoke_particel, transform.position + new Vector3 (0, smoke_distance_to_cannon, 0), Quaternion.identity);
				smoke_effect.GetComponent<Renderer> ().sortingOrder = 10;
				Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D> ();
				rb.velocity = new Vector2 (0, bulletSpeed);
			}
		}

	}
}
