using UnityEngine;
using System.Collections;

public class SpellTwoProjectile : MonoBehaviour {
//	[HideInInspector]
	public float projectileDamage;
//	[HideInInspector]
	public float projectileSpeed;
//	[HideInInspector]
	public float penetrationLimit;
//	[HideInInspector]
	public float projectileForce;

	private Rigidbody2D rigid;

	private float enemiesHit;

	void Start()
	{
		enemiesHit = 0;
		rigid = gameObject.GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		Vector3 moveVector = transform.right * projectileSpeed;
	//	rigid.AddForce(new Vector2(moveVector.x , moveVector.y));
		rigid.velocity = new Vector2 (moveVector.x, moveVector.y);
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Enemy") {
			col.gameObject.GetComponent<IEnemy>().takeDamage(projectileDamage);
			enemiesHit++;
			if (enemiesHit >= penetrationLimit){
				Destroy (gameObject);
			}
		} else if (col.transform.tag == "Ground") {
			Destroy (gameObject);
		}
	}
}
