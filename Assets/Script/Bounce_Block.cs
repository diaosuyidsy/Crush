using UnityEngine;
using System.Collections;

public class Bounce_Block : MonoBehaviour {

	private BoxCollider2D _collider;
	private Vector3 _center;
	private Vector3 _extents;
	private Vector3 _normal;
	private Vector3 _colliderVec;

	void Start(){
		_collider = GetComponent<BoxCollider2D> ();
		_center = _collider.bounds.center;
		_extents = _collider.bounds.extents;
		Vector3 tmp1 = new Vector3 (_center.x + _extents.x, _center.y + _extents.y - _collider.size.y * 0.5f);
		Vector3 tmp2 = new Vector3 (_center.x - _extents.x, _center.y - _extents.y + _collider.size.y * 0.5f);
		float rotation = transform.localRotation.eulerAngles.z;
		bool flip = (rotation > 90 && rotation <= 180) ||
		            (rotation > 270 && rotation < 360) ? true : false;
		_colliderVec = tmp1 - tmp2;
		Vector2 temp = new Vector2 (-_colliderVec.y, _colliderVec.x).normalized;
		if (flip) {
			temp = new Vector2 (temp.x, -temp.y);
		}
		_normal = new Vector3 (temp.x, temp.y, 0);
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Bullet"){
			Vector3 ve = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			Vector3 vp = 2 * Vector3.Dot (ve, _normal) * _normal - ve;
//			Debug.Log (ve);
//			Debug.Log (vp);
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = -1 * vp;
		}
	}
}
