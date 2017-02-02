using UnityEngine;
using System.Collections;

public class HorizontalMove : MonoBehaviour {

	private bool dirRight = false;
	public float speed = 2.0f;
	private float RightEdge;
	private float LeftEdge;

	void Start(){
		RightEdge = Camera.main.orthographicSize * Screen.width / Screen.height;
		LeftEdge = -1 * RightEdge;
	}
	// Update is called once per frame
	void Update () {
		if (dirRight)
			transform.Translate (Vector2.right * speed * Time.deltaTime);
		else
			transform.Translate (-Vector2.right * speed * Time.deltaTime);

		if(transform.position.x >= RightEdge) {
			dirRight = false;
		}

		if(transform.position.x <= LeftEdge) {
			dirRight = true;
		}
	}
}
