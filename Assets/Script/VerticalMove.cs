using UnityEngine;
using System.Collections;

public class VerticalMove : MonoBehaviour {

	private bool dirUp = false;
	public float speed = 10f;
	private float UpEdge;
	private float DownEdge;

	void Start(){
//		UpEdge = Camera.main.orthographicSize * Screen.height / Screen.width;
		UpEdge = Camera.main.orthographicSize;
		DownEdge = 0;
	}
	// Update is called once per frame
	void Update () {
		if (dirUp)
			transform.Translate (Vector2.up * speed * Time.deltaTime);
		else
			transform.Translate (-Vector2.up * speed * Time.deltaTime);

		if(transform.position.y >= UpEdge) {
			dirUp = false;
		}

		if(transform.position.y <= DownEdge) {
			dirUp = true;
		}
	}
}
