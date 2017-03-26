using UnityEngine;
using System.Collections;

public class brick_zone : MonoBehaviour {

	public bool moving;
	public Vector2 PatrolArea;
	public float move_speed;

	private GameObject explode;
	private Vector3 pos1;
	private Vector3 pos2;
	private bool dirVertical = false;
	private bool dirHorizontal = false;
	private bool dirUP = false;
	private bool dirRight = false;


	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Bullet"){
			explode = null;
			explode = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Controller> ().explode;
			Instantiate (explode, this.transform.position, Quaternion.identity );
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

	void Start(){
		pos1 = gameObject.transform.position + new Vector3 (PatrolArea.x, PatrolArea.y);
		pos2 = gameObject.transform.position - new Vector3 (PatrolArea.x, PatrolArea.y);
		if(PatrolArea.x > 0){
			dirHorizontal = true;
		}else if(PatrolArea.y > 0){
			dirVertical = true;
		}
	}

	void Update(){
		if(moving){
			if(dirHorizontal){
				if(transform.position.x >= pos1.x){
					dirRight = false;
				}

				if(transform.position.x <= pos2.x){
					dirRight = true;
				}

				if(dirRight){
					transform.Translate (Vector3.right * move_speed * Time.deltaTime);
				}else{
					transform.Translate (Vector3.left * move_speed * Time.deltaTime);
				}
			}

			if(dirVertical){
				if(transform.position.y >= pos1.y){
					dirUP = false;
				}

				if(transform.position.y <= pos2.y){
					dirUP = true;
				}

				if(dirUP){
					transform.Translate (Vector3.up * move_speed * Time.deltaTime);
				}else{
					transform.Translate (Vector3.down * move_speed * Time.deltaTime);
				}
			}

		}
	}
}
