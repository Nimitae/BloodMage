using UnityEngine;
using System.Collections;

public class rangedEnemyAttack : MonoBehaviour {
	[HideInInspector]
	public float attackDamage;
	[HideInInspector]
	public float projectileSpeed;

	private Rigidbody2D rigid;
	private ResourceLogic resLogic;

	void Start()
	{
		rigid = gameObject.GetComponent<Rigidbody2D> ();
		resLogic = GameObject.Find ("GameManager").GetComponent<ResourceLogic> ();
	}
	
	void Update()
	{
		Vector3 moveVector = transform.right * projectileSpeed;
		//	rigid.AddForce(new Vector2(moveVector.x , moveVector.y));
		rigid.velocity = new Vector2 (moveVector.x, moveVector.y);
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Player") {
			resLogic.monsterDealDamage(attackDamage);
			Destroy(gameObject);
		} else if (col.transform.tag == "Boundary") {
			Destroy(gameObject);
		}
	}

}
