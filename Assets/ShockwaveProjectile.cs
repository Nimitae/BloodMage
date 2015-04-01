using UnityEngine;
using System.Collections;

public class ShockwaveProjectile : MonoBehaviour {
	[HideInInspector]
	public float projectileDamage;
	[HideInInspector]
	public float projectileSpeed;
	[HideInInspector]
	public float projectileForce;
	[HideInInspector]
	public float projectileDuration;

	private float timeProjectileExpire;
	private Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
		timeProjectileExpire = Time.time + projectileDuration;
		rigid = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVector = transform.right * projectileSpeed;
		//	rigid.AddForce(new Vector2(moveVector.x , moveVector.y));
		rigid.velocity = new Vector2 (moveVector.x, moveVector.y);

		if (Time.time > timeProjectileExpire) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Enemy") {
			col.gameObject.GetComponent<IEnemy> ().takeDamage (projectileDamage);
			Destroy (gameObject);
		} else if (col.transform.tag == "Ground") {
			Destroy (gameObject);
		} else if (col.transform.tag == "Boundary") {
			Destroy(gameObject);
		}
	}
}
